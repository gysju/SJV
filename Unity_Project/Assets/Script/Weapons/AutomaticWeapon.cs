using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MechaVR/Weapon/AutomaticWeapon")]
public class AutomaticWeapon : SemiAutomaticWeapon
{
    public enum WeaponTriggerType
    {
        SemiAutomatic,
        Automatic
    }

    public WeaponTriggerType m_triggerType = WeaponTriggerType.Automatic;

    public float m_maxBurstTime;
    protected float m_currentBurstTime;

    private bool m_isFiring = false;

    [Range(1, 600)]
    public float m_rpm;
    private bool m_overHeated = false;
    public Material m_muzzleMaterial;
    protected Color m_defaultMuzzleColor;

    private Coroutine m_firingWeapon = null;

    protected override void Start()
    {
        base.Start();
        m_currentBurstTime = m_maxBurstTime;
		m_muzzleMaterial = GetComponentInChildren<SkinnedMeshRenderer> ().materials [1];
        m_defaultMuzzleColor = m_muzzleMaterial.color;
    }

    IEnumerator FiringWeapon()
    {
        m_isFiring = true;
        while (m_currentBurstTime > 0)
        {
            FireWeapon();
            yield return new WaitForSeconds(60f / m_rpm);
        }
        m_overHeated = true;
        m_isFiring = false;
    }

    public override void TriggerPressed()
    {
        switch (m_triggerType)
        {
            case WeaponTriggerType.SemiAutomatic:
                base.TriggerPressed();
                break;
            case WeaponTriggerType.Automatic:
                if (!m_overHeated)
                {
                    m_firingWeapon = StartCoroutine(FiringWeapon());
                }
                break;
            default:
                break;
        }
    }

    public override void TriggerReleased()
    {
        switch (m_triggerType)
        {
            case WeaponTriggerType.SemiAutomatic:
                base.TriggerReleased();
                break;
            case WeaponTriggerType.Automatic:
                StopCoroutine(m_firingWeapon);
                m_isFiring = false;
                break;
            default:
                break;
        }
    }

    protected void Update()
    {
        switch (m_triggerType)
        {
            case WeaponTriggerType.SemiAutomatic:
                break;
            case WeaponTriggerType.Automatic:
                if (m_isFiring)
                {
                    m_currentBurstTime -= Time.deltaTime;
                }
                else
                {
                    m_currentBurstTime = Mathf.Min(m_currentBurstTime + Time.deltaTime * 2f, m_maxBurstTime);
                    if (m_currentBurstTime == m_maxBurstTime)
                    {
                        m_overHeated = false;
                    }
                }
                m_muzzleMaterial.color = Color.Lerp(m_defaultMuzzleColor * Color.red, m_defaultMuzzleColor, m_currentBurstTime / m_maxBurstTime);
                break;
            default:
                break;
        }
    }

    void OnApplicationQuit()
    {
        m_muzzleMaterial.color = m_defaultMuzzleColor;
    }
}
