using UnityEngine;
using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class AllowNestingAttribute : PropertyAttribute, IDrawerAttribute
	{
	}
}
