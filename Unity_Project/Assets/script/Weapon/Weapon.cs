using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Weapon/DEV/Weapon")]
public class Weapon : MonoBehaviour
{
    [Tooltip("From where the ammo is fired.")]
    public Transform m_muzzle;
    public GameObject m_muzzleFlash;

    [Header("Weapon Specs")]
    [Tooltip("Ammo type the weapon fire.")]
    public GameObject m_ammo;

    public enum FiringMethod
    {
        SemiAutomatic,
        Burst,
        Automatic
    }
    [Tooltip("Firing Method.")]
    public FiringMethod m_firingMethod = FiringMethod.Automatic;
    private bool m_isFiring = false;

    [Tooltip("Rate of fire (Rounds per minute).")]
    public float m_rpm = 60;

    [Tooltip("Number of ammo in a magazine.")]
    public int m_magazine = 100;
    private int m_currentAmmo;

    [Tooltip("Time in seconds to reload a magazine.")]
    public float m_reloadTime = 1f;

    [Tooltip("Optimal range to use weapon.")]
    [Range(0f, 100f)]
    public float m_optimalRange = 25f;

    [Tooltip("Optimal range to use weapon.")]
    [Range(0f, 10f)]
    public float m_precision = 0f;

    void Start ()
    {
        m_currentAmmo = m_magazine;
	}

    public bool IsInAim(Vector3 targetPosition, float imprecisionAngle)
    {
        Vector3 targetDir = targetPosition - m_muzzle.position;
        float angle = Vector3.Angle(targetDir, m_muzzle.forward);

        return (angle <= imprecisionAngle);
    }

    public bool IsInOptimalRange(Vector3 targetPosition)
    {
        return (Vector3.Distance(targetPosition, m_muzzle.position) < m_optimalRange);
    }
	
    public void FireWeapon()
    {
        Quaternion spread = Quaternion.Euler(Random.Range(-m_precision, m_precision), Random.Range(-m_precision, m_precision), Random.Range(-m_precision, m_precision));
        Instantiate(m_ammo, m_muzzle.position, m_muzzle.rotation * spread);
        Instantiate(m_muzzleFlash, m_muzzle.position, m_muzzle.rotation);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_reloadTime);
        m_currentAmmo = m_magazine;
        //Debug.Log("Reloaded");
    }

    IEnumerator AutoFire()
    {
        m_isFiring = true;
        while (m_isFiring)
        {
            if (m_currentAmmo > 0)
            {
                yield return new WaitForSeconds(60 / m_rpm);
                FireWeapon();
                m_currentAmmo--;
            }
            else
            {
                yield return StartCoroutine(Reload());
            }
        }
    }

    public void TriggerPressed()
    {
        switch (m_firingMethod)
        {
            case FiringMethod.SemiAutomatic:
                FireWeapon();
                break;
            case FiringMethod.Burst:
                break;
            case FiringMethod.Automatic:
                if (!m_isFiring)
                    StartCoroutine(AutoFire());
                break;
            default:
                break;
        }
    }

    public void TriggerReleased()
    {
        m_isFiring = false;
    }

    void Update ()
    {
	    
	}
}
