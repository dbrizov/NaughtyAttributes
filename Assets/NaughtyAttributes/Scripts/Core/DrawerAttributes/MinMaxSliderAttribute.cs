using System;
using UnityEngine;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class MinMaxSliderAttribute : PropertyAttribute, IDrawerAttribute
	{
		public float MinValue { get; private set; }
		public float MaxValue { get; private set; }

		public MinMaxSliderAttribute(float minValue, float maxValue)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}
	}
}
