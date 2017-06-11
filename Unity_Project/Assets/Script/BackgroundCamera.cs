using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCamera : MonoBehaviour {

	Transform mainCameraTransform = null;
	Transform thisTransform = null;
	void Start () {
		mainCameraTransform = Camera.main.transform;
		thisTransform = transform;
	}
	
	void LateUpdate () {
        thisTransform.rotation = mainCameraTransform.rotation;
	}
}
