using UnityEngine;
using System.Collections;

#if UNITY_PS4
using UnityEngine.PS4;
#endif
public class PlayerInputs : MonoBehaviour
{
	public static PlayerInputs Instance = null;
    protected Camera m_mainCamera;

    public BaseMecha m_mecha;

    public bool m_inGame = false;

    protected Vector3 m_leftWeaponDefaultPosition;
    protected Vector3 m_rightWeaponDefaultPosition;
    protected Vector3 m_baseOffset;
    protected float m_sensitivity = 0.001f;

    public bool m_weaponsConnected = false;

    public MechaTorso m_torso;
    public bool m_torsoConnected = false;

    public float m_maxHorinzontalHeadAngle = 10f;
    public float m_maxVerticalHeadAngle = 75f;

    public MechaLegs m_legs;
    public bool m_legsConnected = false;

    public LaserPointingSystem m_leftRay;
    public LaserPointingSystem m_rightRay;

    public GameObject teleportIndication;

    private bool m_leftMovePriority = false;
    private bool m_rightMovePriority = false;

#if UNITY_PS4
    [Header("PSMove Related")]

    public TrackedDeviceMoveControllers trackedDeviceMoveControllers;
	public float MinimumAngleToRotate = 15.0f;

    private MoveController m_leftController;
    private MoveController m_rightController;

    private void PSMoveStart()
    {
        trackedDeviceMoveControllers = GetComponentInChildren<TrackedDeviceMoveControllers>();
        m_baseOffset = Vector3.zero;
		m_leftController = trackedDeviceMoveControllers.primaryMoveController;
		m_rightController = trackedDeviceMoveControllers.secondaryMoveController;
        m_leftRay = m_leftController.GetComponent<LaserPointingSystem>();
        m_rightRay = m_rightController.GetComponent<LaserPointingSystem>();

        teleportIndication.SetActive(false);
    }
#endif

    void Start ()
	{
		if (Instance == null) 
		{
			Instance = this;

			m_mainCamera = Camera.main;
			if (!m_mecha) m_mecha = GetComponentInParent<BaseMecha>();
			if (!m_torso) m_torso = m_mecha.m_torso;
            if (!m_legs) m_legs = m_mecha.m_legs;
			#if UNITY_PS4
			PSMoveStart();
			#endif
		}
		else if (Instance != this)
			Destroy(gameObject);
    }

    protected void CheckPilotHead()
    {
        float horizontalAnglePrevision = m_mainCamera.transform.localRotation.eulerAngles.y;
        horizontalAnglePrevision = (horizontalAnglePrevision > 180) ? horizontalAnglePrevision - 360 : horizontalAnglePrevision;

        if (horizontalAnglePrevision > m_maxHorinzontalHeadAngle)
        {
            m_torso.RotateRight();
        }
        else if (horizontalAnglePrevision < -(m_maxHorinzontalHeadAngle))
        {
            m_torso.RotateLeft();
        }
    }

    void RotatePilotHead(float horizontalAngle, float verticalAngle)
    {
        float horizontalAnglePrevision = m_mainCamera.transform.localRotation.eulerAngles.y;
        horizontalAnglePrevision = (horizontalAnglePrevision > 180) ? horizontalAnglePrevision - 360 : horizontalAnglePrevision;
        horizontalAnglePrevision += horizontalAngle;
        float finalHorizontalAngle = horizontalAngle;
        float toTransforToTorso = 0f;

        if (m_torsoConnected)
        {
            if (horizontalAnglePrevision > m_maxHorinzontalHeadAngle)
            {
                toTransforToTorso = (horizontalAnglePrevision - m_maxHorinzontalHeadAngle);
                finalHorizontalAngle -= toTransforToTorso;
                m_mecha.RotateMechaHorizontaly(toTransforToTorso);
            }
            else if (horizontalAnglePrevision < -(m_maxHorinzontalHeadAngle))
            {
                toTransforToTorso = (horizontalAnglePrevision + m_maxHorinzontalHeadAngle);
                finalHorizontalAngle -= toTransforToTorso;
                m_mecha.RotateMechaHorizontaly(toTransforToTorso);
            }
        }

        float verticalAnglePrevision = m_mainCamera.transform.localRotation.eulerAngles.x;
        verticalAnglePrevision = (verticalAnglePrevision > 180) ? verticalAnglePrevision - 360 : verticalAnglePrevision;
        verticalAnglePrevision += verticalAngle;
        float finalVerticalAngle = verticalAngle;
        float rest = 0f;

        if (verticalAnglePrevision > m_maxVerticalHeadAngle)
        {
            rest = (verticalAnglePrevision - m_maxVerticalHeadAngle);
            finalVerticalAngle -= rest;
        }
        else if (verticalAnglePrevision < -(m_maxVerticalHeadAngle))
        {
            rest = (verticalAnglePrevision + m_maxVerticalHeadAngle);
            finalVerticalAngle -= rest;
        }

        Quaternion currentRotation = m_mainCamera.transform.rotation;
        Quaternion horizontalRotation = Quaternion.AngleAxis(finalHorizontalAngle, Vector3.up);
        Quaternion verticalRotation = Quaternion.AngleAxis(finalVerticalAngle, Vector3.right);
        m_mainCamera.transform.rotation = horizontalRotation * currentRotation * verticalRotation;
    }

