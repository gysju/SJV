using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class Player : MobileGroundUnit
{
    private Camera m_mainCamera;
    public float m_maxHorinzontalHeadAngle = 10f;
    public float m_maxVerticalHeadAngle = 75f;

    //private List<SixenseInput.Controller> m_razerControllers;

    [Header("Player Related")]
    public GameObject m_torso;
    public GameObject m_legs;
    private Weapon m_leftWeapon;
    private Vector3 m_leftWeaponDefaultPosition;
    private Quaternion m_leftWeaponDefaultRotation;
    private Vector3 m_rightWeaponDefaultPosition;
    private Quaternion m_rightWeaponDefaultRotation;
    private Weapon m_rightWeapon;

    public GameObject m_pointer;
    private GameObject m_destinationPointer = null;
	private bool RazerAreConnected = false;

    #if UNITY_PS4
    [Header("PSMove Related")]

    public TrackedDeviceMoveControllers trackedDeviceMoveControllers;
    private MoveController m_leftController;
    private MoveController m_rightController;

    #else
        [Header("Razer Hydra Related")]
        SixenseInput.Controller m_leftController;
        SixenseInput.Controller m_rightController;
    #endif
    Vector3 m_baseOffset;
    float m_sensitivity = 0.001f;

    #region Initialisation
    #if UNITY_STANDALONE
    private void RazerStart()
    {
        m_baseOffset = Vector3.zero;
        m_leftController = SixenseInput.Controllers[0];
        m_rightController = SixenseInput.Controllers[1];

        m_baseOffset += m_leftController.Position;
        m_baseOffset += m_rightController.Position;
        m_baseOffset /= 2;
    }
    #endif
	#if UNITY_PS4
    private void PSMoveStart()
    {
        m_baseOffset = Vector3.zero;
        m_leftController = trackedDeviceMoveControllers.primaryController.GetComponent<MoveController>(); ;
        m_rightController = trackedDeviceMoveControllers.secondaryController.GetComponent<MoveController>(); ;
    }
	#endif
    protected override void Start()
    {
		base.Start();
        m_mainCamera = Camera.main;
        m_leftWeapon = m_weapons[0];
        m_leftWeaponDefaultPosition = m_leftWeapon.transform.localPosition;
        m_leftWeaponDefaultRotation = m_leftWeapon.transform.localRotation;
        m_rightWeapon = m_weapons[1];
        m_rightWeaponDefaultPosition = m_rightWeapon.transform.localPosition;
        m_rightWeaponDefaultRotation = m_rightWeapon.transform.localRotation;

        #if UNITY_STANDALONE

		if(SixenseInput.Controllers[0] != null)
		{
			RazerStart();
			RazerAreConnected = true;
		}
        #else
        PSMoveStart();
        #endif
    }
    #endregion

    protected override void Die()
    {
        m_destroyed = true;

        GetComponent<BoxCollider>().enabled = false;

        //for (int i = 0 ; i < transform.childCount ; i++)
        //{
        //    transform.GetChild(i).gameObject.SetActive(false);
        //}

        foreach (CombatUnit detectingUnit in m_detectingUnits)
        {
            detectingUnit.DetectedUnitDestroyed(this);
        }

        foreach (IA targetingUnit in m_targetingIAs)
        {
            targetingUnit.TargetedUnitDestroyed(this);
        }

    }

    #region Actions
    #region Movements
    void RotateMechaHorizontaly(float horizontalAngle)
    {
        Quaternion currentRotation = transform.localRotation;
        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
        transform.localRotation = horizontalRotation * currentRotation;
    }

    void RotateTorsoHorizontaly(float horizontalAngle)
    {
        Quaternion currentRotation = m_torso.transform.rotation;
        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
        m_torso.transform.rotation = horizontalRotation * currentRotation;
    }

    void CheckPilotHead()
    {
        float horizontalAnglePrevision = m_mainCamera.transform.localRotation.eulerAngles.y;
        horizontalAnglePrevision = (horizontalAnglePrevision > 180) ? horizontalAnglePrevision - 360 : horizontalAnglePrevision;
        float toTransforToTorso = 0f;

        if (horizontalAnglePrevision > m_maxHorinzontalHeadAngle)
        {
            toTransforToTorso = (horizontalAnglePrevision - m_maxHorinzontalHeadAngle);
            RotateMechaHorizontaly(0.5f);
        }
        else if (horizontalAnglePrevision < -(m_maxHorinzontalHeadAngle))
        {
            toTransforToTorso = (horizontalAnglePrevision + m_maxHorinzontalHeadAngle);
			RotateMechaHorizontaly(-0.5f);
        }
    }

    void RotatePilotHead(float horizontalAngle, float verticalAngle)
    {
        float horizontalAnglePrevision = m_mainCamera.transform.localRotation.eulerAngles.y;
        horizontalAnglePrevision = (horizontalAnglePrevision > 180) ? horizontalAnglePrevision - 360 : horizontalAnglePrevision;
        horizontalAnglePrevision += horizontalAngle;
        float finalHorizontalAngle = horizontalAngle;
        float toTransforToTorso = 0f;

        if (horizontalAnglePrevision > m_maxHorinzontalHeadAngle)
        {
            toTransforToTorso = (horizontalAnglePrevision - m_maxHorinzontalHeadAngle);
            finalHorizontalAngle -= toTransforToTorso;
            RotateMechaHorizontaly(toTransforToTorso);
        }
        else if (horizontalAnglePrevision < -(m_maxHorinzontalHeadAngle))
        {
            toTransforToTorso = (horizontalAnglePrevision + m_maxHorinzontalHeadAngle);
            finalHorizontalAngle -= toTransforToTorso;
            RotateMechaHorizontaly(toTransforToTorso);
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

    void RotateCameraHorizontaly(float horizontalAngle)
    {
        Quaternion currentRotation = m_mainCamera.transform.rotation;
        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
        m_mainCamera.transform.rotation = horizontalRotation * currentRotation;
    }

    void RotateCameraVerticaly(float verticalAngle)
    {
        Quaternion currentRotation = m_mainCamera.transform.rotation;
        Quaternion verticalRotation = Quaternion.AngleAxis(verticalAngle, Vector3.left);
        m_mainCamera.transform.rotation = currentRotation * verticalRotation;
    }

    void PointDestination(Transform origin)
    {
        RaycastHit hit;
		if (Physics.Raycast(origin.transform.position, origin.transform.forward, out hit))
        {
			if (hit.transform.gameObject.layer == LayerMask.NameToLayer ("Ground")) 
			{
				if (!m_destinationPointer)
					m_destinationPointer = (GameObject) Instantiate(m_pointer, hit.point, Quaternion.identity);
				LineRenderer line = m_destinationPointer.GetComponent<LineRenderer>();
				line.SetPosition(0, origin.position);
				line.SetPosition(1, hit.point);
				m_destinationPointer.transform.position = hit.point;
			}
			else{
				//not a ground, but...
			}
        }
    }

    void ConfirmDestination()
    {
        if (m_destinationPointer)
        {
            SetDestination(m_destinationPointer.transform.position);
            Destroy(m_destinationPointer);
            m_destinationPointer = null;
        }
    }

    void MoveFromLocalRotation(Vector3 direction)
    {
        MoveToDir(transform.localRotation * direction);
    }
    #endregion

    #region Attacks
    void LeftArmWeaponTriggered()
    {
        m_leftWeapon.TriggerPressed();
    }

    void LeftArmWeaponTriggerReleased()
    {
        m_leftWeapon.TriggerReleased();
    }

    void RightArmWeaponTriggered()
    {
        m_rightWeapon.TriggerPressed();
    }

    void RightArmWeaponTriggerReleased()
    {
        m_rightWeapon.TriggerReleased();
    }

    void AimLeftWeaponTo(Vector3 targetPosition)
    {
        m_leftWeapon.transform.LookAt(targetPosition);
    }

    void AimRightWeaponTo(Vector3 targetPosition)
    {
        m_rightWeapon.transform.LookAt(targetPosition);
    }
    #endregion
    #endregion

    #region Inputs

#if UNITY_PS4
    #region PSMoves

    void PSMoveRotation()
    {
		Vector3 leftHand = new Vector3(m_leftController.getMoveRotation().y, 0.0f, 0.0f);
		Vector3 RightHand = new Vector3(m_rightController.getMoveRotation().y, 0.0f, 0.0f);
		float angle = Vector3.Angle(leftHand, RightHand);

		angle /= 90.0f;
        angle = Mathf.Clamp01(angle) * Time.deltaTime;
        angle = (float)System.Math.Round(angle, 2);

		if (m_leftController.getMoveRotation().y < m_rightController.getMoveRotation().y)
            angle = -angle;

        RotateMechaHorizontaly(-angle * m_rotationSpeed);
    }

    void PSMoveLeftWeaponControl()
    {
        m_leftWeapon.transform.localPosition = m_leftWeaponDefaultPosition + ((m_leftController.transform.position - m_baseOffset) * m_sensitivity);
        m_leftWeapon.transform.localRotation = m_leftController.transform.localRotation * m_leftWeaponDefaultRotation;
    }

    void PSMoveRightWeaponControl()
    {
        m_rightWeapon.transform.localPosition = m_rightWeaponDefaultPosition + ((m_rightController.transform.position - m_baseOffset) * m_sensitivity);
        m_rightWeapon.transform.localRotation = m_rightController.transform.localRotation * m_rightWeaponDefaultRotation;
    }

    void PSMoveInputs()
    {
		bool leftModifier = m_leftController.GetButton(MoveController.MoveButton.MoveButton_Cross);
		bool rightModifier = m_rightController.GetButton(MoveController.MoveButton.MoveButton_Cross);

		bool leftPointer = m_leftController.GetButton(MoveController.MoveButton.MoveButton_Move);
		bool rightPointer = m_rightController.GetButton(MoveController.MoveButton.MoveButton_Move);

		if (m_leftController.GetButtonUp(MoveController.MoveButton.MoveButton_Move) && !rightPointer) ConfirmDestination();
		if (m_rightController.GetButtonUp(MoveController.MoveButton.MoveButton_Move) && !leftPointer) ConfirmDestination();

		GameObject debug = GameObject.FindGameObjectWithTag ("Debug");

		if(debug != null)
		{
			debug.GetComponent<Text>().text = "left:" + m_leftController.GetButton(MoveController.MoveButton.MoveButton_Trigger) + "right:" + m_rightController.GetButton(MoveController.MoveButton.MoveButton_Trigger);
		}

		if (leftModifier)
        {
            if (leftModifier && rightModifier)
            {
                PSMoveRotation();
            }
            else
            {
                MoveFromLocalRotation(PSMoveVirtualJoysticksConvertion(0));
                LeftArmWeaponTriggerReleased();
            }
        }
        else
        {
            PSMoveLeftWeaponControl();
            if (leftPointer)
            {
                PointDestination(m_leftWeapon.m_muzzle);
                LeftArmWeaponTriggerReleased();
            }
            else
            {
				if (m_leftController.GetButtonDown(MoveController.MoveButton.MoveButton_Trigger)) LeftArmWeaponTriggered();
				if (m_leftController.GetButtonUp(MoveController.MoveButton.MoveButton_Trigger)) LeftArmWeaponTriggerReleased();
            }
        }

        if (rightModifier)
        {
            if (leftModifier && rightModifier)
            {
                PSMoveRotation();
            }
            else
            {
                MoveFromLocalRotation(PSMoveVirtualJoysticksConvertion(1));
                RightArmWeaponTriggerReleased();
            }
        }
        else
        {
            PSMoveRightWeaponControl();
            if (rightPointer)
            {
                PointDestination(m_rightWeapon.m_muzzle);
                RightArmWeaponTriggerReleased();
            }
            else
            {
				if (m_rightController.GetButtonDown(MoveController.MoveButton.MoveButton_Trigger)) RightArmWeaponTriggered();
				if (m_rightController.GetButtonUp(MoveController.MoveButton.MoveButton_Trigger)) RightArmWeaponTriggerReleased();
            }
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

#if UNITY_STANDALONE
    #region Razer Hydra
    Vector3 RazerVirtualJoysticksConvertion(SixenseInput.Controller controller)
	{
		Vector3 movementDirection = Vector3.zero;

		float x = controller.Rotation.x;
		float y = controller.Rotation.y;

		const float NEUTRAL_Z = -0.3f;
		const float NEUTRAL_X = 0f;

		const float MAX_FORWARD = 0f;
		const float MIN_FORWARD = -0.2f;
		const float MAX_BACKWARD = -0.6f;
		const float MIN_BACKWARD = -0.4f;
		const float MAX_LEFT = -0.2f;
		const float MIN_LEFT = -0.1f;
		const float MAX_RIGHT = 0.2f;
		const float MIN_RIGHT = 0.1f;

		float zdir = 0;
		if (x > NEUTRAL_Z) //avant
			zdir = (Mathf.InverseLerp (MIN_FORWARD, MAX_FORWARD, x));
		if (x < NEUTRAL_Z) //arrière
			zdir = -(Mathf.InverseLerp (MIN_BACKWARD, MAX_BACKWARD, x));

		float xdir = 0;
		if (y < NEUTRAL_X) //gauche
			xdir = -(Mathf.InverseLerp (MIN_LEFT, MAX_LEFT, y));
		if (y > NEUTRAL_X) //droite
			xdir = (Mathf.InverseLerp (MIN_RIGHT, MAX_RIGHT, y));

		movementDirection += new Vector3 (xdir, 0f, zdir);

        return movementDirection;
	}

    void RazerLeftWeaponControl()
    {
        m_leftWeapon.transform.localPosition = m_leftWeaponDefaultPosition + ((m_leftController.Position - m_baseOffset) * m_sensitivity);
        m_leftWeapon.transform.localRotation = m_leftController.Rotation * m_leftWeaponDefaultRotation;
    }

    void RazerRightWeaponControl()
    {
        m_rightWeapon.transform.localPosition = m_rightWeaponDefaultPosition + ((m_rightController.Position - m_baseOffset) * m_sensitivity);
        m_rightWeapon.transform.localRotation = m_rightController.Rotation * m_rightWeaponDefaultRotation;
    }

    void RazerRotation()
    {
        Quaternion leftHand = Quaternion.Euler(new Vector3(m_leftController.Rotation.eulerAngles.x, 0.0f, 0.0f));
        Quaternion RightHand = Quaternion.Euler(new Vector3(m_rightController.Rotation.eulerAngles.x, 0.0f, 0.0f));
        float angle = Quaternion.Angle(leftHand, RightHand);

        angle /= 90.0f;
        angle = Mathf.Clamp01(angle) * Time.deltaTime;
        angle = (float)System.Math.Round(angle, 2);

        if (m_leftController.Rotation.eulerAngles.x < m_rightController.Rotation.eulerAngles.x)
            angle = -angle;

        RotateMechaHorizontaly(-angle * m_rotationSpeed);
    }

    void RazerInputs()
    {
        if (m_leftController != null && m_rightController != null)
        {
            bool leftModifier = m_leftController.GetButton(SixenseButtons.ONE);
            bool rightModifier = m_rightController.GetButton(SixenseButtons.ONE);

            bool leftPointer = m_leftController.GetButton(SixenseButtons.BUMPER);
            bool rightPointer = m_rightController.GetButton(SixenseButtons.BUMPER);

            if (m_leftController.GetButtonUp(SixenseButtons.BUMPER) && !rightPointer) ConfirmDestination();
            if (m_rightController.GetButtonUp(SixenseButtons.BUMPER) && !leftPointer) ConfirmDestination();
            
            if (leftModifier)
            {
                if (leftModifier && rightModifier)
                {
                    RazerRotation();
                }
                else
                {
                    MoveFromLocalRotation(RazerVirtualJoysticksConvertion(m_leftController));
                    LeftArmWeaponTriggerReleased();
                }
            }
            else
            {
                RazerLeftWeaponControl();
                if (leftPointer)
                {
                    PointDestination(m_leftWeapon.m_muzzle);
                    LeftArmWeaponTriggerReleased();
                }
                else
                {
                    if (m_leftController.GetButtonDown(SixenseButtons.TRIGGER)) LeftArmWeaponTriggered();
                    if (m_leftController.GetButtonUp(SixenseButtons.TRIGGER)) LeftArmWeaponTriggerReleased();
                }
            }

            if (rightModifier)
            {
                if (leftModifier && rightModifier)
                {
                    RazerRotation();
                }
                else
                {
                    MoveFromLocalRotation(RazerVirtualJoysticksConvertion(m_rightController));
                    RightArmWeaponTriggerReleased();
                }
            }
            else
            {
                RazerRightWeaponControl();
                if (rightPointer)
                {
                    PointDestination(m_rightWeapon.m_muzzle);
                    RightArmWeaponTriggerReleased();
                }
                else
                {
                    if (m_rightController.GetButtonDown(SixenseButtons.TRIGGER)) RightArmWeaponTriggered();
                    if (m_rightController.GetButtonUp(SixenseButtons.TRIGGER)) RightArmWeaponTriggerReleased();
                }
            }
        }
    }
    #endregion

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
            AimLeftWeaponTo(m_mainCamera.transform.position + m_mainCamera.transform.forward * 100);
            AimRightWeaponTo(m_mainCamera.transform.position + m_mainCamera.transform.forward * 100);
        }
    }

    void MouseShootInputs()
    {
        if (Input.GetMouseButtonDown(0)) LeftArmWeaponTriggered();
        if (Input.GetMouseButtonUp(0)) LeftArmWeaponTriggerReleased();
        if (Input.GetMouseButtonDown(1)) RightArmWeaponTriggered();
        if (Input.GetMouseButtonUp(1)) RightArmWeaponTriggerReleased();
    }

    void KeyboardMovements()
    {
        Vector3 movementDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.Z)) movementDirection.z += 1f;
        if (Input.GetKey(KeyCode.S)) movementDirection.z -= 1f;
        if (Input.GetKey(KeyCode.D)) movementDirection.x += 1f;
        if (Input.GetKey(KeyCode.Q)) movementDirection.x -= 1f;
        
		if ( movementDirection != Vector3.zero) MoveFromLocalRotation(movementDirection);
        else ResumePath();

        if (Input.GetMouseButton(2))
        {
            PointDestination(m_mainCamera.transform);
        }
        if (Input.GetMouseButtonUp(2))
        {
            ConfirmDestination();
        }
    }

    void MouseKeyboardInputs()
    {
        MouseAim();
        MouseShootInputs();
        KeyboardMovements();
    }

	#endregion
#endif
	#region Controller
	void JoystickRotation()
	{
		float hor = Input.GetAxis ("HorizontalR");
		float vert = Input.GetAxis ("VerticalR");

		if(hor != 0.0f && vert != 0.0f)
		{
			RotatePilotHead (hor, vert);
			AimLeftWeaponTo(m_mainCamera.transform.position + m_mainCamera.transform.forward * 100);
			AimRightWeaponTo(m_mainCamera.transform.position + m_mainCamera.transform.forward * 100);
		}
	}

	void ControllerInputs()
	{
		bool leftModifier = Input.GetButton("Fire1");
		bool rightModifier = Input.GetButton("Fire2");

		bool leftPointer = Input.GetButton("LeftBumper"); //bumper
		bool rightPointer = Input.GetButton("RightBumper"); //bumper

		if (Input.GetButtonUp("LeftBumper") && !rightPointer) ConfirmDestination(); //bumper
		if (Input.GetButtonUp("RightBumper") && !leftPointer) ConfirmDestination(); //bumper

		JoystickRotation();

        MoveFromLocalRotation(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")));

		if (leftModifier)
		{
			MoveFromLocalRotation( new Vector3( Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")) );
			LeftArmWeaponTriggerReleased();
		}
		else
		{
			if (leftPointer)
			{
				PointDestination(m_leftWeapon.m_muzzle);
				LeftArmWeaponTriggerReleased();
			}
			else
			{
				if (Input.GetAxis("LeftTrigger") > 0.0f) LeftArmWeaponTriggered();
				if (Input.GetAxis("LeftTrigger") <= 0.0f) LeftArmWeaponTriggerReleased();
			}
		}

		if (rightModifier)
		{
			MoveFromLocalRotation( new Vector3( Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")) );
			RightArmWeaponTriggerReleased();
		}
		else
		{
			if (rightPointer)
			{
				PointDestination(m_rightWeapon.m_muzzle);
				RightArmWeaponTriggerReleased();
			}
			else
			{
				if (Input.GetAxis("RightTrigger") > 0.0f) RightArmWeaponTriggered();
				if (Input.GetAxis("RightTrigger") <= 0.0f) RightArmWeaponTriggerReleased();
			}
		}
	}

	#endregion
    #endregion

    #region Updates
    void InputsUpdate()
    {
        #if UNITY_STANDALONE
        MouseKeyboardInputs();
		if(RazerAreConnected)
			RazerInputs();
		#elif UNITY_PS4
        CheckPilotHead();
		PSMoveInputs();
        #endif

		//ControllerInputs();
    }

    protected override void Update()
    {
        base.Update();
        InputsUpdate();
    }
#endregion
}
