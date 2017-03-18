using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ZAManager : MonoBehaviour
{
    protected BaseMecha m_player;
    public Vector3 m_playerStartPosition;
    public Vector3 m_playerStartRotation;

    void Awake()
    {
        m_player = FindObjectOfType<BaseMecha>();
        m_player.transform.position = m_playerStartPosition;
        m_player.transform.rotation = Quaternion.Euler(m_playerStartRotation);

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
