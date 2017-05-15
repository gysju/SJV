using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

// Xml2CSharp generator http://xmltocsharp.azurewebsites.net/
namespace Xml2CSharp
{
    [XmlRoot(ElementName = "Title_Text")]
    public class Title_Text
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "font-style")]
        public string Fontstyle { get; set; }
        [XmlAttribute(AttributeName = "font-size")]
        public string Fontsize { get; set; }
        [XmlAttribute(AttributeName = "text-align")]
        public string Textalign { get; set; }
    }

    [XmlRoot(ElementName = "Description_Text")]
    public class Description_Text
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "font-style")]
        public string Fontstyle { get; set; }
        [XmlAttribute(AttributeName = "font-size")]
        public string Fontsize { get; set; }
        [XmlAttribute(AttributeName = "text-align")]
        public string Textalign { get; set; }
    }

    [XmlRoot(ElementName = "HMD_Setup")]
    public class HMD_Setup
    {
        [XmlElement(ElementName = "Title_Text")]
        public Title_Text Title_Text { get; set; }
        [XmlElement(ElementName = "Description_Text")]
        public Description_Text Description_Text { get; set; }
    }

    [XmlRoot(ElementName = "Button_Text")]
    public class Button_Text
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "font-style")]
        public string Fontstyle { get; set; }
        [XmlAttribute(AttributeName = "font-size")]
        public string Fontsize { get; set; }
        [XmlAttribute(AttributeName = "text-align")]
        public string Textalign { get; set; }
    }

    [XmlRoot(ElementName = "UI_Interaction")]
    public class UI_Interaction
    {
        [XmlElement(ElementName = "Title_Text")]
        public Title_Text Title_Text { get; set; }
        [XmlElement(ElementName = "Description_Text")]
        public Description_Text Description_Text { get; set; }
        [XmlElement(ElementName = "Button_Text")]
        public Button_Text Button_Text { get; set; }
    }

    [XmlRoot(ElementName = "Recenter")]
    public class Recenter
    {
        [XmlElement(ElementName = "Title_Text")]
        public Title_Text Title_Text { get; set; }
        [XmlElement(ElementName = "Description_Text")]
        public Description_Text Description_Text { get; set; }
    }

    [XmlRoot(ElementName = "Finished")]
    public class Finished
    {
        [XmlElement(ElementName = "Title_Text")]
        public Title_Text Title_Text { get; set; }
        [XmlElement(ElementName = "Description_Text")]
        public Description_Text Description_Text { get; set; }
        [XmlElement(ElementName = "Button_Text")]
        public Button_Text Button_Text { get; set; }
    }

    [XmlRoot(ElementName = "Setup_scene")]
    public class Setup_scene
    {
        [XmlElement(ElementName = "HMD_Setup")]
        public HMD_Setup HMD_Setup { get; set; }
        [XmlElement(ElementName = "UI_Interaction")]
        public UI_Interaction UI_Interaction { get; set; }
        [XmlElement(ElementName = "Recenter")]
        public Recenter Recenter { get; set; }
        [XmlElement(ElementName = "Finished")]
        public Finished Finished { get; set; }
    }

    [XmlRoot(ElementName = "Button_Start")]
    public class Button_Start
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "font-style")]
        public string Fontstyle { get; set; }
        [XmlAttribute(AttributeName = "font-size")]
        public string Fontsize { get; set; }
        [XmlAttribute(AttributeName = "text-align")]
        public string Textalign { get; set; }
    }

    [XmlRoot(ElementName = "Button_Options")]
    public class Button_Options
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "font-style")]
        public string Fontstyle { get; set; }
        [XmlAttribute(AttributeName = "font-size")]
        public string Fontsize { get; set; }
        [XmlAttribute(AttributeName = "text-align")]
        public string Textalign { get; set; }
    }

    [XmlRoot(ElementName = "Button_Credits")]
    public class Button_Credits
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "font-style")]
        public string Fontstyle { get; set; }
        [XmlAttribute(AttributeName = "font-size")]
        public string Fontsize { get; set; }
        [XmlAttribute(AttributeName = "text-align")]
        public string Textalign { get; set; }
    }

    [XmlRoot(ElementName = "Main_Menu")]
    public class Main_Menu
    {
        [XmlElement(ElementName = "Button_Start")]
        public Button_Start Button_Start { get; set; }
        [XmlElement(ElementName = "Button_Options")]
        public Button_Options Button_Options { get; set; }
        [XmlElement(ElementName = "Button_Credits")]
        public Button_Credits Button_Credits { get; set; }
    }

    [XmlRoot(ElementName = "Intro")]
    public class Intro
    {
        [XmlElement(ElementName = "Main_Menu")]
        public Main_Menu Main_Menu { get; set; }
    }

    [XmlRoot(ElementName = "Menu")]
    public class Menu
    {
        [XmlElement(ElementName = "Setup_scene")]
        public Setup_scene Setup_scene { get; set; }
        [XmlElement(ElementName = "Intro")]
        public Intro Intro { get; set; }
    }

    [XmlRoot(ElementName = "Button_Resume")]
    public class Button_Resume
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "font-style")]
        public string Fontstyle { get; set; }
        [XmlAttribute(AttributeName = "font-size")]
        public string Fontsize { get; set; }
        [XmlAttribute(AttributeName = "text-align")]
        public string Textalign { get; set; }
    }

    [XmlRoot(ElementName = "Button_Return")]
    public class Button_Return
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "font-style")]
        public string Fontstyle { get; set; }
        [XmlAttribute(AttributeName = "font-size")]
        public string Fontsize { get; set; }
        [XmlAttribute(AttributeName = "text-align")]
        public string Textalign { get; set; }
    }

    [XmlRoot(ElementName = "Pause_Menu")]
    public class Pause_Menu
    {
        [XmlElement(ElementName = "Button_Resume")]
        public Button_Resume Button_Resume { get; set; }
        [XmlElement(ElementName = "Button_Return")]
        public Button_Return Button_Return { get; set; }
    }

    [XmlRoot(ElementName = "Gameplay")]
    public class Gameplay
    {
        [XmlElement(ElementName = "HUD")]
        public string HUD { get; set; }
        [XmlElement(ElementName = "Pause_Menu")]
        public Pause_Menu Pause_Menu { get; set; }
    }

    [XmlRoot(ElementName = "French")]
    public class French
    {
        [XmlElement(ElementName = "Menu")]
        public Menu Menu { get; set; }
        [XmlElement(ElementName = "Gameplay")]
        public Gameplay Gameplay { get; set; }
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "English")]
    public class English
    {
        [XmlElement(ElementName = "Menu")]
        public Menu Menu { get; set; }
        [XmlElement(ElementName = "Gameplay")]
        public Gameplay Gameplay { get; set; }
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Dutch")]
    public class Dutch
    {
        [XmlElement(ElementName = "Menu")]
        public Menu Menu { get; set; }
        [XmlElement(ElementName = "Gameplay")]
        public Gameplay Gameplay { get; set; }
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "German")]
    public class German
    {
        [XmlElement(ElementName = "Menu")]
        public Menu Menu { get; set; }
        [XmlElement(ElementName = "Gameplay")]
        public Gameplay Gameplay { get; set; }
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Languages")]
    public class Languages
    {
        [XmlElement(ElementName = "French")]
        public French French { get; set; }
        [XmlElement(ElementName = "English")]
        public English English { get; set; }
        [XmlElement(ElementName = "Dutch")]
        public Dutch Dutch { get; set; }
        [XmlElement(ElementName = "German")]
        public German German { get; set; }
    }

}

