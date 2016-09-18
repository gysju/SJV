using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {


    public float Life = 1.0f;

    void Start ()
    {
	
	}

    void Update ()
    {
	
	}

    public void ReceiveDamages( float amount)
    {
        Life -= amount;
        if (Life <= 0.0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
