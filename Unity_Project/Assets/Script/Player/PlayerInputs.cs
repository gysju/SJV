using UnityEngine;
using System.Collections;

public class PlayerInputs : MonoBehaviour
{
    protected Camera m_mainCamera;

    public BaseMecha m_mecha;

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
#endif

#if UNITY_PS4
    private void PSMoveStart()
    {
        m_baseOffset = Vector3.zero;
        m_leftController = trackedDeviceMoveControllers.primaryController.GetComponent<MoveController>();
        m_rightController = trackedDeviceMoveControllers.secondaryController.GetComponent<MoveController>();
		lastMovement = Vector3.zero;
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
