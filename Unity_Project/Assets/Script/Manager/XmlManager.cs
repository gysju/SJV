using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

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

    [XmlRoot(ElementName = "Description_text")]
    public class Description_text
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
        [XmlElement(ElementName = "Description_text")]
        public Description_text Description_text { get; set; }
    }

    [XmlRoot(ElementName = "Look_here")]
    public class Look_here
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

    [XmlRoot(ElementName = "Button")]
    public class Button
    {
        [XmlElement(ElementName = "Look_here")]
        public Look_here Look_here { get; set; }
        [XmlElement(ElementName = "Main_Menu")]
        public Main_Menu Main_Menu { get; set; }
        [XmlElement(ElementName = "Start")]
        public Start Start { get; set; }
        [XmlElement(ElementName = "Options")]
        public Options Options { get; set; }
        [XmlElement(ElementName = "Return")]
        public Return Return { get; set; }
    }

    [XmlRoot(ElementName = "UI_Interaction")]
    public class UI_Interaction
    {
        [XmlElement(ElementName = "Title_Text")]
        public Title_Text Title_Text { get; set; }
        [XmlElement(ElementName = "Description_text")]
        public Description_text Description_text { get; set; }
        [XmlElement(ElementName = "Button")]
        public Button Button { get; set; }
    }

    [XmlRoot(ElementName = "Recenter")]
    public class Recenter
    {
        [XmlElement(ElementName = "Title_Text")]
        public Title_Text Title_Text { get; set; }
        [XmlElement(ElementName = "Description_text")]
        public Description_text Description_text { get; set; }
    }

    [XmlRoot(ElementName = "Main_Menu")]
    public class Main_Menu
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "font-style")]
        public string Fontstyle { get; set; }
        [XmlAttribute(AttributeName = "font-size")]
        public string Fontsize { get; set; }
        [XmlAttribute(AttributeName = "text-align")]
        public string Textalign { get; set; }
        [XmlElement(ElementName = "Button")]
        public List<Button> Button { get; set; }
    }

    [XmlRoot(ElementName = "Finished")]
    public class Finished
    {
        [XmlElement(ElementName = "Title_Text")]
        public Title_Text Title_Text { get; set; }
        [XmlElement(ElementName = "Description_text")]
        public Description_text Description_text { get; set; }
        [XmlElement(ElementName = "Button")]
        public Button Button { get; set; }
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

    [XmlRoot(ElementName = "Start")]
    public class Start
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

    [XmlRoot(ElementName = "Options")]
    public class Options
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "font-style")]
        public string Fontstyle { get; set; }
        [XmlAttribute(AttributeName = "font-size")]
        public string Fontsize { get; set; }
        [XmlAttribute(AttributeName = "text-align")]
        public string Textalign { get; set; }
        [XmlElement(ElementName = "Button")]
        public Button Button { get; set; }
    }

    [XmlRoot(ElementName = "Return")]
    public class Return
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

    [XmlRoot(ElementName = "Intro")]
    public class Intro
    {
        [XmlElement(ElementName = "Main_Menu")]
        public Main_Menu Main_Menu { get; set; }
        [XmlElement(ElementName = "Options")]
        public Options Options { get; set; }
    }

    [XmlRoot(ElementName = "Menu")]
    public class Menu
    {
        [XmlElement(ElementName = "Setup_scene")]
        public Setup_scene Setup_scene { get; set; }
        [XmlElement(ElementName = "Intro")]
        public Intro Intro { get; set; }
    }

    [XmlRoot(ElementName = "UI")]
    public class UI
    {
        [XmlElement(ElementName = "Menu")]
        public Menu Menu { get; set; }
    }

    [XmlRoot(ElementName = "French")]
    public class French
    {
        [XmlElement(ElementName = "UI")]
        public UI UI { get; set; }
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "English")]
    public class English
    {
        [XmlElement(ElementName = "UI")]
        public UI UI { get; set; }
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Dutch")]
    public class Dutch
    {
        [XmlElement(ElementName = "UI")]
        public UI UI { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "German")]
    public class German
    {
        [XmlElement(ElementName = "UI")]
        public UI UI { get; set; }
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

    [XmlRoot(ElementName = "XML")]
    public class XML
    {
        [XmlElement(ElementName = "Languages")]
        public Languages Languages { get; set; }
    }

}

public class XmlManager : MonoBehaviour
{
    public static XmlManager Instance { get; private set; }
    public enum Language { French = 0, English, Dutch, German };
    public Language languageSelected = Language.English;
    Xml2CSharp.XML XML;

    public void Start()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    public Xml2CSharp.UI GetUI()
    {
        switch (languageSelected)
        {
            case Language.French:
                return XML.Languages.French.UI;
            case Language.English:
                return XML.Languages.English.UI;
            case Language.Dutch:
                return XML.Languages.Dutch.UI;
            case Language.German:
                return XML.Languages.German.UI;
        }
        return null;
    }

    public Xml2CSharp.Menu GetMenu()
    {
        return GetUI().Menu;
    }
}