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
    public GameObject m_bulletHit;
    protected List<ParticleSystem> m_bulletHits = new List<ParticleSystem>();

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
    protected Transform bulletHitParent;
    

    protected virtual void Start()
    {
        m_laser = GetComponent<LineRenderer>();

        bulletHitParent = new GameObject("Hits").transform;
        bulletHitParent.parent = transform;
        if (!m_shotSound) m_shotSound = GetComponent<AudioSource>();
    }

    protected virtual void BulletHitParticle(RaycastHit hit)
    {
        if (m_bulletHit)
        {
            bool bulletHitAvailable = false;
            foreach (ParticleSystem ps in m_bulletHits)
            {
                if (!ps.IsAlive(true))
                {
                    bulletHitAvailable = true;
                    ps.transform.position = hit.point;
                    ps.transform.LookAt(transform);
                    ps.Play(true);
                    break;
                }
            }

            if (!bulletHitAvailable)
            {
                GameObject newBulletHit = Instantiate(m_bulletHit, bulletHitParent);
                m_bulletHits.Add(newBulletHit.GetComponent<ParticleSystem>());
            }
        }
    }

    protected virtual void FireWeapon(MoveController moveController)
    {

    }
    
    public virtual void TriggerPressed(MoveController moveController)
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
