using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class setByLocalisation : MonoBehaviour {

	public enum Root { no_selected = 0, Root_Menu, Root_Gameplay}
	public Root root;

	public enum Gameplay { no_selected = 0, Gameplay_HUD}
	public enum Menu { no_selected = 0, Menu_SetupScene, Menu_Intro}
	public enum SetupScene { no_selected = 0, SetupScene_HmdSetup, SetupScene_UI_Interaction, SetupScene_Recenter, SetupScene_Finished}
	public enum Intro { no_selected = 0, Intro_MainMenu, Intro_Options}

	public Gameplay gameplay = Gameplay.no_selected;
	public Menu menu = Menu.no_selected;
	public SetupScene setupScene = SetupScene.no_selected;
	public Intro intro = Intro.no_selected;

	public Text titleText = null;
	public Text descriptionText = null;
	public Text textButton = null;

	void Start () 
	{
		XmlManager.onChangedLanguage += setLanguage;
		setLanguage ();
	}

	void setLanguage( )
	{
		if (setupScene != 0)
			setSetupScene ();
		else if (intro != 0)
			setIntro ();
		else if (gameplay != 0)
			setGameplay ();
	}

	void setSetupScene()
	{
		switch( setupScene )
		{
			case SetupScene.SetupScene_HmdSetup:
				titleText = XmlManager.Instance.GetSetupScene ().HMD_Setup.Title_Text;
				descriptionText = XmlManager.Instance.GetSetupScene ().HMD_Setup.Description_text;
			break;
			case SetupScene.SetupScene_UI_Interaction:
				textButton = XmlManager.Instance.GetSetupScene ().UI_Interaction.Button;
				titleText = XmlManager.Instance.GetSetupScene ().UI_Interaction.Title_Text;
				descriptionText = XmlManager.Instance.GetSetupScene ().UI_Interaction.Description_text;
				break;
			case SetupScene.SetupScene_Recenter:
				XmlManager.Instance.GetSetupScene ().Recenter;
				break;
			case SetupScene.SetupScene_Finished:
				XmlManager.Instance.GetSetupScene ().Finished;
				break;
		}
	}

	void setIntro()
	{
		switch()
		{

		}
	}

	void setGameplay()
	{
		switch()
		{

		}
	}
}
