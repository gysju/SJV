using UnityEngine;
using System.Collections;

public class script : MonoBehaviour {

	public float timeTransition = 1.0f;
	public float time = 0.0f;

	void Start () 
	{
		StartCoroutine (Rotate(Vector3.right, 30.0f) );
	}
	
	void Update () 
	{
		
	}

	IEnumerator Rotate( Vector3 Axis, float Angle)
	{
		float start = Time.time;

		while(Time.time - start < timeTransition)
		{
			transform.Rotate (Axis, (Angle/timeTransition * Time.deltaTime));
			yield return null;
		}
	}
}
