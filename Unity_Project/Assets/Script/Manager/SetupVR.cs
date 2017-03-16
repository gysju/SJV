using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using System.Collections;
using UnityEngine.SceneManagement;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class SetupVR : MonoBehaviour
{
    bool setup = false;
    [Range(0f, 5f)]
    public float m_timeToMainMenu = 5f;
    public Text text;
    void Start()
    {
#if UNITY_PS4
        Utility.onSystemServiceEvent += OnSystemServiceEvent;
#endif

        if (VRSettings.enabled == false)
        {

        }
        else
        {
            VRManager.instance.BeginVRSetup();
        }
    }
    
    IEnumerator Setup()
    {
        yield return new WaitForSeconds(m_timeToMainMenu);
        ContinueToMainMenu();
    }

    public void ContinueToMainMenu()
    {
       SceneManager.LoadSceneAsync(1);
    }

#if UNITY_PS4
    void OnSystemServiceEvent(Utility.sceSystemServiceEventType eventType)
    {
        if (eventType == Utility.sceSystemServiceEventType.RESET_VR_POSITION && setup == true)
        {
            ContinueToMainMenu();
            //StartCoroutine(Setup());
        }
    }
#endif

    void Update()
    {
        if (!setup)
        {
            if (VRSettings.enabled == true)
            {
                setup = true;
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                VRManager.instance.SetupHMDDevice();
                text.text = "Installez-vous confortablement pour jouer et laissez appuyé le bouton Start";
            }
        }
    }
}
