using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyHideWithFiled : MonoBehaviour
{
    public bool Switch;

    [InfoBox("This InfoBox will always be shown", hideWithField: false)]
    [Required("This Required error will always be shown", hideWithField: false)]
    [ValidateInput("This ValidateInput error will always be shown", hideWithField: false)]
    public GameObject NormalObject;

    [HideIf("Switch")]
    [InfoBox("This InfoBox will be hiddn when 'Switch' is false", hideWithField: true)]
    [Required("This Required error will be hiddn when 'Switch' is false", hideWithField: true)]
    [ValidateInput("This ValidateInput error will be hiddn when 'Switch' is false", hideWithField: true)]
    public GameObject HidewithObject;

    public bool ValidateMethod(GameObject go)
    {
        return false;
    }
}