using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ZAManager : ScenePlayerManager
{
    public static ZAManager Instance = null;

    public EnemiesManager m_enemiesManager;

    public bool m_instaStart = true;
    
    public float m_timeBeforeEndZA = 5f;

    [Header("Debug")]
    public bool m_testMode = false;

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
        if(m_instaStart && !m_testMode) m_enemiesManager.StartWaves();
    }

    protected override IEnumerator PlayerArrival()
    {
        m_player.m_interface.ShowHelmetHUD();
        m_player.m_interface.m_textHelmet.Deployement();
        yield return new WaitForSeconds(2f);
        BunkerOff();
        m_player.m_interface.m_textHelmet.Nothing();
    }

    public void MissionAccomplished()
    {
        m_player.m_interface.m_textHelmet.Victory();
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
