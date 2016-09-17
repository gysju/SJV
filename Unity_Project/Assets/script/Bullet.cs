using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

	[Range(1.0f, 10.0f)]
	public float Velocity = 1.0f;
	public bool UseGravity = false;

	private Rigidbody rigid;

	void Start () 
	{
		rigid = GetComponent<Rigidbody> ();

		rigid.useGravity = UseGravity;
		rigid.AddForce(transform.forward * Velocity, ForceMode.Impulse);
	}
	
	void Update () 
	{
	
	}

    void lifeTime()
    {

    }

	void OnTriggerEnter(Collider col)
	{
        if (col.gameObject != Player.Instance.gameObject)
		    Destroy (gameObject);
	}
}
