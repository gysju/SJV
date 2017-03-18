using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHUD : MonoBehaviour
{
	public static PlayerHUD Instance = null;
    public BaseMecha m_mecha;

    public CanvasGroup m_hud;

    public Image m_integrityGaugeRight;
    public Image m_integrityGaugeLeft;

    public Image m_heatGaugeRight;
    public Image m_heatGaugeLeft;

    private AsyncOperation m_levelLoading = null;

    void Start ()
	{
		if (Instance == null) 
		{
			Instance = this;
			if (!m_mecha) m_mecha = GetComponentInParent<BaseMecha>();
		}
		else if ( Instance != this)
		{
			
		}
    }
	
	void Update ()
	{
        m_integrityGaugeRight.fillAmount = (float)m_mecha.GetCurrentHitPoints() / m_mecha.m_maxHitPoints;
        m_integrityGaugeLeft.fillAmount = (float)m_mecha.GetCurrentHitPoints() / m_mecha.m_maxHitPoints;

        m_heatGaugeRight.fillAmount = m_mecha.GetRightWeaponHeat();
        m_heatGaugeLeft.fillAmount = m_mecha.GetLeftWeaponHeat();

        if (m_levelLoading != null)
        {
            if (m_levelLoading.isDone)
            {
                m_mecha.m_bunker.DeactivateBunkerMode();
                m_levelLoading = null;
            }
        }
    }
}
