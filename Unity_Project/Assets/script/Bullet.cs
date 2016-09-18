using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

	[Range(1.0f, 10.0f)]
	public float Velocity = 1.0f;
	public bool UseGravity = false;
    public Entity Origin;

    public float Damage = 1.0f;

	private Rigidbody rigid;
    private float time = 0.0f;
    const float TIME_TO_DIE = 1f;

    void Start () 
	{
		rigid = GetComponent<Rigidbody> ();

		rigid.useGravity = UseGravity;
		rigid.AddForce(transform.forward * Velocity, ForceMode.Impulse);
	}
	
	void Update () 
	{
        lifeTime();
    }
    
    void lifeTime()
    {
        time += Time.deltaTime;
        if (time > TIME_TO_DIE)
            Destroy(gameObject);
    }

	void OnTriggerEnter(Collider col)
	{
        if (col.gameObject != Origin.gameObject)
        {
            Entity entity = col.GetComponent<Entity>();
            if (entity != null)
                entity.ReceiveDamages(Damage);

            Destroy(gameObject);
        }
	}
}
