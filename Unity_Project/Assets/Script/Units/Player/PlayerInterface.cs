using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerInterface : MonoBehaviour
{
	public static PlayerInterface Instance = null;
    public BaseMecha m_mecha;
    public PlayerInputs m_inputs;
        
    [Header("Helmet HUD")]
    public CanvasGroup m_helmetHUD;
    public Image m_integrityGaugeRight;
    public Image m_integrityGaugeLeft;

    public Image m_heatGaugeRight;
    public Image m_heatGaugeLeft;

    [Header("Debug")]
    public GameObject m_fps;
    public bool m_showFPS;

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

        if (m_showFPS) m_fps.SetActive(true);
    }

    public void ShowHelmetHUD()
    {
        m_helmetHUD.alpha = 1;
    }

    public void HideHelmetHUD()
    {
        m_helmetHUD.alpha = 0;
    }

    void Update ()
	{
        m_integrityGaugeRight.fillAmount = (float)m_mecha.GetCurrentHitPoints() / m_mecha.m_maxHitPoints;
        m_integrityGaugeLeft.fillAmount = (float)m_mecha.GetCurrentHitPoints() / m_mecha.m_maxHitPoints;

        m_heatGaugeRight.fillAmount = m_mecha.GetRightWeaponHeat();
        m_heatGaugeLeft.fillAmount = m_mecha.GetLeftWeaponHeat();
    }
}
