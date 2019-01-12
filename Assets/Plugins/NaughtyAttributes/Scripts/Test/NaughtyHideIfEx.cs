using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyHideIfEx : MonoBehaviour
{
    public bool Bool1 = true;
    public bool Bool2 = false;

    [HideIfAny("Bool1")]
    [HideIfAny("Bool2")]
    public int HideIfAny;


    [HideIfAll("Bool1")]
    [HideIfAll("Bool2")]
    public int HideIfAll;


    [HideIfAny("Bool1", false)]
    [HideIfAny("Bool2", false)]
    public int HideIfAnyFalse;

    [HideIfAll("Bool1", false)]
    [HideIfAll("Bool2", false)]
    public int HideIfAllFalse;
}
