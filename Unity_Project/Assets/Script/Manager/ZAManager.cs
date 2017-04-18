using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ZAManager : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
