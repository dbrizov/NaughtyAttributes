using UnityEngine;

namespace NaughtyAttributes.Test
{
    [ExecuteAlways]
    public class UsePropertySetter : MonoBehaviour
    {
        public int MyPositiveProperty
        {
            get => _myPositiveProperty;
            set
            {
                if (value >= 0)
                {
                    _myPositiveProperty = value;
                    OnNewValue?.Invoke(value);
                }
                else Debug.Log("Negative! Not setting that.");
            }
        }
        [SerializeField, UsePropertySetter, Tooltip("This is a working tooltip."), Delayed] private int _myPositiveProperty;

        public event System.Action<int> OnNewValue;

        private void OnEnable()
        {
            OnNewValue -= PrintValue;
            OnNewValue += PrintValue;
        }

        private void PrintValue(int a)
        {
            print($"New value: {a}!");
        }
    }
}