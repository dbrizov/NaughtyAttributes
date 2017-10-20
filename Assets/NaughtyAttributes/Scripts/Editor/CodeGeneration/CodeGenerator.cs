using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void GenerateCode()
        {
            GenerateScript<PropertyMeta, PropertyMetaAttribute>("MetaDatabase", "MetaDatabaseTemplate", META_ENTRY_FORMAT);
            GenerateScript<PropertyDrawer, PropertyDrawerAttribute>("DrawerDatabase", "DrawerDatabaseTemplate", DRAWER_ENTRY_FORMAT);
            GenerateScript<PropertyGrouper, PropertyGrouperAttribute>("GrouperDatabase", "GrouperDatabaseTemplate", GROUPER_ENTRY_FORMAT);
            GenerateScript<PropertyValidator, PropertyValidatorAttribute>("ValidatorDatabase", "ValidatorDatabaseTemplate", VALIDATOR_ENTRY_FORMAT);
            GenerateScript<PropertyDrawCondition, PropertyDrawConditionAttribute>("DrawConditionDatabase", "DrawConditionDatabaseTemplate", DRAW_CONDITION_ENTRY_FORMAT);

            AssetDatabase.Refresh();
        }

        private static void GenerateScript<TAttributeGroup, TPropertyAttribute>(string scriptName, string templateName, string entryFormat)
            where TPropertyAttribute : IPropertyAttribute
        {
            string[] templateAssets = AssetDatabase.FindAssets(templateName);
            if (templateAssets.Length == 0)
            {
                return;
            }

            string templateGUID = templateAssets[0];
            string templateRelativePath = AssetDatabase.GUIDToAssetPath(templateGUID);
            string templateFullPath = (Application.dataPath.Replace("Assets", string.Empty) + templateRelativePath).Replace("/", "\\");
            string templateFormat = IOUtility.ReadFromFile(templateFullPath);

            StringBuilder entriesBuilder = new StringBuilder();
            List<Type> subTypes = GetAllSubTypes(typeof(TAttributeGroup));

            foreach (var subType in subTypes)
            {
                IPropertyAttribute[] attributes =
                    (IPropertyAttribute[])subType.GetCustomAttributes(typeof(TPropertyAttribute), true);

                if (attributes.Length > 0)
                {
                    entriesBuilder.AppendFormat(entryFormat, attributes[0].TargetAttributeType.Name, subType.Name);
                }
            }

            string scriptContent = templateFormat
                .Replace(CLASS_NAME_PLACEHOLDER, scriptName)
                .Replace(ENTRIES_PLACEHOLDER, entriesBuilder.ToString());

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
