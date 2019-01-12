using NaughtyAttributes;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    public bool bool1;
    public bool bool2;

    [EnableIf(ConditionOperator.And, "bool1", "bool2")]
    public int enableIfAll = 1;

    [EnableIf(ConditionOperator.Or, "bool1", "bool2")]
    public int enableIfAny = 2;
}
