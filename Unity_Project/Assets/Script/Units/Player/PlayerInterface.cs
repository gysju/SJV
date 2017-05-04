﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerInterface : MonoBehaviour
{
	public static PlayerInterface Instance = null;
    public BaseMecha m_mecha;
    public PlayerInputs m_inputs;

    [Header("Setup Scene")]
    public CanvasGroup m_setupBoard;
    public CanvasGroup m_resetCamBoard;
    
    [Header("Helmet HUD")]
    public CanvasGroup m_helmetHUD;
    public Image m_integrityGaugeRight;
    public Image m_integrityGaugeLeft;

    public Image m_heatGaugeRight;
    public Image m_heatGaugeLeft;

    [Header("Debug")]
    public GameObject m_fps;
    public bool m_showFPS;

    private AsyncOperation m_levelUnloading = null;
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

        if (m_showFPS) m_fps.SetActive(true);
    }

    protected void ShowBoard(CanvasGroup boardToShow)
    {
        boardToShow.alpha = 1f;
    }

    protected void HideBoard(CanvasGroup boardToHide)
    {
        boardToHide.alpha = 0f;
    }

    protected void ShowMenu(CanvasGroup menuToShow)
    {
        ShowBoard(menuToShow);
        menuToShow.blocksRaycasts = true;
        menuToShow.interactable = true;
    }

    protected void HideMenu(CanvasGroup menuToHide)
    {
        HideBoard(menuToHide);
        menuToHide.blocksRaycasts = false;
        menuToHide.interactable = false;
    }

    protected void LoadLevel(int sceneID)
    {
        StartCoroutine(LoadLevelCoroutine(sceneID));
    }

    protected void UnloadLevel()
    {
        m_levelUnloading = SceneManager.LoadSceneAsync(1);
    }

    IEnumerator LoadLevelCoroutine(int sceneID)
    {
        m_mecha.m_bunker.ActivateBunkerMode();
        yield return new WaitForSeconds(m_mecha.m_bunker.m_bunkerTransitionSpeed);
        m_levelLoading = SceneManager.LoadSceneAsync(sceneID);
    }

    public void ShowHelmetHUD()
    {
        ShowBoard(m_helmetHUD);
    }

    #region Setup Scene
    public void SetupBoard()
    {
        ShowBoard(m_setupBoard);
    }

    public void ResetCamBoard()
    {
        HideBoard(m_setupBoard);
        ShowBoard(m_resetCamBoard);
    }
    #endregion

    #region InGame
    public void BackToMainMenu()
    {
        HideBoard(m_helmetHUD);
        UnloadLevel();
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
