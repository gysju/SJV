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
    }

    protected override void FireWeapon(MoveController moveController)
    {
		if ( animator != null)
			animator.SetTrigger ("Fired");
		
        MuzzleFlash();
        ShotSound();

        if (moveController)
        {
            moveController.StartVibration(m_vibrationPower, m_vibrationDuration);
        }

        RaycastHit hit;
        Vector3 shotDirection = (GetSpread() * m_muzzle.forward);
        if (Physics.Raycast(m_muzzle.position, shotDirection, out hit, m_maxRange, m_mask))
        {
            BulletHitParticle(hit);
            BaseUnit unitHit = hit.transform.GetComponent<BaseUnit>();
            if (unitHit)
            {
                unitHit.ReceiveDamages(Damage, ArmorPenetration);
                if (unitHit is AirEnemy && unitHit.IsDestroyed())
                {

                }
            }
        }
        else
        {

        }
    }

    public override void TriggerPressed(MoveController moveController)
    {
        FireWeapon(moveController);
    }

    public override void TriggerReleased()
    {

    }
}
