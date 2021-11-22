using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour {

	public static T Instance { get; protected set; }

	protected virtual void Awake() {
		if(Instance == null)
			Instance = this as T;
		else {
			Debug.Log("Doubled singleton: " + typeof(T).ToString());
			Destroy(this);
		}
	}

}
