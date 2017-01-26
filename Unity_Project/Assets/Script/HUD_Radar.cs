using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Radar : MonoBehaviour {

	private Transform Mecha;
	private Transform ThisTransform;

	void Start () 
	{
		Mecha = GetComponentInParent<BaseMecha> ().transform;	
		ThisTransform = transform;
	}
	
	void Update () {
		if (Mecha != null)
			ThisTransform.localRotation = Quaternion.Euler ( new Vector3 (0.0f, 0.0f, Mecha.localRotation.eulerAngles.y ));
	}
}
 