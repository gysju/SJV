using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour {

	public GameObject Bullet;
	private GameObject output;

	void Start () 
	{
		output = transform.FindChild ("output").gameObject;	
	}
	
	void Update () 
	{
		
	}

	public void Shoot( Entity origin )
	{
		GameObject bullet = Instantiate (Bullet, output.transform.position, output.transform.rotation) as GameObject;
        bullet.GetComponent<Bullet>().Origin = origin;
    }
}
