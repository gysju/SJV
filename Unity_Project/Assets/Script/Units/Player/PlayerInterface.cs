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

    [Header("Pause menu")]
    public CanvasGroup m_pauseMenu;

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
            if (m_showFPS) m_fps.SetActive(true);
        }
		else if ( Instance != this)
		{
			
		}
    }

    #region Helmet HUD
    public void ShowHelmetHUD()
    {
        m_helmetHUD.alpha = 1;
    }

    public void HideHelmetHUD()
    {
        m_helmetHUD.alpha = 0;
    }
    #endregion

    #region Pause menu
    protected void ShowPauseMenu()
    {
        m_pauseMenu.alpha = 1f;
        m_pauseMenu.interactable = true;
        m_pauseMenu.blocksRaycasts = true;
    }

    protected void HidePauseMenu()
    {
        m_pauseMenu.alpha = 0f;
        m_pauseMenu.interactable = false;
        m_pauseMenu.blocksRaycasts = false;
    }

    public void StartPause()
    {
        Time.timeScale = 0f;
        m_inputs.m_inGame = false;
        m_mecha.ResetWeapons();

#if UNITY_STANDALONE
        Camera.main.transform.localRotation = Quaternion.identity;
#endif
        ShowPauseMenu();
        //StartCoroutine(SoundManager.Instance.PauseAudioSource());
    }

    public void Resume()
    {
        HidePauseMenu();
        m_inputs.m_inGame = true;
        Time.timeScale = 1f;
        //StartCoroutine(SoundManager.Instance.UnPauseAudioSource());
    }

    IEnumerator BackToBaseCoroutine()
    {
        HidePauseMenu();
        m_mecha.PrepareExtraction();
        Time.timeScale = 1f;
        yield return new WaitForSeconds(m_mecha.m_bunker.m_bunkerTransitionSpeed);
        HUD_Radar.Instance.RemoveAllInfos();
        SceneManager.LoadSceneAsync(1);
    }

    public void BackToBase()
    {
        StartCoroutine(BackToBaseCoroutine());
    }
    #endregion
    void Update ()
	{
        m_integrityGaugeRight.fillAmount = (float)m_mecha.GetCurrentHitPoints() / m_mecha.m_maxHitPoints;
        m_integrityGaugeLeft.fillAmount = (float)m_mecha.GetCurrentHitPoints() / m_mecha.m_maxHitPoints;

        m_heatGaugeRight.fillAmount = m_mecha.GetRightWeaponHeat();
        m_heatGaugeLeft.fillAmount = m_mecha.GetLeftWeaponHeat();
    }
}
