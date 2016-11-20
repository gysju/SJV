using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Ammo/DEV/Ammo")]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BallisticAmmo : Ammo
{
    private Rigidbody m_rigidBody;
    
    [Header("Life Time")]
    const float MIN_LIFE_TIME = 0f;
    const float MAX_LIFE_TIME = 10f;

    [Tooltip("The ammo is lost at after that time (seconds).")]
    [Range(MIN_LIFE_TIME, MAX_LIFE_TIME)]
    public float m_lifeTime = 10f;

    const float MIN_IMPULSE = 0f;
    const float MAX_IMPULSE = 500f;

    [Tooltip("The force applied to the ammo when fired.")]
    [Range(MIN_IMPULSE, MAX_IMPULSE)]
    public float m_impulseForce = 1f;

	void Awake()
	{
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.AddForce(transform.forward * m_impulseForce, ForceMode.Impulse);
        StartCoroutine(LifeTimer(m_lifeTime));
    }

    IEnumerator LifeTimer(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Lost();
    }
	
    protected virtual void DestroyAmmo()
    {
        m_rigidBody.isKinematic = true;
        m_rigidBody.velocity = Vector3.zero;
    }

    protected virtual void MetalHit()
    {
        Instantiate(m_metalHit, transform.position, transform.rotation);
    }

    protected virtual void DirtHit()
    {
        Instantiate(m_dirtHit, transform.position, transform.rotation);
    }

    protected void HitAnimation(PhysicMaterial physicMaterial)
    {
        if (physicMaterial.name == "Metal (Instance)")
            MetalHit();
        else
            DirtHit();
    }

    protected virtual void Lost()
    {
        Destroy(gameObject);
    }

    protected virtual void Ricochet()
    {
        //Debug.Log("RICOCHET");
        //if (m_explosive) Explode();
        //else
        {

        }
    }

    protected virtual void CheckExplosion()
    {
        if (m_explosive) Explode();
        else DestroyAmmo();
    }

    private void Explode()
    {
        Instantiate(m_explosionPrefab, transform.position, transform.rotation);
        DestroyAmmo();
    }

    protected void CheckRicochet(Vector3 collisionNormal)
    {
        float collisionAngle = Vector3.Dot(Vector3.Reflect(transform.forward, collisionNormal), transform.forward);
        if (collisionAngle > 0.5f)
        {
            Ricochet();
        }
        else
        {
            CheckExplosion();
        }
    }

	protected virtual void Update()
    {
        if (Mathf.Approximately(m_rigidBody.velocity.sqrMagnitude, 0f)) Lost();
    }

    void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponentInParent<Unit>();
        PhysicMaterial physicMaterial = collision.collider.material;

        bool couldRicochet = (physicMaterial.name == "Metal (Instance)");

        Vector3 collisionNormal = collision.contacts[0].normal;

        if (unit)
        {
            if (unit.ReceiveDamages(m_directDamages, m_armorPenetration))
            {
                CheckExplosion();
            }
            else if (couldRicochet)
            {
                CheckRicochet(collisionNormal);
            }
        }

        if (couldRicochet)
        {
            CheckRicochet(collisionNormal);
        }
        else
        {
            CheckExplosion();
        }

        HitAnimation(physicMaterial);
    }
}
