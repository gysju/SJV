using UnityEngine;
using System.Collections;

public class BaseMecha : BaseUnit
{
	public static BaseMecha _instance = null;

    public PlayerInputs m_inputs;
    public PlayerInterface m_interface;

    protected BaseWeapon m_leftWeapon;
    protected BaseWeapon m_rightWeapon;

    public CockpitBunker m_bunker;

    public MechaTorso m_torso;
    public MechaLegs m_legs;

    protected ZAManager m_zaManager;

	public MeshRenderer meshRendererSeeTrough;
	private Material SeeTroughMaterial;
	private Material SeeTroughMaterialChild;

	private float speedHit = 1.0f;
	private float radiusMax = 1.0f;

	private Coroutine HitCoroutine = null;

    [HideInInspector]
    public AsyncOperation m_levelLoading = null;

    public static BaseMecha instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BaseMecha>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    protected override void Awake()
    {
        if (_instance == null) 
		{
			_instance = this;
            DontDestroyOnLoad(this);
            base.Awake ();
            m_inputs = GetComponentInChildren<PlayerInputs>();
            m_interface = GetComponentInChildren<PlayerInterface>();
            m_torso = GetComponentInChildren<MechaTorso> ();
			m_leftWeapon = m_weapons [0];
			m_rightWeapon = m_weapons [1];
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
		else if ( _instance != this )
		{
			Destroy (gameObject);
		}
    }

    public void BackToBase()
    {
        m_currentHitPoints = m_maxHitPoints;
        m_destroyed = false;
    }

    public void PrepareExtraction()
    {
        m_inputs.m_weaponsConnected = false;
        m_inputs.m_inGame = false;
        m_interface.HideHelmetHUD();
        m_bunker.ActivateBunkerMode();

#if UNITY_STANDALONE

#endif

    }

    public void ReadyToAction()
    {
        m_inputs.m_weaponsConnected = true;
        m_inputs.m_inGame = true;
        m_interface.ShowHelmetHUD();
        //m_bunker.DeactivateBunkerMode();
    }

    protected override void StartDying()
    {
        PrepareExtraction();
        HUD_Radar.Instance.RemoveAllInfos();
        LaserOff();

        StartCoroutine(Dying());
    }

    protected override void FinishDying()
    {
        m_destroyed = true;
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

    public override bool ReceiveDamages(int damages, int armorPenetration = 0)
    {
        StartCoroutine(CameraManager.Instance.ChromaticAberationShake());
        return base.ReceiveDamages(damages, armorPenetration);
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
		while( time <  speedHit)
		{
			time += Time.deltaTime;

			float norm = time / speedHit;

			SeeTroughMaterial.SetFloat ("_RadiusMax", Mathf.Lerp(0, radiusMax, norm));
			SeeTroughMaterialChild.SetFloat ("_RadiusMax", Mathf.Lerp(0, radiusMax, norm));
            yield return null;
		}

		SeeTroughMaterial.SetFloat ("_RadiusMax", 0.0f);
		SeeTroughMaterialChild.SetFloat ("_RadiusMax", 0.0f);
	}

    public float GetLeftWeaponHeat()
    {
        return m_leftWeapon.GetHeat();
    }

    public float GetRightWeaponHeat()
    {
        return m_rightWeapon.GetHeat();
    }

    protected virtual void Update()
    {
        if (m_levelLoading != null)
        {
            if (m_levelLoading.isDone)
            {
                ReadyToAction();
                m_levelLoading = null;
            }
        }
    }
}
