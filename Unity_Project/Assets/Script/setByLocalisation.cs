using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class setByLocalisation : MonoBehaviour {

	public enum Root { no_selected = 0, Root_Menu, Root_Gameplay}
	public Root root;

	public enum Gameplay { no_selected = 0, Gameplay_HUD, GamePlay_PauseMenu}
	public enum Menu { no_selected = 0, Menu_SetupScene, Menu_Intro}
	public enum SetupScene { no_selected = 0, SetupScene_HmdSetup, SetupScene_UI_Interaction, SetupScene_Recenter, SetupScene_Finished}
	public enum Intro { no_selected = 0, Intro_MainMenu}

	public Gameplay gameplay = Gameplay.no_selected;
	public Menu menu = Menu.no_selected;
	public SetupScene setupScene = SetupScene.no_selected;
	public Intro intro = Intro.no_selected;

	public Text titleText = null;
	public Text descriptionText = null;
	public Text[] textButton;

	void Start () 
	{
		XmlManager.onChangedLanguage += setLanguage;
		setLanguage ();
	}

	void setLanguage( )
	{
        if (this == null)
            return;

        if (setupScene != SetupScene.no_selected)
			setSetupScene ();
        if (intro != Intro.no_selected)
            setIntro();
        if (gameplay != Gameplay.no_selected)
            setGameplay();
	}

	void setSetupScene()
	{
		switch( setupScene )
		{
			case SetupScene.SetupScene_HmdSetup:
                titleText.text = XmlManager.Instance.GetSetupScene().HMD_Setup.Title_Text.Text;
				descriptionText.text = XmlManager.Instance.GetSetupScene ().HMD_Setup.Description_Text.Text;
			break;
			case SetupScene.SetupScene_UI_Interaction:
				textButton[0].text = XmlManager.Instance.GetSetupScene ().UI_Interaction.Button_Text.Text;
				titleText.text = XmlManager.Instance.GetSetupScene ().UI_Interaction.Title_Text.Text;
				descriptionText.text = XmlManager.Instance.GetSetupScene ().UI_Interaction.Description_Text.Text;
				break;
			case SetupScene.SetupScene_Recenter:
                titleText.text = XmlManager.Instance.GetSetupScene().Recenter.Title_Text.Text;
                descriptionText.text = XmlManager.Instance.GetSetupScene().Recenter.Description_Text.Text;
                break;
			case SetupScene.SetupScene_Finished:
                textButton[0].text = XmlManager.Instance.GetSetupScene().Finished.Button_Text.Text;
                titleText.text = XmlManager.Instance.GetSetupScene().Finished.Title_Text.Text;
                descriptionText.text = XmlManager.Instance.GetSetupScene().Finished.Description_Text.Text;
                break;
		}
	}

    void setIntro()
    {
        textButton[0].text = XmlManager.Instance.GetIntro().Main_Menu.Button_Start.Text;
        textButton[1].text = XmlManager.Instance.GetIntro().Main_Menu.Button_Options.Text;
        textButton[2].text = XmlManager.Instance.GetIntro().Main_Menu.Button_Credits.Text;
    }

    void setPauseMenu()
    {
        textButton[0].text = XmlManager.Instance.GetGameplay().Pause_Menu.Button_Resume.Text;
        textButton[1].text = XmlManager.Instance.GetGameplay().Pause_Menu.Button_Return.Text;
    }

    void setHUD()
    {
        textButton[0].text = XmlManager.Instance.GetGameplay().HUD.InfoCockpit.Text;
        textButton[1].text = XmlManager.Instance.GetGameplay().HUD.InfoLeftWeapons.Text;
        textButton[2].text = XmlManager.Instance.GetGameplay().HUD.InfoRightWeapons.Text;
    }

    void setGameplay()
    {
        switch (gameplay)
        {
            case Gameplay.Gameplay_HUD:
                setHUD();
                break;
            case Gameplay.GamePlay_PauseMenu:
                setPauseMenu();
                break;
        }
    }
}
