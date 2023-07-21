using System;
using System.Collections.Generic;
using UnityEngine;
namespace NaughtyAttributes
{
    public class TypeTest : MonoBehaviour
    {
        [Type(typeof(A), typeof(B), typeof(A.B), typeof(C), typeof(global::C))]
        public List<string> WaitForGenerate;

        [Button]
        void Generate()
        {
            foreach (var typeName in WaitForGenerate)
            {
                Type type = Type.GetType(typeName);
                object obj = Activator.CreateInstance(type);
                Debug.Log(obj);
            }
        }

        [Type(typeof(MonoBehaviour), typeof(ScriptableObject))]
        public int type;

        public class A
        {
            public int a;
            public class B
            {
            }
        }

        public class B
        {
            public int b;
        }
        public class C
        {
            public int c;
        }

        
    }

}

public class C
{
    public int c;
}