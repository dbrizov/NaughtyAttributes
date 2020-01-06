using UnityEngine;
using NaughtyAttributes;
using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class AnimatorTypeAttribute : DrawerAttribute
{
  //  public enum paramType { Float, Int, Bool, Trigger }

    public AnimatorControllerParameterType Type;
    public string FieldName;

    public AnimatorTypeAttribute(string animatorFieldName, AnimatorControllerParameterType type)
    {
        Type = type;
        FieldName = animatorFieldName;
    }
}
