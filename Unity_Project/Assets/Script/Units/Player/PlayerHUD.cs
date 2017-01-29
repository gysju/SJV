using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHUD : MonoBehaviour
{
    public BaseMecha m_mecha;

    public CanvasGroup m_hud;

    public Image m_integrityGaugeRight;
    public Image m_integrityGaugeLeft;

    void Start ()
	{
        if (!m_mecha) m_mecha = GetComponentInParent<BaseMecha>();
    }
	
	void Update ()
	{
        m_integrityGaugeRight.fillAmount = (float)m_mecha.GetCurrentHitPoints() / m_mecha.m_maxHitPoints;
        m_integrityGaugeLeft.fillAmount = (float)m_mecha.GetCurrentHitPoints() / m_mecha.m_maxHitPoints;

    }
}