    void PointDestinationLeft()
    {
        m_leftMovePriority = true;
        if (m_leftRay.raycastHit && m_legs.CheckDestination(m_leftRay.hit))
        {
            m_leftRay.setLineColor(Color.green);
            //display effect at the destination
            if (teleportIndication != null)
            {
                teleportIndication.SetActive(true);
                teleportIndication.transform.position = m_leftRay.hit.point;
            }
        }
        else
        {
            m_leftRay.setLineColor(Color.red);
            if (teleportIndication != null)
            {
                teleportIndication.SetActive(false);
            }
        }
    }

    void PointDestinationRight()
    {
        m_rightMovePriority = true;
        if (m_rightRay.raycastHit && m_legs.CheckDestination(m_rightRay.hit))
        {
            m_rightRay.setLineColor(Color.green);
            //display effect at the destination
            if (teleportIndication != null)
            {
                teleportIndication.SetActive(true);
                teleportIndication.transform.position = m_rightRay.hit.point;
            }
        }
        else
        {
            m_rightRay.setLineColor(Color.red);
            //Hide effect
            if (teleportIndication != null)
            {
                teleportIndication.SetActive(false);
            }
        }
    }

    void ConfirmDestination()
    {
        m_leftMovePriority = false;
        m_rightMovePriority = false;
        m_legs.ConfirmTeleport();
        m_leftRay.setLineColor(Color.white);
        m_rightRay.setLineColor(Color.white);
        teleportIndication.SetActive(false);
    }

    public void CameraDepth()
    {
        m_mainCamera.clearFlags = CameraClearFlags.Depth;
    }

    public void CameraSky()
    {
        m_mainCamera.clearFlags = CameraClearFlags.Skybox;
    }

#if UNITY_PS4
    #region PSMoves

    void PSMoveLeftWeaponControl()
    {
        m_mecha.MoveLeftWeapon(m_leftWeaponDefaultPosition + ((m_leftController.transform.position - m_baseOffset) * m_sensitivity));
        if (m_leftController.lookAtHit != Vector3.zero && m_inGame)
            m_mecha.AimLeftWeaponTo(m_leftController.lookAtHit);
        else
        {
            m_mecha.AimLeftWeaponTo(transform.forward);
            trackedDeviceMoveControllers.targetLeft.position = trackedDeviceMoveControllers.targetLeftOriginPos;
        }
    }

    void PSMoveRightWeaponControl()
    {
        m_mecha.MoveRightWeapon(m_rightWeaponDefaultPosition + ((m_rightController.transform.position - m_baseOffset) * m_sensitivity));
        if (m_rightController.lookAtHit != Vector3.zero && m_inGame)
            m_mecha.AimRightWeaponTo(m_rightController.lookAtHit);
        else
        {
            m_mecha.AimRightWeaponTo(transform.forward);
            trackedDeviceMoveControllers.targetRight.position = trackedDeviceMoveControllers.targetRightOriginPos;
        }
    }

    void PSMoveInputs()
    {
        if (m_inGame)
        {
            if (m_weaponsConnected)
            {
                PSMoveLeftWeaponControl();
                PSMoveRightWeaponControl();
                if (m_leftController.GetButtonDown(MoveController.MoveButton.MoveButton_Trigger)) m_mecha.LeftArmWeaponTriggered();
                if (m_leftController.GetButtonUp(MoveController.MoveButton.MoveButton_Trigger)) m_mecha.LeftArmWeaponTriggerReleased();

                if (m_rightController.GetButtonDown(MoveController.MoveButton.MoveButton_Trigger)) m_mecha.RightArmWeaponTriggered();
                if (m_rightController.GetButtonUp(MoveController.MoveButton.MoveButton_Trigger)) m_mecha.RightArmWeaponTriggerReleased();
            }
			if (m_legsConnected)
			{
				PSMoveMovementInputs();
			}
            if (m_leftController.GetButtonDown(MoveController.MoveButton.MoveButton_Start) || m_rightController.GetButtonDown(MoveController.MoveButton.MoveButton_Start))
            {
                PlayerInterface.Instance.StartPause();
            }
        }
    }

    void PSMoveMovementInputs()
    {
        // Switch Move System when you presse circle button

        if (m_leftController.GetButtonDown(MoveController.MoveButton.MoveButton_Circle)) m_mecha.m_legs.SwitchMoveSystem();
        if (m_rightController.GetButtonDown(MoveController.MoveButton.MoveButton_Circle)) m_mecha.m_legs.SwitchMoveSystem();


        if (!m_rightMovePriority && m_leftController.GetButton(MoveController.MoveButton.MoveButton_Move))
        {
            PointDestinationLeft();
        }
        if (m_leftMovePriority  && m_leftController.GetButtonUp(MoveController.MoveButton.MoveButton_Move))
        {
            ConfirmDestination();
        }

        if (!m_leftMovePriority && m_rightController.GetButton(MoveController.MoveButton.MoveButton_Move))
        {
            PointDestinationRight();
        }
        if (m_rightMovePriority && m_rightController.GetButtonUp(MoveController.MoveButton.MoveButton_Move))
        {
            ConfirmDestination();
        }
    }

