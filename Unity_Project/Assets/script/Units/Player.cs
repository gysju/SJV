using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MobileGroundUnit
{
    private Camera m_mainCamera;
    public float m_maxHorinzontalHeadAngle = 10f;
    public float m_maxVerticalHeadAngle = 75f;

    //private List<SixenseInput.Controller> m_razerControllers;

    [Header("Player Related")]
    public GameObject m_torso;
    public GameObject m_legs;

    public GameObject m_pointer;
    private GameObject m_destinationPointer = null;

    #region Initialisation
    protected override void Start()
    {
		base.Start();
        m_mainCamera = Camera.main;
    }
    #endregion

    #region Actions
    #region Movements
    void RotateTorsoHorizontaly(float horizontalAngle)
    {
        Quaternion currentRotation = m_torso.transform.rotation;
        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
        m_torso.transform.rotation = horizontalRotation * currentRotation;
    }

    void RotatePilotHead(float horizontalAngle, float verticalAngle)
    {
        Quaternion currentRotation = m_mainCamera.transform.rotation;
        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
        Quaternion verticalRotation = Quaternion.AngleAxis(verticalAngle, Vector3.left);
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
            if (!m_destinationPointer)
                m_destinationPointer = (GameObject) Instantiate(m_pointer, hit.point, Quaternion.identity);
            LineRenderer line = m_destinationPointer.GetComponent<LineRenderer>();
            line.SetPosition(0, origin.position);
            line.SetPosition(1, hit.point);
            m_destinationPointer.transform.position = hit.point;
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
    #endregion

    #region Attacks
    void LeftArmWeaponTriggered(bool value)
    {
        if (value) m_weapons[0].TriggerPressed();
        else m_weapons[0].TriggerReleased();
    }

    void RightArmWeaponTriggered(bool value)
    {
        if (value) m_weapons[1].TriggerPressed();
        else m_weapons[1].TriggerReleased();
    }
    #endregion
    #endregion

    #region Inputs

    #region PSMoves

    #endregion

    #region Razer Hydra
    Vector3 RazerVirtualJoysticksConvertion(SixenseInput.Controller controller)
	{
		Vector3 movementDirection = Vector3.zero;

		float x = controller.RotationRaw.x;
		float y = controller.RotationRaw.y;
        Debug.Log(x + " | " + y);
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
			zdir = (Mathf.InverseLerp (MIN_FORWARD, MAX_FORWARD, x) * Time.deltaTime);
		if (x < NEUTRAL_Z) //arrière
			zdir = -(Mathf.InverseLerp (MIN_BACKWARD, MAX_BACKWARD, x) * Time.deltaTime);

		float xdir = 0;
		if (y < NEUTRAL_X) //gauche
			xdir = -(Mathf.InverseLerp (MIN_LEFT, MAX_LEFT, y) * Time.deltaTime);
		if (y > NEUTRAL_X) //droite
			xdir = (Mathf.InverseLerp (MIN_RIGHT, MAX_RIGHT, y) * Time.deltaTime);

		movementDirection += new Vector3 (xdir, 0f, zdir);

        return movementDirection;
	}

	void PointingSystem(SixenseHand hand)
	{
		if (hand.m_controller.GetButton(SixenseButtons.BUMPER))
		{
			Transform origin = hand.GetComponent<Weapon>().transform;

            PointDestination(origin);
		}

		if (CheckDestination() && hand.m_controller.GetButtonUp(SixenseButtons.BUMPER))
		{
            ConfirmDestination();
		}
    }

    //void RazerMovementInputs()
    //{
    //    bool test = false;
    //    foreach (SixenseHand hand in m_razerControllers)
    //    {
    //        if (hand.m_controller != null)
    //        {
    //            if (hand.m_controller.GetButton(SixenseButtons.ONE))
    //            {
    //                PauseNavMesh();
    //                RazerVirtualJoysticksConvertion(hand);
    //                test = true;
    //            }
    //        }
    //    }
    //    foreach (SixenseHand hand in m_razerControllers)
    //    {
    //        if (hand.m_controller != null && !test)
    //        {
    //            PointingSystem(hand);
    //        }
    //    }
    //}

    void RazerInputs()
    {
        if (SixenseInput.Controllers[0] != null)
        {
            SixenseInput.Controller leftController = SixenseInput.Controllers[0];
            SixenseInput.Controller rightController = SixenseInput.Controllers[1];

            bool leftModifier = leftController.GetButton(SixenseButtons.ONE);
            bool rightModifier = rightController.GetButton(SixenseButtons.ONE);

            bool leftPointer = leftController.GetButton(SixenseButtons.BUMPER);
            bool rightPointer = rightController.GetButton(SixenseButtons.BUMPER);

            if (leftModifier) MoveToDir(RazerVirtualJoysticksConvertion(leftController));
            else if (leftPointer) PointDestination(m_weapons[0].m_muzzle);
            else LeftArmWeaponTriggered(leftController.GetButton(SixenseButtons.TRIGGER));

            if (rightModifier) MoveToDir(RazerVirtualJoysticksConvertion(rightController));
            else if (rightPointer) PointDestination(m_weapons[1].m_muzzle);
            else RightArmWeaponTriggered(rightController.GetButton(SixenseButtons.TRIGGER));
        }
    }
    #endregion

    #region Mouse & Keyboard
    void MouseAim()
    {
        RotatePilotHead(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    void MouseShootInputs()
    {
        LeftArmWeaponTriggered(Input.GetMouseButton(0));
        RightArmWeaponTriggered(Input.GetMouseButton(1));
    }

    void MouseKeyboardInputs()
    {
        MouseAim();
        MouseShootInputs();
    }
    #endregion
    #endregion

    #region Updates
    void InputsUpdate()
    {
        
        RazerInputs();
    }

    protected override void Update()
    {
        base.Update();
        InputsUpdate();
    }
    #endregion
}
