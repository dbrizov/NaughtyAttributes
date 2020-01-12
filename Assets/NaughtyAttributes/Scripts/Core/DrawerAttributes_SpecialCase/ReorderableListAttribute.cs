using System;
using UnityEngine;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ReorderableListAttribute : PropertyAttribute, ISpecialCaseDrawerAttribute
	{
	}
}
