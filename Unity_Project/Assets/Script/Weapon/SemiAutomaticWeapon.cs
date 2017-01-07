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

    protected void BulletHitParticle(RaycastHit hit)
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

    protected override void FireWeapon()
    {
        MuzzleFlash();
        ShotSound();
        RaycastHit hit;
        Vector3 shotDirection = (GetSpread() * m_muzzle.forward);
        if (Physics.Raycast(m_muzzle.position, shotDirection, out hit, m_maxRange, m_mask))
        {
            BulletHitParticle(hit);
            BaseUnit unitHit = hit.transform.GetComponentInParent<BaseUnit>();
            if (unitHit) unitHit.ReceiveDamages(Damage, ArmorPenetration);
        }
        else
        {

        }
    }

    public override void TriggerPressed()
    {
        FireWeapon();
    }

    public override void TriggerReleased()
    {

    }

    void Update ()
	{
        if (m_laser && CanvasManager.EState_Menu.EState_Menu_InGame == CanvasManager.Get.eState_Menu)
        {
            m_laser.SetPosition(0, m_muzzle.position);
            m_laser.SetPosition(1, m_muzzle.position + m_muzzle.forward * 20);
        }
    }
}
