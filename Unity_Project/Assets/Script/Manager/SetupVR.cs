using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using System.Collections;
using UnityEngine.SceneManagement;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class SetupVR : MenuManager
{
    bool setup = false;

    [Header("UI")]
    public CanvasGroup m_setupBoard;
    public CanvasGroup m_resetCamBoard;

    void Start()
    {
#if UNITY_PS4
        Utility.onSystemServiceEvent += OnSystemServiceEvent;
#endif

        if (VRSettings.enabled == false)
        {
            SetupBoard();
        }
        else
        {
            VRManager.instance.BeginVRSetup();
            ResetCamBoard();
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

    #region Setup Scene
    public void SetupBoard()
    {
        ShowBoard(m_setupBoard);
    }

    public void ResetCamBoard()
    {
        HideBoard(m_setupBoard);
        ShowBoard(m_resetCamBoard);
    }
    #endregion

    protected override void Update()
    {
        base.Update();

        if (!setup)
        {
            if (VRSettings.enabled == true)
            {
                setup = true;
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                VRManager.instance.SetupHMDDevice();
                ResetCamBoard();
                ContinueToMainMenu();
            }
        }
    }
}