public class XmlManager : MonoBehaviour
{
	public delegate void onChangeLanguage ();
	public static event onChangeLanguage onChangedLanguage;

    public static XmlManager Instance { get; private set; }
    public enum Language { French = 0, English, Dutch, German };
    public Language languageSelected = Language.English;
    Xml2CSharp.Languages languages;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        XmlSerializer serial = new XmlSerializer(typeof(Xml2CSharp.Languages));

        //Stream reader = new FileStream("Assets/Resources/Language.xml", FileMode.Open);
		TextAsset textAsset = (TextAsset)Resources.Load("Language");
		languages = (Xml2CSharp.Languages)serial.Deserialize(new StringReader (textAsset.text));
    }

	public Xml2CSharp.Setup_scene GetSetupScene()
	{
		return GetMenu().Setup_scene;
	}

	public Xml2CSharp.Intro GetIntro()
	{
		return GetMenu().Intro;
	}

    public Xml2CSharp.Menu GetMenu()
    {
		switch (languageSelected)
		{
		case Language.French:
			return languages.French.Menu;
		case Language.English:
			return languages.English.Menu;
		case Language.Dutch:
			return languages.Dutch.Menu;
		case Language.German:
			return languages.German.Menu;
		}
		return null;
    }

	public Xml2CSharp.Gameplay GetGameplay()
	{
		switch (languageSelected)
		{
		case Language.French:
			return languages.French.Gameplay;
		case Language.English:
			return languages.English.Gameplay;
		case Language.Dutch:
			return languages.Dutch.Gameplay;
		case Language.German:
			return languages.German.Gameplay;
		}
		return null;
	}

	public void ChangeLanguage( Language newLanguage )
	{
		languageSelected = newLanguage;
		onChangedLanguage ();
	}
}