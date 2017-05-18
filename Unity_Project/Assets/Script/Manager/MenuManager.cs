using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : ScenePlayerManager
{
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
        m_player.PrepareExtraction();
        yield return new WaitForSeconds(m_player.m_bunker.m_bunkerTransitionSpeed);
        m_player.m_levelLoading = SceneManager.LoadSceneAsync(sceneID);
    }
}
