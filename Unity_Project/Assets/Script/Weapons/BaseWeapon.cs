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
    public string m_shotSoundName;
    public AudioSource audioSource;

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

    public int m_maxAmmoInClip = 1;
    protected int m_currentAmmoInClip;

    public float m_reloadTime = 2f;
    protected float m_currentReloadTime;

	public Animator animator;

    public bool m_showLaser = false;

    protected virtual void Start()
    {
        m_currentAmmoInClip = m_maxAmmoInClip;

        m_currentReloadTime = m_reloadTime;
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
			BaseMecha._instance.HitEffect(hit.point);
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

    public virtual float GetHeat()
    {
        return 0f;
    }

    public virtual bool IsTargetAimable(Transform target)
    {
        Vector3 targetDir = (target.position - m_muzzle.position).normalized;

        RaycastHit hit;

        Physics.Raycast(m_muzzle.position, targetDir, out hit, m_maxRange, m_mask.value);

        if (hit.rigidbody == null) return false;

        return (hit.transform.gameObject.layer == target.gameObject.layer);
    }

    public virtual bool IsWeaponOnTarget(Vector3 targetPosition)
    {
        Vector3 targetDir = targetPosition - m_muzzle.position;
        float angle = Vector3.Angle(targetDir, m_muzzle.forward);

        return (angle < m_imprecision);
    }

    protected virtual void Update()
	{
        if (m_currentAmmoInClip == 0)
        {
            m_currentReloadTime -= Time.deltaTime;
            if (m_currentReloadTime <= 0f)
            {
                m_currentAmmoInClip = m_maxAmmoInClip;
                m_currentReloadTime = m_reloadTime;
            }
        }
	}
}
