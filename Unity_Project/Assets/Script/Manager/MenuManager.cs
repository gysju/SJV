using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : ScenePlayerManager
{
    [Header("Main Menu")]
    public CanvasGroup m_title;
    public CanvasGroup m_optionMenu;
    public CanvasGroup m_credits;

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

    IEnumerator LoadLevelCoroutine(int sceneID)
    {
        m_player.m_bunker.ActivateBunkerMode();
        yield return new WaitForSeconds(m_player.m_bunker.m_bunkerTransitionSpeed);
        m_player.m_levelLoading = SceneManager.LoadSceneAsync(sceneID);
    }

    #region Main Menu
    public void StartButton()
    {
        LoadLevel(2);
    }

    public void OptionsButton()
    {
        HideMenu(m_title);
        ShowMenu(m_optionMenu);
        HideMenu(m_credits);
    }

    public void CreditsButton()
    {
        HideMenu(m_title);
        HideMenu(m_optionMenu);
        ShowMenu(m_credits);
    }
    #endregion

    protected override void Update()
    {
        base.Update();
    }
}
