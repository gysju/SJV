using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeTrough : MonoBehaviour {

	Material mat = null;
	Transform Cam = null;
	Vector4 dir; 
	void Start () 
	{
		mat = GetComponent<MeshRenderer> ().material;
		Cam = Camera.main.transform;	
	}
	
	void LateUpdate () 
	{
		dir = new Vector4(Cam.forward.x, Cam.forward.y, Cam.forward.z, 1.0f);
		mat.SetVector ("_CamDir", dir);
	}
}
