using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePlayerManager : MonoBehaviour
{
    public BaseMecha m_player;
    public Vector3 m_playerStartPosition;
    public Vector3 m_playerStartRotation;

    public bool m_playerRotation = false;
    public bool m_playerMovement = false;

    protected virtual void FindPlayer()
    {
        m_player = FindObjectOfType<BaseMecha>();
        m_player.transform.position = m_playerStartPosition;
        m_player.transform.rotation = Quaternion.Euler(m_playerStartRotation);
        m_player.m_inputs.m_torsoConnected = m_playerRotation;
        m_player.m_inputs.m_legsConnected = m_playerMovement;
        m_player.m_bunker.DeactivateBunkerMode();
    }

    void Update()
    {
        if (!m_player)
        {
            FindPlayer();
        }
    }
}
