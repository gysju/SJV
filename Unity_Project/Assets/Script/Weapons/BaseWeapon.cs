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

    protected virtual void FireWeapon()
    {

    }
    
    public virtual void TriggerPressed()
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
