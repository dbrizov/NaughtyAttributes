using UnityEngine;
using NaughtyAttributes;

public class ShowHideIf : MonoBehaviour
{
    public bool show1;
    public bool show2;
    public bool hide1;
    public bool hide2;

    [ShowIf("False")]
    public int showIf = 0;

    [ShowIf(ConditionOperator.And, "show1", "show2")]
    public int showIfAll = 1;

    [ShowIf(ConditionOperator.Or, "show1", "show2")]
    public int showIfAny = 2;

    [HideIf("True")]
    public int hideIf = 0;

    [HideIf(ConditionOperator.And, "hide1", "hide2")]
    public int hideIfAll = 1;

    [HideIf(ConditionOperator.Or, "hide1", "hide2")]
    public int hideIfAny = 2;

    private bool True()
    {
        return true;
    }

    private bool False()
    {
        return false;
    }
}
