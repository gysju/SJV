using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFPS : MonoBehaviour {

	private Text text;
	private float deltaTime = 0;

	void Start () {
		text = GetComponent<Text> ();	
	}
	
	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;

		text.text = "fps : " + string.Format("{0:0.00}",fps); 
	}
}
