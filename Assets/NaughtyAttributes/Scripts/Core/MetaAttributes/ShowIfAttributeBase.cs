using System;
using System.Linq;

namespace NaughtyAttributes
{
    public class ShowIfAttributeBase : MetaAttribute
    {
        public string[] Conditions { get; private set; }
        public EConditionOperator ConditionOperator { get; private set; }
        public bool Inverted { get; protected set; }

        /// <summary>
        ///		If this not null, <see cref="Conditions"/>[0] is name of an enum variable.
        /// </summary>
        public Enum[] EnumValues { get; private set; }

        public ShowIfAttributeBase(string condition)
        {
            ConditionOperator = EConditionOperator.And;
            Conditions = new string[1] { condition };
        }

        public ShowIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }

        public ShowIfAttributeBase(string enumName, Enum enumValue)
            : this(enumName)
        {
            if (enumValue == null)
            {
                throw new ArgumentNullException(nameof(enumValue), "This parameter must be an enum value.");
            }

            EnumValues = new []{ enumValue };
        }

        public ShowIfAttributeBase(string enumName, params object[] enumValues)
            : this(enumName)
        {
            if (enumValues == null || enumValues.Any(value => value == null))
            {
                throw new ArgumentNullException(nameof(enumValues), "All parameters must be enum values.");
            }

            EnumValues = enumValues.Select(obj => obj as Enum).ToArray();
        }
    }
}
