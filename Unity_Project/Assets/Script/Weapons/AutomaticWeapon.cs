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

    public bool m_canOverHeat = true;
    const float m_maxHeat = 1f;
    protected float m_currentHeat = 0f;
    [Range(0f, 1f)]
    public float m_heatByShot = 0f;
    [Range(0.5f, 3f)]
    public float m_timeToCooldown = 0.5f;
    private bool m_overHeated = false;

    private bool m_isFiring = false;

    [Range(1, 600)]
    public float m_rpm;
    protected Material m_muzzleMaterial;
	protected Material m_muzzleSecondeMaterial;

    private Coroutine m_firingWeapon = null;

    protected override void Start()
    {
        base.Start();

		m_muzzleMaterial = GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
		m_muzzleSecondeMaterial = GetComponentInChildren<SkinnedMeshRenderer>().materials[1];
    }

    IEnumerator FiringWeapon(MoveController moveController)
    {
        m_isFiring = true;
        while (m_currentHeat <= m_maxHeat)
        {
            FireWeapon(moveController);
            m_currentHeat += m_heatByShot;
            yield return new WaitForSeconds(60f / m_rpm);
        }
        m_overHeated = true;
        m_isFiring = false;
    }

    public override void TriggerPressed(MoveController moveController = null)
    {
        switch (m_triggerType)
        {
            case WeaponTriggerType.SemiAutomatic:
                base.TriggerPressed(moveController);
                break;
            case WeaponTriggerType.Automatic:
                if (!m_overHeated)
                {
                    m_firingWeapon = StartCoroutine(FiringWeapon(moveController));
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

    public override float GetHeat()
    {
        return m_currentHeat;
    }

    protected void Update()
    {
        switch (m_triggerType)
        {
            case WeaponTriggerType.SemiAutomatic:
                break;
			case WeaponTriggerType.Automatic:
				if (!m_isFiring) {
					m_currentHeat = Mathf.Max (m_currentHeat - Time.deltaTime * m_timeToCooldown, 0f);
					if (m_currentHeat == 0f) {
						m_overHeated = false;
					}
				}
				m_muzzleMaterial.SetFloat ("_OverHeatRange", m_currentHeat);
				m_muzzleSecondeMaterial.SetFloat ("_OverHeatRange", m_currentHeat);

                break;
            default:
                break;
        }
    }
}
