using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

public class CodeGenerator : Editor
{
    private const string GENERATED_CODE_TARGET_RELATIVE_PATH = "Assets/DIA/Scripts/Editor/Utility/";

    private static readonly string ENTRIES_PLACEHOLDER = "__entries__";
    private static readonly string VALIDATOR_ENTRY_FORMAT = "validatorsByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine;
    
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void GenerateCode()
    {
        GenerateValidatorUtilityScript();

        AssetDatabase.Refresh();
    }

    private static void GenerateValidatorUtilityScript()
    {
        string templateGUID = AssetDatabase.FindAssets("ValidatorUtilityTemplate")[0];
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

        string scriptContent = templateFormat.Replace(ENTRIES_PLACEHOLDER, validatorEntriesBuilder.ToString());
        string scriptPath = (Application.dataPath.Replace("Assets", string.Empty) + GENERATED_CODE_TARGET_RELATIVE_PATH).Replace("/", "\\") + "ValidatorUtility.cs";
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