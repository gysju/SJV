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

    public bool m_backgroundCamera = false;

    protected virtual void FindPlayer()
    {
        m_player = BaseMecha.instance;

        if (m_backgroundCamera)
            m_player.m_inputs.CameraDepth();
        else
            m_player.m_inputs.CameraSky();

        m_player.m_torso.ResetTorso();
        m_player.transform.rotation = Quaternion.Euler(m_playerStartRotation);
        m_player.m_inputs.m_torsoConnected = m_playerRotation;
        m_player.m_inputs.m_legsConnected = m_playerMovement;

        m_player.m_legs.m_navmeshAgent.enabled = false;
        m_player.transform.position = m_playerStartPosition;
        m_player.m_legs.m_navmeshAgent.enabled = true;

        BunkerOff();
    }

    protected void BunkerOff()
    {
        m_player.m_bunker.DeactivateBunkerMode();
    }
    
    protected virtual void Update()
    {
        if (!m_player)
        {
            FindPlayer();
        }
    }
}
