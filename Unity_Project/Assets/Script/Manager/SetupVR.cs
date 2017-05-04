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
    public PlayerInterface m_interface;
    void Start()
    {
#if UNITY_PS4
        Utility.onSystemServiceEvent += OnSystemServiceEvent;
#endif

        if (VRSettings.enabled == false)
        {
            m_interface.SetupBoard();
        }
        else
        {
            VRManager.instance.BeginVRSetup();
            m_interface.ResetCamBoard();
        }
    }

    public void ContinueToMainMenu()
    {
       SceneManager.LoadSceneAsync(1);
       //m_interface.MainMenu();
    }

#if UNITY_PS4
    void OnSystemServiceEvent(Utility.sceSystemServiceEventType eventType)
    {
        if (eventType == Utility.sceSystemServiceEventType.RESET_VR_POSITION && setup == true)
        {
            ContinueToMainMenu();
            Utility.onSystemServiceEvent -= OnSystemServiceEvent;
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
                m_interface.ResetCamBoard();
                ContinueToMainMenu();
            }
        }
    }
}