    Vector3 PSMoveVirtualJoysticksConvertion(int index)
    {
        Vector3 movementDirection = Vector3.zero;

        float x = -PS4Input.GetLastMoveAcceleration(0, index).x;
        float y = PS4Input.GetLastMoveAcceleration(0, index).y;

        const float NEUTRAL_Z = -0.3f;
        const float NEUTRAL_X = 0f;

        const float MAX_FORWARD = 0.8f;
        const float MIN_FORWARD = 0.2f;
        const float MAX_BACKWARD = -0.8f;
        const float MIN_BACKWARD = -0.2f;
        const float MAX_LEFT = -0.6f;
        const float MIN_LEFT = -0.2f;
        const float MAX_RIGHT = 0.6f;
        const float MIN_RIGHT = 0.2f;

        float zdir = 0;
        if (y > NEUTRAL_Z) //avant
            zdir = (Mathf.InverseLerp(MIN_FORWARD, MAX_FORWARD, y));
        if (y < NEUTRAL_Z) //arrière
            zdir = -(Mathf.InverseLerp(MIN_BACKWARD, MAX_BACKWARD, y));

        float xdir = 0;
        if (x < NEUTRAL_X) //gauche
            xdir = -(Mathf.InverseLerp(MIN_LEFT, MAX_LEFT, x));
        if (x > NEUTRAL_X) //droite
            xdir = (Mathf.InverseLerp(MIN_RIGHT, MAX_RIGHT, x));

        movementDirection += new Vector3(xdir, 0f, zdir);

        return movementDirection;
    }
    #endregion

#endif

    #region Mouse, Keyboard
    void MouseAim()
    {
        RotatePilotHead(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //RaycastHit aimTarget;
        //if(Physics.Raycast(m_mainCamera.transform.position, m_mainCamera.transform.forward, out aimTarget))
        //{
        //    AimLeftWeaponTo(aimTarget.point);
        //    AimRightWeaponTo(aimTarget.point);
        //}
        //else
        if (m_weaponsConnected)
        {
            m_mecha.AimLeftWeaponTo(m_mainCamera.transform.position + m_mainCamera.transform.forward * 100);
            m_mecha.AimRightWeaponTo(m_mainCamera.transform.position + m_mainCamera.transform.forward * 100);
        }
    }

    void MouseShootInputs()
    {
        if (m_weaponsConnected)
        {
            if (Input.GetMouseButtonDown(0)) m_mecha.LeftArmWeaponTriggered();
            if (Input.GetMouseButtonUp(0)) m_mecha.LeftArmWeaponTriggerReleased();

            if (Input.GetMouseButtonDown(1)) m_mecha.RightArmWeaponTriggered();
            if (Input.GetMouseButtonUp(1)) m_mecha.RightArmWeaponTriggerReleased();
        }
    }

    void KeyboardMovements()
    {
        Vector3 movementDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.Z)) movementDirection.z += 1f;
        if (Input.GetKey(KeyCode.S)) movementDirection.z -= 1f;
        if (Input.GetKey(KeyCode.D)) movementDirection.x += 1f;
        if (Input.GetKey(KeyCode.Q)) movementDirection.x -= 1f;

        if (movementDirection != Vector3.zero) m_legs.MoveTo(movementDirection);

        if (Input.GetMouseButton(2))
        {
            PointDestinationLeft();
        }
        if (Input.GetMouseButtonUp(2))
        {
            ConfirmDestination();
        }
    }

    void TeleportMouse()
    {
        if (Input.GetKeyDown(KeyCode.Space)) m_mecha.m_legs.SwitchMoveSystem();

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            PointDestinationLeft();
        }
        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            ConfirmDestination();
        }
    }

    void MouseKeyboardInputs()
    {
        if (m_inGame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            MouseAim();
            MouseShootInputs();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PlayerInterface.Instance.StartPause();
            }
            TeleportMouse();
            //KeyboardMovements();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    #endregion

    void InputsUpdate()
    {
#if UNITY_STANDALONE
        MouseKeyboardInputs();
#elif UNITY_PS4
        if(m_mecha.m_inputs.m_torsoConnected)
        {
            CheckPilotHead();
        }
		PSMoveInputs();
#endif

        //ControllerInputs();
    }

    void Update ()
	{
        if (!m_mecha.IsDestroyed()/* && CanvasManager.EState_Menu.EState_Menu_InGame == CanvasManager.Get.eState_Menu*/)
        {
            InputsUpdate();
        }
    }
}
