using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ZAManager : ScenePlayerManager
{
    public EnemiesManager m_enemiesManager;

    void Start()
    {
        if (!m_enemiesManager)
            m_enemiesManager = GetComponentInChildren<EnemiesManager>();
    }

    protected override void FindPlayer()
    {
        base.FindPlayer();
        m_player.m_inputs.m_weaponsConnected = true;
        m_enemiesManager.StartWaves();
    }

    public void MissionAccomplished()
    {
        m_player.m_bunker.ActivateBunkerMode();
    }

    public void BackToMainMenu()
    {
        m_player.m_interface.BackToMainMenu();
        //SceneManager.LoadSceneAsync(1);
    }
}
