using UnityEngine;
using NaughtyAttributes;

public class EnableDisableIf : MonoBehaviour
{
    public bool enable1;
    public bool enable2;
    public bool disable1;
    public bool disable2;

    [EnableIf(ConditionOperator.And, "enable1", "enable2")]
    public int enableIfAll = 1;

    [EnableIf(ConditionOperator.Or, "enable1", "enable2")]
    public int enableIfAny = 2;

    [DisableIf(ConditionOperator.And, "disable1", "disable2")]
    public int disableIfAll = 1;

    [DisableIf(ConditionOperator.Or, "disable1", "disable2")]
    public int disableIfAny = 2;
}
