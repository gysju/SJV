using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFPS : MonoBehaviour {

	private Text text;
	private int value = 0;
	private float time = 0;

	void Start () {
		text = GetComponent<Text> ();	
	}
	
	void Update () {

		if (time >= 1.0f) 
		{
			text.text = "fps : " + value;
			time = 0.0f;
			value = 0;
		} 
		else 
		{
			time += Time.deltaTime;
			value += 1;
		}

	}
}
