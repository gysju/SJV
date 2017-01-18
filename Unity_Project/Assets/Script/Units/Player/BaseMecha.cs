using UnityEngine;
using System.Collections;

public class BaseMecha : BaseUnit
{
    protected BaseWeapon m_leftWeapon;
    protected BaseWeapon m_rightWeapon;

    public MechaTorso m_torso;

	private IEnumerator previousLeftVibrationCoroutine;
	private IEnumerator previousRightVibrationCoroutine;

    protected override void Awake()
    {
        base.Awake();
        m_torso = GetComponentInChildren<MechaTorso>();
        m_leftWeapon = m_weapons[0];
        m_rightWeapon = m_weapons[1];
    }

    public void RotateMechaHorizontaly(float horizontalAngle)
    {
        Quaternion currentRotation = m_transform.localRotation;
        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
        m_transform.localRotation = horizontalRotation * currentRotation;
    }

    public void LeftArmWeaponTriggered()
    {
        m_leftWeapon.TriggerPressed();
		if( previousLeftVibrationCoroutine != null )
			StopCoroutine ( previousLeftVibrationCoroutine);
		previousLeftVibrationCoroutine = TrackedDeviceMoveControllers.Instance.primaryMoveController.Vibration (100.0f, 100.0f, .5f);
		StartCoroutine( previousLeftVibrationCoroutine );
    }

    public void LeftArmWeaponTriggerReleased()
    {
        m_leftWeapon.TriggerReleased();
    }

    public void RightArmWeaponTriggered()
    {
        m_rightWeapon.TriggerPressed();
		if( previousRightVibrationCoroutine != null )
			StopCoroutine ( previousRightVibrationCoroutine);
		previousRightVibrationCoroutine = TrackedDeviceMoveControllers.Instance.secondaryMoveController.Vibration (100.0f, 100.0f, .5f);
		StartCoroutine( previousRightVibrationCoroutine );
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
