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
        if (m_shotSound) m_shotSound.Play();
        // play shoot sound
        //SoundManager.Instance.PlaySoundOnShot("", audioSource);
    }

    protected override void FireWeapon(MoveController moveController = null)
    {
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
