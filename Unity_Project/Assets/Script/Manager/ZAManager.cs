using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ZAManager : ScenePlayerManager
{
    public static ZAManager Instance = null;

    public EnemiesManager m_enemiesManager;

    public static ZAManager instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<ZAManager>();
            }

            return Instance;
        }
    }

    void Start()
    {
        Instance = this;
        if (!m_enemiesManager) m_enemiesManager = GetComponentInChildren<EnemiesManager>();
    }

    protected override void FindPlayer()
    {
        base.FindPlayer();
        m_player.ReadyToAction();
        m_enemiesManager.StartWaves();
    }

    public void MissionAccomplished()
    {
        m_player.m_bunker.ActivateBunkerMode();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }

    protected override void Update()
    {
        base.Update();
    }
}
