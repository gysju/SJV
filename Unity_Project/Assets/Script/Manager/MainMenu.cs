using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MenuManager
{
    public CanvasGroup m_title;
    public CanvasGroup m_optionMenu;
    public CanvasGroup m_credits;

    protected override void FindPlayer()
    {
        base.FindPlayer();
        m_player.BackToBase();
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
}
