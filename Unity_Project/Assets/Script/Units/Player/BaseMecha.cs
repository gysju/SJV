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

	public MeshRenderer meshRendererSeeTrough;
	private Material SeeTroughMaterial;
	private Material SeeTroughMaterialChild;

	private float speedHit = 1.0f;
	private float radiusMax = 1.0f;

	private Coroutine HitCoroutine = null;
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

			if (meshRendererSeeTrough != null) 
			{
				SeeTroughMaterial = meshRendererSeeTrough.material;
				SeeTroughMaterialChild = meshRendererSeeTrough.GetComponentInChildren<MeshRenderer> ().material;

				speedHit = SeeTroughMaterial.GetFloat ("_HitSpeed");
				radiusMax = SeeTroughMaterial.GetFloat ("_RadiusMax");
			}
			
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

	public void HitEffect( Vector3 hitPos)
	{
		if (SeeTroughMaterial == null)
			return;
		
		SeeTroughMaterial.SetVector ("_HitPos", new Vector4 (hitPos.x, hitPos.y, hitPos.z, 1.0f));
		SeeTroughMaterialChild.SetVector ("_HitPos", new Vector4 (hitPos.x, hitPos.y, hitPos.z, 1.0f));

		if (HitCoroutine != null)
			StopCoroutine (HitCoroutine);
		HitCoroutine = StartCoroutine (LaunchHitTime());
	}

	IEnumerator LaunchHitTime()
	{
		float time = 0;
		Debug.Log (radiusMax);
		while( time <  speedHit)
		{
			time += Time.deltaTime;

			float norm = time / speedHit;

			SeeTroughMaterial.SetFloat ("_RadiusMax", Mathf.Lerp(0, radiusMax, norm));
			SeeTroughMaterialChild.SetFloat ("_RadiusMax", Mathf.Lerp(0, radiusMax, norm));
			Debug.Log (Mathf.Lerp (0, radiusMax, norm));
			yield return null;
		}

		SeeTroughMaterial.SetFloat ("_RadiusMax", 0.0f);
		SeeTroughMaterialChild.SetFloat ("_RadiusMax", 0.0f);
	}
}
