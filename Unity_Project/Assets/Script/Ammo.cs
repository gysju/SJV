using UnityEngine;
using System.Collections;

[AddComponentMenu("Test")]
[RequireComponent(typeof(Rigidbody))]
public class Ammo : MonoBehaviour
{
    private Rigidbody m_rigidBody;

    [Header("Damages")]
    public int m_directDamages;
    public int m_armorPenetration;
    public bool m_explosive;
    public GameObject m_explosionPrefab;

    const float MIN_IMPULSE = 0f;
    const float MAX_IMPULSE = 500f;
    [Range(MIN_IMPULSE, MAX_IMPULSE)]
    public float m_impulseForce = 1f;

    const float MIN_LIFE_TIME = 0f;
    const float MAX_LIFE_TIME = 10f;
    [Range(MIN_LIFE_TIME, MAX_LIFE_TIME)]
    public float m_lifeTime = 10f;

	void Start()
	{
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.AddForce(Vector3.forward * m_impulseForce, ForceMode.Impulse);
        StartCoroutine(LifeTimer(m_lifeTime));
	}

    IEnumerator LifeTimer(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Lost();
    }
	
    protected virtual void Lost()
    {
        Debug.Log("LOST AMMO");
        Destroy(gameObject);
    }

    protected virtual void Ricochet()
    {
        Debug.Log("RICOCHET");
        //if (m_explosive) Explode();
        //else
        {

        }
    }

    protected virtual void Hit()
    {
        Debug.Log("HIT");
        if (m_explosive) Explode();
        else Destroy(gameObject);
    }

    private void Explode()
    {
        Debug.Log("EXPLOSION");
        Instantiate(m_explosionPrefab);
        Destroy(gameObject);
    }

	protected virtual void Update()
    {
        if (Mathf.Approximately(m_rigidBody.velocity.sqrMagnitude, 0f)) Lost();
    }

    void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit)
        {
            if (unit.ReceiveDamages(m_directDamages, m_armorPenetration))
            {
                Hit();
            }
            else
            {
                Vector3 normal = collision.contacts[0].normal;
                Vector3 vel = m_rigidBody.velocity;
                // measure angle
                if (Vector3.Angle(vel, -normal) < 0f)
                {
                    //m_rigidBody.velocity = Vector3.Reflect(vel, normal);
                    Ricochet();
                }
                else
                {
                    Hit();
                }
            }
        }
        else
        {
            Vector3 normal = collision.contacts[0].normal;
            Vector3 vel = m_rigidBody.velocity;
            // measure angle
            if (Vector3.Angle(vel, -normal) < 0f)
            {
                //m_rigidBody.velocity = Vector3.Reflect(vel, normal);
                Ricochet();
            }
            else
            {
                Hit();
            }
        }
    }
}
