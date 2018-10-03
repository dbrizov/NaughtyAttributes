using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public class CodeGenerator : UnityEditor.Editor
    {
        private static readonly string GENERATED_CODE_TARGET_FOLDER =
            (Application.dataPath.Replace("Assets", string.Empty) + AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("CodeGenerator")[0]))
            .Replace("CodeGenerator.cs", string.Empty)
            .Replace("/", "\\");

        private static readonly string CLASS_NAME_PLACEHOLDER = "__classname__";
        private static readonly string ENTRIES_PLACEHOLDER = "__entries__";
        private static readonly string META_ENTRY_FORMAT = "metasByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine;
        private static readonly string DRAWER_ENTRY_FORMAT = "drawersByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine;
        private static readonly string GROUPER_ENTRY_FORMAT = "groupersByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine;
        private static readonly string VALIDATOR_ENTRY_FORMAT = "validatorsByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine;
        private static readonly string DRAW_CONDITION_ENTRY_FORMAT = "drawConditionsByAttributeType[typeof({0})] = new {1}();" + Environment.NewLine;

        //[UnityEditor.Callbacks.DidReloadScripts]
        [MenuItem("Tools/NaughtyAttributes/Update Attributes Database")]
        private static void GenerateCode()
        {
            GenerateScript<PropertyMeta, PropertyMetaAttribute>("PropertyMetaDatabase", "PropertyMetaDatabaseTemplate", META_ENTRY_FORMAT);
            GenerateScript<PropertyDrawer, PropertyDrawerAttribute>("PropertyDrawerDatabase", "PropertyDrawerDatabaseTemplate", DRAWER_ENTRY_FORMAT);
            GenerateScript<PropertyGrouper, PropertyGrouperAttribute>("PropertyGrouperDatabase", "PropertyGrouperDatabaseTemplate", GROUPER_ENTRY_FORMAT);
            GenerateScript<PropertyValidator, PropertyValidatorAttribute>("PropertyValidatorDatabase", "PropertyValidatorDatabaseTemplate", VALIDATOR_ENTRY_FORMAT);
            GenerateScript<PropertyDrawCondition, PropertyDrawConditionAttribute>("PropertyDrawConditionDatabase", "PropertyDrawConditionDatabaseTemplate", DRAW_CONDITION_ENTRY_FORMAT);

            GenerateScript<FieldDrawer, FieldDrawerAttribute>("FieldDrawerDatabase", "FieldDrawerDatabaseTemplate", DRAWER_ENTRY_FORMAT);
            GenerateScript<MethodDrawer, MethodDrawerAttribute>("MethodDrawerDatabase", "MethodDrawerDatabaseTemplate", DRAWER_ENTRY_FORMAT);
            GenerateScript<NativePropertyDrawer, NativePropertyDrawerAttribute>("NativePropertyDrawerDatabase", "NativePropertyDrawerDbTemplate", DRAWER_ENTRY_FORMAT);

            AssetDatabase.Refresh();
        }

        private static void GenerateScript<TClass, TAttribute>(string scriptName, string templateName, string entryFormat)
            where TAttribute : IAttribute
        {
            string[] templateAssets = AssetDatabase.FindAssets(templateName);
            if (templateAssets.Length == 0)
            {
                return;
            }

            string templateGUID = templateAssets[0];
            string templateRelativePath = AssetDatabase.GUIDToAssetPath(templateGUID);
            string templateFormat = (AssetDatabase.LoadAssetAtPath(templateRelativePath, typeof(TextAsset)) as TextAsset).ToString();
            //string templateFullPath = (Application.dataPath.Replace("Assets", string.Empty) + templateRelativePath).Replace("/", "\\");
            //string templateFormat = IOUtility.ReadFromFile(templateFullPath);

            StringBuilder entriesBuilder = new StringBuilder();
            List<Type> subTypes = GetAllSubTypes(typeof(TClass));

            foreach (var subType in subTypes)
            {
                IAttribute[] attributes = (IAttribute[])subType.GetCustomAttributes(typeof(TAttribute), true);
                if (attributes.Length > 0)
                {
                    entriesBuilder.AppendFormat(entryFormat, attributes[0].TargetAttributeType.Name, subType.Name);
                }
            }

            string scriptContent = templateFormat
                .Replace(CLASS_NAME_PLACEHOLDER, scriptName)
                .Replace(ENTRIES_PLACEHOLDER, entriesBuilder.ToString());

            scriptContent = Regex.Replace(scriptContent, @"\r\n|\n\r|\r|\n", Environment.NewLine); // Normalize line endings

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
}
