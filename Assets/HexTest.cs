using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTest : MonoBehaviour {

	[HexBrushAttribute]
	public HexVector[] TestArray;

	[HexBrushAttribute]
	public HexVector[] SecondArray;
}
