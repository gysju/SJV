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

    protected virtual void Start()
    {
        if (!m_shotSound) m_shotSound = GetComponent<AudioSource>();
    }

    protected virtual void BulletHitParticle(RaycastHit hit)
    {
		Hit bullet = null;
		Transform HitTransform = hit.transform;

		if (HitTransform.GetComponent<GroundEnemy>() != null)
		{
			if (HitManager.Instance.TankHit == null)
				return;
			
			bullet = HitManager.Instance.TankHits.Find( x => x.Available == true);
			if(bullet != null)
			{
				bullet.reset ();
			}
			else
			{
				bullet = Instantiate(HitManager.Instance.TankHit, HitTransform.parent);
				HitManager.Instance.TankHits.Add(bullet);
			}
		}
		else if (HitTransform.GetComponent<AirEnemy>() != null)
		{
			if (HitManager.Instance.DroneHit == null)
				return;

			bullet = HitManager.Instance.DroneHits.Find( x => x.Available == true);
			if(bullet != null)
			{
				bullet.reset ();
			}
			else
			{
				bullet = Instantiate(HitManager.Instance.DroneHit, HitTransform.parent);
				HitManager.Instance.DroneHits.Add(bullet);
			}
		}
		else if (HitTransform.GetComponent<BaseMecha>() != null)
		{
			if (HitManager.Instance.PlayerHit == null)
				return;
			
			bullet = HitManager.Instance.PlayerHits.Find( x => x.Available == true);
			if(bullet != null)
			{
				bullet.reset ();
			}
			else
			{
				bullet = Instantiate(HitManager.Instance.PlayerHit, HitTransform.parent);
				HitManager.Instance.PlayerHits.Add(bullet);
			}
			BaseMecha.Instance.HitEffect(hit.point);
		}
		else 
		{
			if (HitManager.Instance.GroundHit == null)
				return;
			
			bullet = HitManager.Instance.GroundHits.Find( x => x.Available == true);
			if(bullet != null)
			{
				bullet.reset ();
			}
			else
			{
				bullet = Instantiate(HitManager.Instance.GroundHit, HitTransform.parent);
				HitManager.Instance.GroundHits.Add(bullet);
			}
		}

		Transform bulletTransform = bullet.transform;

		bulletTransform.position = hit.point;
		bulletTransform.LookAt(transform);
		if (bullet.Decal != null)
			bullet.Decal.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
		bulletTransform.parent = HitTransform;
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
        
	}
}
