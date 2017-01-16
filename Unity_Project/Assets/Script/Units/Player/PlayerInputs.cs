using UnityEngine;
using System.Collections;

#if UNITY_PS4
using UnityEngine.PS4;
#endif
public class PlayerInputs : MonoBehaviour
{
    protected Camera m_mainCamera;

    public BaseMecha m_mecha;

    protected Vector3 m_leftWeaponDefaultPosition;
    protected Vector3 m_rightWeaponDefaultPosition;
    protected Vector3 m_baseOffset;
    protected float m_sensitivity = 0.001f;

    public MechaTorso m_torso;
    protected bool m_torsoConnected;
    
    public float m_maxHorinzontalHeadAngle = 10f;
    public float m_maxVerticalHeadAngle = 75f;

    //public MechaLegs m_legs;
    //protected bool m_legsConnected;

#if UNITY_PS4
    [Header("PSMove Related")]

    public TrackedDeviceMoveControllers trackedDeviceMoveControllers;
	public float MinimumAngleToRotate = 15.0f;

    private MoveController m_leftController;
    private MoveController m_rightController;
    
	private Vector3 m_lastMovement;
    
    private void PSMoveStart()
    {
        m_baseOffset = Vector3.zero;
        m_leftController = trackedDeviceMoveControllers.primaryController.GetComponent<MoveController>();
        m_rightController = trackedDeviceMoveControllers.secondaryController.GetComponent<MoveController>();
		m_lastMovement = Vector3.zero;
    }
#endif

    void Start ()
	{
        m_mainCamera = Camera.main;
        if (!m_mecha) m_mecha = GetComponentInParent<BaseMecha>();
        if (!m_torso) m_torso = m_mecha.m_torso;
        m_torsoConnected = m_torso;
#if UNITY_PS4
        PSMoveStart();
#endif
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

#if UNITY_PS4
    #region PSMoves

    void PSMoveLeftWeaponControl()
    {
        m_mecha.MoveLeftWeapon(m_leftWeaponDefaultPosition + ((m_leftController.transform.position - m_baseOffset) * m_sensitivity));
        if (m_leftController.lookAtHit != Vector3.zero)
            m_mecha.AimLeftWeaponTo(m_leftController.lookAtHit);
    }

    void PSMoveRightWeaponControl()
    {
        m_mecha.MoveRightWeapon(m_rightWeaponDefaultPosition + ((m_rightController.transform.position - m_baseOffset) * m_sensitivity));
        if (m_rightController.lookAtHit != Vector3.zero)
            m_mecha.AimRightWeaponTo(m_rightController.lookAtHit);
    }

    void PSMoveInputs()
    {
		if (m_leftController.GetButtonDown(MoveController.MoveButton.MoveButton_Trigger)) m_mecha.LeftArmWeaponTriggered();
		if (m_leftController.GetButtonUp(MoveController.MoveButton.MoveButton_Trigger)) m_mecha.LeftArmWeaponTriggerReleased();

        if (m_rightController.GetButtonDown(MoveController.MoveButton.MoveButton_Trigger)) m_mecha.RightArmWeaponTriggered();
		if (m_rightController.GetButtonUp(MoveController.MoveButton.MoveButton_Trigger)) m_mecha.RightArmWeaponTriggerReleased();
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
        {
            m_mecha.AimLeftWeaponTo(m_mainCamera.transform.position + m_mainCamera.transform.forward * 100);
            m_mecha.AimRightWeaponTo(m_mainCamera.transform.position + m_mainCamera.transform.forward * 100);
        }
    }

    void MouseShootInputs()
    {
        if (Input.GetMouseButtonDown(0)) m_mecha.LeftArmWeaponTriggered();
        if (Input.GetMouseButtonUp(0)) m_mecha.LeftArmWeaponTriggerReleased();

        if (Input.GetMouseButtonDown(1)) m_mecha.RightArmWeaponTriggered();
        if (Input.GetMouseButtonUp(1)) m_mecha.RightArmWeaponTriggerReleased();
    }

    //void KeyboardMovements()
    //{
    //    Vector3 movementDirection = Vector3.zero;
    //    if (Input.GetKey(KeyCode.Z)) movementDirection.z += 1f;
    //    if (Input.GetKey(KeyCode.S)) movementDirection.z -= 1f;
    //    if (Input.GetKey(KeyCode.D)) movementDirection.x += 1f;
    //    if (Input.GetKey(KeyCode.Q)) movementDirection.x -= 1f;

    //    if (movementDirection != Vector3.zero) MoveFromLocalRotation(movementDirection);
    //    else ResumePath();

    //    if (Input.GetMouseButton(2))
    //    {
    //        PointDestination(m_mainCamera.transform);
    //    }
    //    if (Input.GetMouseButtonUp(2))
    //    {
    //        ConfirmDestination();
    //    }
    //}

    void MouseKeyboardInputs()
    {
        MouseAim();
        MouseShootInputs();
        //KeyboardMovements();
    }

    #endregion

    void InputsUpdate()
    {
#if UNITY_STANDALONE
        MouseKeyboardInputs();
#elif UNITY_PS4
        if(m_torsoConnected)
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
