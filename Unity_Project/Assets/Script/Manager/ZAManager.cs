using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ZAManager : MonoBehaviour
{
    public BaseMecha m_player;
    public Vector3 m_playerStartPosition;
    public Vector3 m_playerStartRotation;

    public bool m_playerRotation = false;
    public bool m_playerMovement = false;
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
        m_player.m_inputs.m_torsoConnected = m_playerRotation;
        m_player.m_inputs.m_legsConnected = m_playerMovement;
        m_player.m_inputs.m_weaponsConnected = true;
        m_player.m_bunker.DeactivateBunkerMode();
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

    void Update()
    {
        if (!m_player)
        {
            FindPlayer();
        }
    }
}
