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
                text.text = "Installez-vous confortablement pour jouer et laissez le bouton Start appuyé";
            }
        }
    }
}
