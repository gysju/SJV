using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ZAManager : MonoBehaviour
{
    public BaseMecha m_player;
    public Vector3 m_playerStartPosition;
    public Vector3 m_playerStartRotation;
    public EnemiesManager m_enemiesManager;

    void Start()
    {
        if (!m_enemiesManager)
            m_enemiesManager = GetComponentInChildren<EnemiesManager>();
    }

    void FindPlayer()
    {
        m_player = FindObjectOfType<BaseMecha>();
        m_player.transform.position = m_playerStartPosition;
        m_player.transform.rotation = Quaternion.Euler(m_playerStartRotation);
        m_enemiesManager.StartWaves();
        m_player.m_bunker.DeactivateBunkerMode();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }

    void Update()
    {
        if (!m_player)
        {
            FindPlayer();
        }
    }
}
