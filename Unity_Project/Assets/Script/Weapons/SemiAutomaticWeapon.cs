using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("MechaVR/Weapon/SemiAutomaticWeapon")]
public class SemiAutomaticWeapon : BaseWeapon
{

    protected Quaternion GetSpread()
    {
        return Quaternion.Euler(Random.Range(-m_imprecision, m_imprecision), Random.Range(-m_imprecision, m_imprecision), Random.Range(-m_imprecision, m_imprecision));
    }

    protected void MuzzleFlash()
    {
        if(m_muzzleFlash) m_muzzleFlash.Play();
    }

    protected void ShotSound()
    {
        // play shoot sound
        SoundManager.Instance.PlaySoundOnShot(m_shotSoundName, audioSource);
    }

    protected override void FireWeapon(MoveController moveController = null)
    {
        if (m_currentAmmoInClip != 0)
        {
            if (m_currentAmmoInClip > 0)
                m_currentAmmoInClip--;

		    if ( animator != null)
		    	animator.SetTrigger ("Fired");
		    
            MuzzleFlash();
            ShotSound();

            if (m_shellParticles)
                m_shellParticles.Emit(1);

#if UNITY_PS4
            if (moveController)
            {
                moveController.StartVibration(m_vibrationPower, m_vibrationDuration);
            }
#endif
            RaycastHit hit;
            Vector3 shotDirection = (GetSpread() * m_muzzle.forward);
            Vector3 hitPosition = m_muzzle.position + (shotDirection * m_maxRange);
            if (Physics.Raycast(m_muzzle.position, shotDirection, out hit, m_maxRange, m_mask))
            {
                BaseUnit unitHit = hit.transform.GetComponent<BaseUnit>();
                if (unitHit)
                {
                    if( unitHit.ReceiveDamages(Damage, ArmorPenetration))
                        BulletHitParticle(hit);
                }
                else
                {
                    BulletHitParticle(hit);
                }
                hitPosition = hit.point;
            }

            if (m_tracerBullet)
                m_tracerBullet.Use(m_muzzle.position, hitPosition);
        }
    }

    public override void TriggerPressed(MoveController moveController = null)
    {
        FireWeapon(moveController);
    }

    public override void TriggerReleased()
    {

    }
}
