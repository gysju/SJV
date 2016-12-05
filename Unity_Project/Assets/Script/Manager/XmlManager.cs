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
	[XmlRoot(ElementName="Title_Text")]
	public class Title_Text {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="Description_text")]
	public class Description_text {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="HMD_Setup")]
	public class HMD_Setup {
		[XmlElement(ElementName="Title_Text")]
		public Title_Text Title_Text { get; set; }
		[XmlElement(ElementName="Description_text")]
		public Description_text Description_text { get; set; }
	}

	[XmlRoot(ElementName="Look_here")]
	public class Look_here {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="Button")]
	public class Button {
		[XmlElement(ElementName="Look_here")]
		public Look_here Look_here { get; set; }
		[XmlElement(ElementName="Main_Menu")]
		public Main_Menu Main_Menu { get; set; }
		[XmlElement(ElementName="Start")]
		public Start Start { get; set; }
		[XmlElement(ElementName="Options")]
		public Options Options { get; set; }
		[XmlElement(ElementName="Return")]
		public Return Return { get; set; }
	}

	[XmlRoot(ElementName="UI_Interaction")]
	public class UI_Interaction {
		[XmlElement(ElementName="Title_Text")]
		public Title_Text Title_Text { get; set; }
		[XmlElement(ElementName="Description_text")]
		public Description_text Description_text { get; set; }
		[XmlElement(ElementName="Button")]
		public Button Button { get; set; }
	}

	[XmlRoot(ElementName="Recenter")]
	public class Recenter {
		[XmlElement(ElementName="Title_Text")]
		public Title_Text Title_Text { get; set; }
		[XmlElement(ElementName="Description_text")]
		public Description_text Description_text { get; set; }
	}

	[XmlRoot(ElementName="Main_Menu")]
	public class Main_Menu {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
		[XmlElement(ElementName="Button")]
		public List<Button> Button { get; set; }
	}

	[XmlRoot(ElementName="Finished")]
	public class Finished {
		[XmlElement(ElementName="Title_Text")]
		public Title_Text Title_Text { get; set; }
		[XmlElement(ElementName="Description_text")]
		public Description_text Description_text { get; set; }
		[XmlElement(ElementName="Button")]
		public Button Button { get; set; }
	}

	[XmlRoot(ElementName="Setup_scene")]
	public class Setup_scene {
		[XmlElement(ElementName="HMD_Setup")]
		public HMD_Setup HMD_Setup { get; set; }
		[XmlElement(ElementName="UI_Interaction")]
		public UI_Interaction UI_Interaction { get; set; }
		[XmlElement(ElementName="Recenter")]
		public Recenter Recenter { get; set; }
		[XmlElement(ElementName="Finished")]
		public Finished Finished { get; set; }
	}

	[XmlRoot(ElementName="Start")]
	public class Start {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="Options")]
	public class Options {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
		[XmlElement(ElementName="Button")]
		public Button Button { get; set; }
	}

	[XmlRoot(ElementName="Return")]
	public class Return {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="Intro")]
	public class Intro {
		[XmlElement(ElementName="Main_Menu")]
		public Main_Menu Main_Menu { get; set; }
		[XmlElement(ElementName="Options")]
		public Options Options { get; set; }
	}

	[XmlRoot(ElementName="Menu")]
	public class Menu {
		[XmlElement(ElementName="Setup_scene")]
		public Setup_scene Setup_scene { get; set; }
		[XmlElement(ElementName="Intro")]
		public Intro Intro { get; set; }
	}

	[XmlRoot(ElementName="Life")]
	public class Life {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="Radar")]
	public class Radar {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="Shield")]
	public class Shield {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="LaserGun")]
	public class LaserGun {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="MachineGun")]
	public class MachineGun {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="RocketGun")]
	public class RocketGun {
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
		[XmlAttribute(AttributeName="font-style")]
		public string Fontstyle { get; set; }
		[XmlAttribute(AttributeName="font-size")]
		public string Fontsize { get; set; }
		[XmlAttribute(AttributeName="text-align")]
		public string Textalign { get; set; }
	}

	[XmlRoot(ElementName="Weapons")]
	public class Weapons {
		[XmlElement(ElementName="LaserGun")]
		public LaserGun LaserGun { get; set; }
		[XmlElement(ElementName="MachineGun")]
		public MachineGun MachineGun { get; set; }
		[XmlElement(ElementName="RocketGun")]
		public RocketGun RocketGun { get; set; }
	}

	[XmlRoot(ElementName="HUD")]
	public class HUD {
		[XmlElement(ElementName="Life")]
		public Life Life { get; set; }
		[XmlElement(ElementName="Radar")]
		public Radar Radar { get; set; }
		[XmlElement(ElementName="Shield")]
		public Shield Shield { get; set; }
		[XmlElement(ElementName="Weapons")]
		public Weapons Weapons { get; set; }
	}

	[XmlRoot(ElementName="Gameplay")]
	public class Gameplay {
		[XmlElement(ElementName="HUD")]
		public HUD HUD { get; set; }
	}

	[XmlRoot(ElementName="French")]
	public class French {
		[XmlElement(ElementName="Menu")]
		public Menu Menu { get; set; }
		[XmlElement(ElementName="Gameplay")]
		public Gameplay Gameplay { get; set; }
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="English")]
	public class English {
		[XmlElement(ElementName="Menu")]
		public Menu Menu { get; set; }
		[XmlElement(ElementName="Gameplay")]
		public Gameplay Gameplay { get; set; }
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="Dutch")]
	public class Dutch {
		[XmlElement(ElementName="Menu")]
		public Menu Menu { get; set; }
		[XmlElement(ElementName="Gameplay")]
		public Gameplay Gameplay { get; set; }
		[XmlAttribute(AttributeName="value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName="German")]
	public class German {
		[XmlElement(ElementName="Menu")]
		public Menu Menu { get; set; }
		[XmlElement(ElementName="Gameplay")]
		public Gameplay Gameplay { get; set; }
		[XmlAttribute(AttributeName="text")]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName="Languages")]
	public class Languages {
		[XmlElement(ElementName="French")]
		public French French { get; set; }
		[XmlElement(ElementName="English")]
		public English English { get; set; }
		[XmlElement(ElementName="Dutch")]
		public Dutch Dutch { get; set; }
		[XmlElement(ElementName="German")]
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

    public void Start()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        XmlSerializer serial = new XmlSerializer(typeof(Xml2CSharp.Languages));
        Stream reader = new FileStream("Assets/Resources/Language.xml", FileMode.Open);
        languages = (Xml2CSharp.Languages)serial.Deserialize(reader);
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