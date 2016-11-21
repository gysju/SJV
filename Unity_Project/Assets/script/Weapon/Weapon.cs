using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Weapon/DEV/Weapon")]
public class Weapon : MonoBehaviour
{
    [SerializeField]
    [Tooltip("What can shoot the weapon.")]
    protected LayerMask m_mask;

    public LineRenderer m_lineRenderer;

    [Tooltip("From where the ammo is fired.")]
    public Transform m_muzzle;

    [Tooltip("Graphical effect when firing.")]
    public GameObject m_muzzleFlash;

    public enum FiringMethod
    {
        SemiAutomatic,
        Burst,
        Automatic
    }
    [Header("Weapon Specs")]
    [Tooltip("Firing Method.")]
    protected FiringMethod m_firingMethod = FiringMethod.Automatic;

    private bool m_isFiring = false;

    [Tooltip("Rate of fire (Rounds per minute fired maximum).")]
    public float m_rpm = 60;

    [Tooltip("Number of ammo in a magazine.")]
    public int m_magazineSize = 100;
    private int m_ammoLeftInMagazine;

    [Tooltip("Time in seconds to reload a magazine.")]
    public float m_reloadTime = 1f;

    [Tooltip("Optimal range to use weapon.")]
    [Range(0f, 100f)]
    public float m_optimalRange = 25f;

    [Tooltip("Optimal range to use weapon.")]
    [Range(0f, 10f)]
    public float m_imprecision = 0f;

	public int Damage = 1;
	public int ArmorPenetration = 1;

    void Start ()
    {
        m_ammoLeftInMagazine = m_magazineSize;
        m_lineRenderer = GetComponent<LineRenderer>();
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

    protected Quaternion GetSpread()
    {
        return Quaternion.Euler(Random.Range(-m_imprecision, m_imprecision), Random.Range(-m_imprecision, m_imprecision), Random.Range(-m_imprecision, m_imprecision));
    }

    public virtual void FireWeapon()
    {
        RaycastHit hit;
        if (m_lineRenderer) m_lineRenderer.SetPosition(0, m_muzzle.position);
        Vector3 shotDirection = (GetSpread() * m_muzzle.forward);
        if (Physics.Raycast(m_muzzle.position, shotDirection, out hit, m_optimalRange, m_mask))
        {
            if (m_lineRenderer) m_lineRenderer.SetPosition(1, hit.point);
            Unit unitHit = hit.transform.GetComponentInParent<Unit>();
			if (unitHit) unitHit.ReceiveDamages(Damage, ArmorPenetration);
        }
        else
        {
            if (m_lineRenderer) m_lineRenderer.SetPosition(1, m_muzzle.position + (shotDirection * m_optimalRange));
        }
        Instantiate(m_muzzleFlash, m_muzzle.position, m_muzzle.rotation);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_reloadTime);
        m_ammoLeftInMagazine = m_magazineSize;
    }

    IEnumerator AutoFire()
    {
        m_isFiring = true;
        while (m_isFiring)
        {
            if (m_ammoLeftInMagazine > 0)
            {
                yield return new WaitForSeconds(60 / m_rpm);
                FireWeapon();
                m_ammoLeftInMagazine--;
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
