using UnityEngine;
using System.Collections;

[AddComponentMenu("MecaVR/Ammo/Ballistic")]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Ballistic : Ammo
{
    [Header("Ballistic Ammo")]
    protected Rigidbody m_rigidBody;

    const float MIN_IMPULSE = 0f;
    const float MAX_IMPULSE = 500f;

    [Tooltip("The force applied to the ammo when fired.")]
    [Range(MIN_IMPULSE, MAX_IMPULSE)]
    public float m_impulseForce = 1f;

    // Use this for initialization
    protected override void Awake ()
    {
        base.Awake();
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.AddForce(transform.forward * m_impulseForce, ForceMode.Impulse);
    }

    protected override void DestroyAmmo()
    {
        m_rigidBody.isKinematic = true;
        m_rigidBody.velocity = Vector3.zero;
    }

    protected virtual void Ricochet()
    {
        Debug.Log("RICOCHET");
        //if (m_explosive) Explode();
        //else
        {

        }
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
        Unit unit = collision.gameObject.GetComponent<Unit>();
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
