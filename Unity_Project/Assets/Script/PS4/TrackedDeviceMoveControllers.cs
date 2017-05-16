using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_PS4
using UnityEngine.PS4;
using UnityEngine.PS4.VR;
#endif

public class TrackedDeviceMoveControllers : MonoBehaviour {
	public static TrackedDeviceMoveControllers Instance;

	public MoveController primaryMoveController;
	public MoveController secondaryMoveController;

    public Renderer[] illuminatedComponents;

    public Transform targetLeft;
    public Transform targetRight;

	public Vector3 TargetBias;

    [Range( 0.0f, 5.0f)]
    public float IkIntensity = 1.5f;

	private Transform primaryController;
	private Transform secondaryController;

#if UNITY_PS4
    private int m_primaryHandle = -1;
	private int m_secondaryHandle = -1;

	private Vector3 primaryPosition = Vector3.zero;
	private Quaternion primaryOrientation = Quaternion.identity;

	private Vector3 secondaryPosition = Vector3.zero;
	private Quaternion secondaryOrientation = Quaternion.identity;

	private Vector3 primaryPositionOriginPos = Vector3.zero;
	private Vector3 secondaryPositionOriginPos = Vector3.zero;

    public Vector3 targetLeftOriginPos;
    public Vector3 targetRightOriginPos;

    IEnumerator Start()
	{
		if (Instance == null) 
		{
			Instance = this;
		
			primaryController = primaryMoveController.transform;
			secondaryController = secondaryMoveController.transform;

			if (!primaryController || !secondaryController || !primaryController.gameObject.activeSelf || !secondaryController.gameObject.activeSelf) {
				Debug.LogWarning ("A controller is either null or inactive!");
				this.enabled = false;
			}

			// Keep waiting until we have a VR Device available
			while (!VRDevice.isPresent)
				yield return new WaitForSeconds (1.0f);

			// Make sure the device we now have is PlayStation VR
			#if UNITY_5_3
			if (VRSettings.loadedDevice != VRDeviceType.PlayStationVR)
			#elif UNITY_5_4_OR_NEWER
			if (VRSettings.loadedDeviceName != VRDeviceNames.PlayStationVR)
			#endif
			{
				Debug.LogWarning ("Tracking only works for PS4!");
				this.enabled = false;
			} else {
				ResetControllerTracking ();
			}

			if (targetLeft != null && targetRight != null) 
			{
				targetLeftOriginPos = targetLeft.localPosition;
				targetRightOriginPos = targetRight.localPosition;

                SceneManager.sceneLoaded += delegate {
                    targetLeft.localPosition = targetLeftOriginPos;
                    targetRight.localPosition = targetRightOriginPos;
                };
			}
		} 
		else if (Instance != this) 
		{
			Destroy(gameObject);
		}
    }

	void Update()
	{
#if UNITY_5_3
        if (VRDevice.isPresent && VRSettings.loadedDevice == VRDeviceType.PlayStationVR)
#elif UNITY_5_4_OR_NEWER
        if (VRDevice.isPresent && VRSettings.loadedDeviceName == VRDeviceNames.PlayStationVR)
#endif
		{
			// Reset the controller using the 'Start' button
			if(Input.GetKeyDown(KeyCode.JoystickButton7))
			{
				ResetControllerTracking();
			}

			// Perform tracking for the primary controller, if we've got a handle
			if(m_primaryHandle >= 0)
			{
				if( Tracker.GetTrackedDevicePosition(m_primaryHandle, out primaryPosition) == PlayStationVRResult.Ok )
                	primaryController.localPosition = primaryPosition;
                 

                if (Tracker.GetTrackedDeviceOrientation(m_primaryHandle, out primaryOrientation) == PlayStationVRResult.Ok)
					primaryController.localRotation = primaryOrientation;

				if(targetLeft != null && PlayerInputs.Instance.m_inGame)
					targetLeft.transform.localPosition = (targetLeftOriginPos - (primaryPositionOriginPos - primaryController.localPosition)) * IkIntensity + TargetBias;
			}

			// Perform tracking for the secondary controller, if we've got a handle
			if(secondaryController && m_secondaryHandle >= 0)
			{
                if (Tracker.GetTrackedDevicePosition(m_secondaryHandle, out secondaryPosition) == PlayStationVRResult.Ok)
					secondaryController.localPosition = secondaryPosition;

                if (Tracker.GetTrackedDeviceOrientation(m_secondaryHandle, out secondaryOrientation) == PlayStationVRResult.Ok)
					secondaryController.localRotation = secondaryOrientation;

				if (targetRight != null && PlayerInputs.Instance.m_inGame) 
				{
					Vector3 dir = (secondaryPositionOriginPos - secondaryController.localPosition);
					targetRight.transform.localPosition = (targetRightOriginPos - new Vector3(-dir.x, dir.y, dir.z)) * IkIntensity + TargetBias;
				}
			}
		}
	}

