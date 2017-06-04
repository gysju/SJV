using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextZoneHelmetHUD : MonoBehaviour
{
    Text m_text;

    public Color m_neutralColor = Color.white;
    public Color m_positiveColor = Color.green;
    public Color m_dangerColor = Color.red;

    public string m_deployementText = "Arriving";
    public string m_extractionText = "Coming back to ship";
    public string m_victoryText = "Mission Accomplished";
    public string m_lowHealth = "[!] Integrity critical [!]";
    public string m_defeatText = "Mission Failed";

    void Start()
    {
        m_text = GetComponent<Text>();
        XmlManager.onChangedLanguage += setLanguage;
        setLanguage();
    }

    void setLanguage()
    {
        m_deployementText = XmlManager.Instance.GetGameplay().HUD.Deployement.Text;
        m_extractionText = XmlManager.Instance.GetGameplay().HUD.Extraction.Text;
        m_victoryText = XmlManager.Instance.GetGameplay().HUD.Victory.Text;
        m_lowHealth = XmlManager.Instance.GetGameplay().HUD.LowHealth.Text;
        m_defeatText = XmlManager.Instance.GetGameplay().HUD.Defeat.Text;
    }

    public void Nothing()
    {
        m_text.text = "";
    }

    public void Deployement()
    {
        m_text.text = m_deployementText;
        m_text.color = m_neutralColor;
    }

    public void Extraction()
    {
        m_text.text = m_extractionText;
        m_text.color = m_neutralColor;
    }

    public void Victory()
    {
        m_text.text = m_victoryText;
        m_text.color = m_positiveColor;
    }

    public void LowHealth()
    {
        m_text.text = m_lowHealth;
        m_text.color = m_dangerColor;
    }

    public void Defeat()
    {
        m_text.text = m_defeatText;
        m_text.color = m_dangerColor;
    }
}
