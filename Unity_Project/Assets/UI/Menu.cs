﻿using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
    #region static&constant
    #endregion

    #region variables

    public bool bOnClick = false;
    public string NextState { get; private set;}
    public string PreviousState;

	private XmlManager languageManager;

    #endregion

    #region fields
    #endregion

    #region functions

	void Start()
	{
		languageManager = GameObject.FindObjectOfType<XmlManager>();
	}

    public void OnClick(string StateSelected)
    {
        bOnClick = true;
        NextState = StateSelected;
    }

	public void ChangeLanguageToFrench()
	{
		languageManager.ChangeLanguage(XmlManager.Language.French);
	}

	public void ChangeLanguageToEnglish()
	{
		languageManager.ChangeLanguage(XmlManager.Language.English);
	}

	public void ChangeLanguageToDutch()
	{
		languageManager.ChangeLanguage(XmlManager.Language.Dutch);
	}

	public void ChangeLanguageToGerman()
	{
		languageManager.ChangeLanguage(XmlManager.Language.German);
	}

    void Update()
    {
		if (Input.GetButtonDown("Start") && PreviousState != null && PreviousState != "")
		{
		   bOnClick = true;
		   NextState = PreviousState;
		}
    }
    #endregion

    #region events
    #endregion
}
