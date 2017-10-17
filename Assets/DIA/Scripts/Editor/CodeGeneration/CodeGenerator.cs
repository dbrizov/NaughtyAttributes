using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

public class CodeGenerator : Editor
{
    private static readonly string GENERATED_CODE_TARGET_FOLDER =
        (Application.dataPath.Replace("Assets", string.Empty) + AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("CodeGenerator")[0]))
        .Replace("CodeGenerator.cs", string.Empty)
        .Replace("/", "\\");

    private static readonly string CLASS_NAME_PLACEHOLDER = "__classname__";
    private static readonly string ENTRIES_PLACEHOLDER = "__entries__";
    private static readonly string DRAWER_ENTRY_FORMAT = "drawersByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine;
    private static readonly string VALIDATOR_ENTRY_FORMAT = "validatorsByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine;

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void GenerateCode()
    {
        GenerateDrawerDatabaseScript("DrawerDatabase", "DrawerDatabaseTemplate");
        GenerateValidatorDatabaseScript("ValidatorDatabase", "ValidatorDatabaseTemplate");

        AssetDatabase.Refresh();
    }

    private static void GenerateDrawerDatabaseScript(string scriptName, string templateName)
    {
        string templateGUID = AssetDatabase.FindAssets(templateName)[0];
        string templateRelativePath = AssetDatabase.GUIDToAssetPath(templateGUID);
        string templateFullPath = (Application.dataPath.Replace("Assets", string.Empty) + templateRelativePath).Replace("/", "\\");
        string templateFormat = IOUtility.ReadFromFile(templateFullPath);

        StringBuilder drawerEntriesBuilder = new StringBuilder();
        List<Type> drawerTypes = GetAllSubTypes(typeof(PropertyDrawer));

        foreach (var drawerType in drawerTypes)
        {
            PropertyDrawerAttribute[] attributes = (PropertyDrawerAttribute[])drawerType.GetCustomAttributes(typeof(PropertyDrawerAttribute), true);
            if (attributes.Length > 0)
            {
                drawerEntriesBuilder.AppendFormat(DRAWER_ENTRY_FORMAT, attributes[0].TargetAttributeType.Name, drawerType.Name);
            }
        }

        string scriptContent = templateFormat
            .Replace(CLASS_NAME_PLACEHOLDER, scriptName)
            .Replace(ENTRIES_PLACEHOLDER, drawerEntriesBuilder.ToString());

        string scriptPath = GENERATED_CODE_TARGET_FOLDER + scriptName + ".cs";

        IOUtility.WriteToFile(scriptPath, scriptContent);
    }

    private static void GenerateValidatorDatabaseScript(string scriptName, string templateName)
    {
        string templateGUID = AssetDatabase.FindAssets(templateName)[0];
        string templateRelativePath = AssetDatabase.GUIDToAssetPath(templateGUID);
        string templateFullPath = (Application.dataPath.Replace("Assets", string.Empty) + templateRelativePath).Replace("/", "\\");
        string templateFormat = IOUtility.ReadFromFile(templateFullPath);

        StringBuilder validatorEntriesBuilder = new StringBuilder();
        List<Type> validatorTypes = GetAllSubTypes(typeof(PropertyValidator));

        foreach (var validatorType in validatorTypes)
        {
            PropertyValidatorAttribute[] attributes = (PropertyValidatorAttribute[])validatorType.GetCustomAttributes(typeof(PropertyValidatorAttribute), true);
            if (attributes.Length > 0)
            {
                validatorEntriesBuilder.AppendFormat(VALIDATOR_ENTRY_FORMAT, attributes[0].TargetAttributeType.Name, validatorType.Name);
            }
        }

        string scriptContent = templateFormat
            .Replace(CLASS_NAME_PLACEHOLDER, scriptName)
            .Replace(ENTRIES_PLACEHOLDER, validatorEntriesBuilder.ToString());

        string scriptPath = GENERATED_CODE_TARGET_FOLDER + scriptName + ".cs";

        IOUtility.WriteToFile(scriptPath, scriptContent);
    }

    private static List<Type> GetAllSubTypes(Type baseClass)
    {
        var result = new List<Type>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assemly in assemblies)
        {
            Type[] types = assemly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsSubclassOf(baseClass))
                {
                    result.Add(type);
                }
            }
        }
        return result;
    }
}