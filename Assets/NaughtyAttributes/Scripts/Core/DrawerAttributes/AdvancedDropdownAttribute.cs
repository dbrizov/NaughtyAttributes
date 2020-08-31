using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class AdvancedDropdownAttribute : DrawerAttribute
	{
		public string ValuesName { get; private set; }
		public bool KeepStateWhenReopened { get; set; }
		public bool MaximizeWidth { get; set; }
		public int MinHeight { get; set; } = -1;
		public int MaxHeight { get; set; } = -1;
		public bool FlattenTree { get; set; }
		public bool DrawOnSameLine { get; set; }
		public string ListTitle { get; set; } = "Options";

		//public SortByMode SortBy { get; set; } = SortByMode.Name | SortByMode.BranchesFirst;

		//[Flags]
		//public enum SortByMode { None, Name = 1 , BranchesFirst = 2, LeafsFirst = 4};

		public AdvancedDropdownAttribute(string valuesName)
		{
			ValuesName = valuesName;
		}
	}
}
