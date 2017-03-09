using UnityEngine;
using System.Collections;

public class BaseMecha : BaseUnit
{
	public static BaseMecha Instance = null;

    protected BaseWeapon m_leftWeapon;
    protected BaseWeapon m_rightWeapon;

    public GameObject m_bunker;
    public MechaTorso m_torso;
    public MechaLegs m_legs;

    protected ZAManager m_zaManager;

    protected override void Awake()
    {
		if (Instance == null) 
		{
			Instance = this;
			base.Awake ();
			m_torso = GetComponentInChildren<MechaTorso> ();
			m_leftWeapon = m_weapons [0];
			m_rightWeapon = m_weapons [1];
			m_bunker.SetActive (false);
			m_zaManager = FindObjectOfType<ZAManager> ();

            LaserOn();
        } 
		else if ( Instance != this )
		{
			Destroy (gameObject);
		}
    }

    protected override void StartDying()
    {
        m_destroyed = true;

        ActivateBunkerMode();

        LaserOff();

        StartCoroutine(Dying());
    }

    protected override void FinishDying()
    {
        m_zaManager.BackToMainMenu();
    }

    public void ActivateBunkerMode()
    {
        m_bunker.SetActive(true);
    }

    public void RotateMechaHorizontaly(float horizontalAngle)
    {
        Quaternion currentRotation = m_transform.localRotation;
        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
        m_transform.localRotation = horizontalRotation * currentRotation;
    }

    public void LeftArmWeaponTriggered()
    {
        m_leftWeapon.TriggerPressed(TrackedDeviceMoveControllers.Instance.primaryMoveController);
    }

    public void LeftArmWeaponTriggerReleased()
    {
        m_leftWeapon.TriggerReleased();
    }

    public void RightArmWeaponTriggered()
    {
        m_rightWeapon.TriggerPressed(TrackedDeviceMoveControllers.Instance.secondaryMoveController);
    }

    public void RightArmWeaponTriggerReleased()
    {
        m_rightWeapon.TriggerReleased();
    }

    public void MoveLeftWeapon(Vector3 newPosition)
    {
        m_leftWeapon.transform.localPosition = newPosition;
    }

    public void MoveRightWeapon(Vector3 newPosition)
    {
        m_rightWeapon.transform.localPosition = newPosition;
    }

    public void AimLeftWeaponTo(Vector3 targetPosition)
    {
        m_leftWeapon.transform.LookAt(targetPosition);
    }

    public void AimRightWeaponTo(Vector3 targetPosition)
    {
        m_rightWeapon.transform.LookAt(targetPosition);
    }
}
