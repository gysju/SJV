using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public bool Available = false;
    public float lifeTime = 5.0f;
    public ParticleSystem particle;
	public GameObject Decal;
    private float currentTime = 0.0f;

	void Awake ()
    {
		particle = GetComponentInChildren<ParticleSystem>();
		Decal = transform.FindChild ("Decal").gameObject;
    }

    void Update ()
    {
        currentTime += Time.deltaTime;
        if (currentTime > lifeTime)
        {
            Available = true;
        }
	}

	public void reset()
	{
		currentTime = 0;
		Available = false;
		if ( particle != null )
			particle.Play ();
	}
}