	// Unregister and re-register the controllers to reset them
	public void ResetControllerTracking()
	{
		UnregisterMoveControllers();
		StartCoroutine(RegisterMoveControllers());
	}

	// Register a Move device to track
	IEnumerator RegisterMoveControllers()
	{
		int [] primaryHandles = new int[1];
		int [] secondaryHandles = new int[1];
		PS4Input.MoveGetUsersMoveHandles(1, primaryHandles, secondaryHandles);
		m_primaryHandle = primaryHandles[0];
		m_secondaryHandle = secondaryHandles[0];

		Tracker.RegisterTrackedDevice(PlayStationVRTrackedDevice.DeviceMove, m_primaryHandle, PlayStationVRTrackingType.Absolute, PlayStationVRTrackerUsage.OptimizedForHmdUser);

        if (secondaryController)
            Tracker.RegisterTrackedDevice(PlayStationVRTrackedDevice.DeviceMove, m_secondaryHandle, PlayStationVRTrackingType.Absolute, PlayStationVRTrackerUsage.OptimizedForHmdUser);

        // Get the tracking, and wait for it to start
        PlayStationVRTrackingStatus trackingStatusPrimary = new PlayStationVRTrackingStatus();
        PlayStationVRTrackingStatus trackingStatusSecondary = new PlayStationVRTrackingStatus();

        // Force the secondary controller to something if it doesn't need to exist
        if (!secondaryController)
            trackingStatusSecondary = PlayStationVRTrackingStatus.NotTracking;

        while (trackingStatusPrimary == PlayStationVRTrackingStatus.NotStarted && trackingStatusSecondary == PlayStationVRTrackingStatus.NotStarted)
        {
            Tracker.GetTrackedDeviceStatus(m_primaryHandle, out trackingStatusPrimary);

            if (secondaryController)
                Tracker.GetTrackedDeviceStatus(m_secondaryHandle, out trackingStatusSecondary);

            yield return null;
        }

        PlayStationVRTrackingColor trackedColor;
        Tracker.GetTrackedDeviceLedColor(m_primaryHandle, out trackedColor);
        //illuminatedComponents[0].material.color = GetUnityColor(trackedColor);

        if (secondaryController)
        {
            Tracker.GetTrackedDeviceLedColor(m_secondaryHandle, out trackedColor);
            //illuminatedComponents[1].material.color = GetUnityColor(trackedColor);
        }

        // check target's origin position
        
        if( m_primaryHandle >= 0 && Tracker.GetTrackedDevicePosition(m_primaryHandle, out primaryPosition) == PlayStationVRResult.Ok )
            primaryPositionOriginPos = primaryController.localPosition = primaryPosition;

        if (secondaryController && m_secondaryHandle >= 0 && Tracker.GetTrackedDevicePosition(m_secondaryHandle, out secondaryPosition) == PlayStationVRResult.Ok)
            secondaryPositionOriginPos = secondaryController.localPosition = secondaryPosition;
    }

	// Remove the registered devices from tracking and reset the transform
	void UnregisterMoveControllers()
	{
		int[] primaryHandles = new int[1];
		int[] secondaryHandles = new int[1];
		PS4Input.MoveGetUsersMoveHandles(1, primaryHandles, secondaryHandles);
		m_primaryHandle = -1;
		m_secondaryHandle = -1;

		Tracker.UnregisterTrackedDevice(primaryHandles[0]);
		primaryController.localPosition = Vector3.zero;
		primaryController.localRotation = Quaternion.identity;

		if(secondaryController)
		{
			Tracker.UnregisterTrackedDevice(secondaryHandles[0]);
			secondaryController.localPosition = Vector3.zero;
			secondaryController.localRotation = Quaternion.identity;
		}
	}

    Color GetUnityColor(PlayStationVRTrackingColor trackingColor)
    {
        switch (trackingColor)
        {
            case PlayStationVRTrackingColor.Blue:
                return Color.blue;
            case PlayStationVRTrackingColor.Green:
                return Color.green;
            case PlayStationVRTrackingColor.Magenta:
                return Color.magenta;
            case PlayStationVRTrackingColor.Red:
                return Color.red;
            case PlayStationVRTrackingColor.Yellow:
                return Color.yellow;
            default:
                return Color.black;
        }
    }
#elif UNITY_5_4_OR_NEWER
	void Start()
	{
		if (Instance == null)
		{
			primaryController = primaryMoveController.transform;
			secondaryController = secondaryMoveController.transform;
			Instance = this;
		}
		else if (Instance != this)
			Destroy(gameObject);

	}
#endif
}
