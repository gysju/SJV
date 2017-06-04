using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MenuManager
{
    public CanvasGroup m_title;
    public CanvasGroup m_levelSelecter;
    public CanvasGroup m_optionMenu;
    public CanvasGroup m_credits;

    void Start()
    {
        Time.timeScale = 1f;
    }

    protected override void FindPlayer()
    {
        base.FindPlayer();
        m_player.ResetWeapons();
        m_player.BackToBase();
        HUD_Radar.Instance.RemoveAllInfos();
    }

    protected override IEnumerator PlayerArrival()
    {
        m_player.m_interface.m_textHelmet.Extraction();
        yield return new WaitForSeconds(2f);
        m_player.m_interface.m_textHelmet.Nothing();
        BunkerOff();
        m_player.m_interface.HideHelmetHUD();
    }

    #region Main Menu
    public void StartButton()
    {
        HideMenu(m_title);
        ShowMenu(m_levelSelecter);
        HideMenu(m_optionMenu);
        HideMenu(m_credits);
    }

    public void Button01()
    {
        LoadLevel(2);
    }

    public void Button02()
    {
        LoadLevel(3);
    }

    public void OptionsButton()
    {
        HideMenu(m_title);
        HideMenu(m_levelSelecter);
        ShowMenu(m_optionMenu);
        HideMenu(m_credits);
    }

    public void CreditsButton()
    {
        HideMenu(m_title);
        HideMenu(m_levelSelecter);
        HideMenu(m_optionMenu);
        ShowMenu(m_credits);
    }
    #endregion
}
