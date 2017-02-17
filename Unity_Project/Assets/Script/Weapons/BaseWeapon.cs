using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseWeapon : MonoBehaviour
{
    [Tooltip("What can shoot the weapon.")]
    public LayerMask m_mask;

    [Tooltip("From where the ammo is fired.")]
    public Transform m_muzzle;

    [Tooltip("Graphical effect when firing.")]
    public ParticleSystem m_muzzleFlash;
    public Hit m_bulletHit;
    protected List<Hit> m_bulletHits = new List<Hit>();
    
    public ParticleSystem m_shellParticles;

    [Tooltip("Sound effect when firing.")]
    public AudioSource m_shotSound;

    public float m_vibrationDuration;
    public int m_vibrationPower;

    [Tooltip("Max range of the weapon.")]
    [Range(1f, 100f)]
    public float m_maxRange = 25f;

    [Tooltip("Random angle the weapon have when shooting.")]
    [Range(0f, 10f)]
    public float m_imprecision = 0f;

    public int Damage = 1;
    public int ArmorPenetration = 1;

	public Animator animator;

    public bool m_showLaser = false;
	protected LineRenderer m_laser;    

    protected virtual void Start()
    {
        m_laser = GetComponent<LineRenderer>();
        if (!m_shotSound) m_shotSound = GetComponent<AudioSource>();
    }

    protected virtual void BulletHitParticle(RaycastHit hit)
    {
        if (m_bulletHit)
        {
            bool bulletHitAvailable = false;
            foreach (Hit bulletHit in m_bulletHits)
            {
                if (bulletHit.Available)
                {
                    bulletHitAvailable = true;
                    bulletHit.Available = false;
                    bulletHit.transform.position = hit.point;
                    bulletHit.transform.LookAt(transform);
                    bulletHit.transform.rotation = Quaternion.FromToRotation(bulletHit.transform.up, hit.normal) * bulletHit.transform.rotation;
                    bulletHit.particle.Play(true);
                    break;
                }
            }

            if (!bulletHitAvailable)
            {
                Hit newBulletHit = Instantiate(m_bulletHit, hit.transform.parent);
                m_bulletHits.Add(newBulletHit.GetComponent<Hit>());

                newBulletHit.transform.position = hit.point;
                newBulletHit.transform.LookAt(transform);
                newBulletHit.transform.rotation = Quaternion.FromToRotation(newBulletHit.transform.up, hit.normal) * newBulletHit.transform.rotation;
                newBulletHit.particle.Play(true);
            }
        }
    }

    protected virtual void FireWeapon(MoveController moveController = null)
    {

    }
    
    public virtual void TriggerPressed(MoveController moveController = null)
    {

    }

    public virtual void TriggerReleased()
    {

    }

    void Update ()
	{
        if (m_laser)
        {
            if(m_showLaser)
            {
                m_laser.SetPosition(0, m_muzzle.position);
                m_laser.SetPosition(1, m_muzzle.position + m_muzzle.forward * m_maxRange);
            }
            else
            {
                m_laser.SetPosition(0, m_muzzle.position);
                m_laser.SetPosition(1, m_muzzle.position);
            }
        }
	}
}
