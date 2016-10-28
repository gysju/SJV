using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        BattleState_OnGoing,
        BattleState_Victory,
        BattleState_Defeat
    }

    public BattleState battleState = BattleState.BattleState_OnGoing;

    public Unit m_allyBaseCenter;
    public Unit m_enemyBaseCenter;

    public GameObject m_victoryText;
    public GameObject m_defeatText;

    void Start ()
    {
        m_victoryText.SetActive(false);
        m_defeatText.SetActive(false);

    }

    private bool VictoryCondition()
    {
        return (m_enemyBaseCenter.IsDestroyed());
    }

    private bool DefeatCondition()
    {
        return (m_allyBaseCenter.IsDestroyed());
    }

    private void VictoryEvent()
    {
        battleState = BattleState.BattleState_Victory;
        m_victoryText.SetActive(true);
        StartCoroutine(BackTimer());
    }

    private void DefeatEvent()
    {
        battleState = BattleState.BattleState_Defeat;
        m_defeatText.SetActive(true);
        StartCoroutine(BackTimer());
    }

    IEnumerator BackTimer()
    {
        yield return new WaitForSeconds(2f);
        BackToMainMenu();
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("Intro");
    }

    void Update ()
    {
        switch (battleState)
        {
            case BattleState.BattleState_OnGoing:
                if (VictoryCondition())
                    VictoryEvent();
                else if (DefeatCondition())
                    DefeatEvent();

                break;
            case BattleState.BattleState_Victory:
                break;
            case BattleState.BattleState_Defeat:
                break;
            default:
                break;
        }
    }
}
