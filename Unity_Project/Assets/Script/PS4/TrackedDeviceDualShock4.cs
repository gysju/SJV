using UnityEngine;
using UnityEngine.VR;
using System.Collections;
#if UNITY_PS4
using UnityEngine.PS4;
using UnityEngine.PS4.VR;
#endif

public class TrackedDeviceDualShock4 : MonoBehaviour {
	public Transform controller;
    public Renderer illuminatedComponent;
#if UNITY_PS4
	private int m_handle = -1;
	private Vector3 position = Vector3.zero;
	private Quaternion orientation = Quaternion.identity;
	
	IEnumerator Start()
	{
		if(!controller || !controller.gameObject.activeSelf)
		{
			Debug.LogWarning("The controller gameObject is either null or inactive!");
			this.enabled = false;
		}

		// Keep waiting until we have a VR Device available
		while(!VRDevice.isPresent)
			yield return new WaitForSeconds(1.0f);

		// Make sure the device we now have is PlayStation VR
#if UNITY_5_3
        if(VRSettings.loadedDevice != VRDeviceType.PlayStationVR)
#elif UNITY_5_4_OR_NEWER
        if (VRSettings.loadedDeviceName != VRDeviceNames.PlayStationVR)
#endif
		{
			Debug.LogWarning("Tracking only works for PS4!");
			this.enabled = false;
		}
		else
		{
			StartCoroutine(RegisterPadController());
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
			// Reset the controller using the 'Options' button
			if(Input.GetKeyDown(KeyCode.JoystickButton7))
			{
				ResetControllerTracking();
			}

			// Perform tracking as long as we've got a handle
			if(m_handle >= 0)
			{
				if( Tracker.GetTrackedDevicePosition(m_handle, out position) == PlayStationVRResult.Ok )
					controller.transform.localPosition = position;

                if (Tracker.GetTrackedDeviceOrientation(m_handle, out orientation) == PlayStationVRResult.Ok)
					controller.transform.localRotation = orientation;
			}
		}
	}

	// Unregister and re-register the controller to reset it
	public void ResetControllerTracking()
	{
		UnregisterController();
		StartCoroutine(RegisterPadController());
	}

	IEnumerator RegisterPadController()
	{
        // Register a wireless controller to track
        int[] handles = new int[1];
		PS4Input.PadGetUsersPadHandles(1, handles);
		m_handle = handles[0];
        Tracker.RegisterTrackedDevice(PlayStationVRTrackedDevice.DeviceDualShock4, m_handle, PlayStationVRTrackingType.Absolute, PlayStationVRTrackerUsage.OptimizedForHmdUser);

        // Get the tracking, and wait for it to start
        PlayStationVRTrackingStatus trackingStatus = new PlayStationVRTrackingStatus();
        while(trackingStatus == PlayStationVRTrackingStatus.NotStarted)
        {
            Tracker.GetTrackedDeviceStatus(m_handle, out trackingStatus);
            yield return null;
        }

        // Get the color of the now tracked device
        PlayStationVRTrackingColor trackedColor;
        Tracker.GetTrackedDeviceLedColor(m_handle, out trackedColor);

        // Apply the color to the relevant mesh component
        illuminatedComponent.material.color = GetUnityColor(trackedColor);
    }

	// Remove the registered device from tracking and reset the transform
	void UnregisterController()
	{
		int[] handles = new int[1];
		PS4Input.PadGetUsersPadHandles(1, handles);
		m_handle = -1;
		Tracker.UnregisterTrackedDevice(handles[0]);
		
		controller.transform.localPosition = Vector3.zero;
		controller.transform.localRotation = Quaternion.identity;
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
#endif
}
