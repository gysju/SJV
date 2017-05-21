using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : BaseUnit
{
    public EnemiesManager m_poolManager;

    protected enum EnemyState
    {
        EnemyState_Sleep,
        EnemyState_Moving,
        EnemyState_Attacking
    }
    protected EnemyState m_enemyState = EnemyState.EnemyState_Sleep;

    protected Vector3? m_attackPosition = null;


    [Header("Attack")]
    [Tooltip("Time the unit will take to shoot.")]
    [Range(1f, 5f)]
    public float m_timeToAttack = 2f;
    protected float m_currentTimeToAttack;

    protected Transform m_target;

	public float DeathfadeSpeed = 1.0f;

	public Color EmissiveColor;
	private Material material;

    protected AudioSource audioSource;

	[Header("Effect")]

	[Range(0.0f, 1.0f)]
	public float ShockDuration = 1.0f;
	[Range(0.0f, 1.0f)]
	public float ShockIntensity = 1.0f;
	public AnimationCurve Shockcurve;

	[Space(10)]
	[Range(1.0f, 1.25f)]
	public float ScaleIntensity = 1.0f;

	public AnimationCurve ScaleCurve;

	protected Coroutine ShockCoroutine = null;
	protected Transform modelTransform;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        m_currentTimeToAttack = m_timeToAttack;
		material = GetComponentInChildren<SkinnedMeshRenderer> ().material;
        audioSource = GetComponent<AudioSource>();

        if (!m_poolManager) m_poolManager = FindObjectOfType<EnemiesManager>();
		modelTransform = transform.FindChild ("Model");
    }

    public virtual void ResetUnit(Vector3 spawn, Vector3 movementTarget, Transform target)
    {
        m_transform.position = spawn;

        m_attackPosition = movementTarget;

        m_target = target;

        m_currentHitPoints = m_maxHitPoints;

        m_destroyed = false;

        m_enemyState = EnemyState.EnemyState_Sleep;

		HUD_Radar.Instance.AddInfo (this);

        LaserOff();

		//if (m_animator != null) 
		//{
		//	m_animator.SetTrigger ("Idle");
		//}
        StartCoroutine (SpawnFade ());
    }
    #endregion

    #region HitPoints Related
	public override bool ReceiveDamages(int damages, int armorPenetration = 0)
	{
		if (base.ReceiveDamages(damages, armorPenetration))
		{
			//ShockCoroutine = StartCoroutine (Shock ());
			return true;
		}
		return false;
	}

    /// <summary>A appeler à la mort de l'unité.</summary>
    protected override void StartDying()
    {
        m_attackPosition = null;
        m_target = null;
        m_enemyState = EnemyState.EnemyState_Sleep;
		HUD_Radar.Instance.RemoveInfo (this);
        LaserOff();
		//StartCoroutine (DeathFade());
        base.StartDying();
    }

    protected override IEnumerator Dying()
    {
        WaitForSeconds wait = new WaitForSeconds(m_timeToDie);
        yield return wait;
        
        yield return StartCoroutine(DeathFade());
        if (m_destructionSpawn) Instantiate(m_destructionSpawn, transform.position, transform.rotation);
        FinishDying();
    }

    protected override void FinishDying()
    {
        if (m_animator)
            m_animator.SetTrigger("Idle");
        m_poolManager.PoolUnit(this);
    }
    #endregion

    #region Movement Related
    public virtual void StartMovement()
    {
        m_enemyState = EnemyState.EnemyState_Moving;

        if (m_animator) m_animator.SetTrigger("Locomotion");
    }
    #endregion

    #region Attack Related
    public virtual void AimWeaponAt(Vector3 target)
    {
        foreach (BaseWeapon weapon in m_weapons)
        {
            weapon.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(weapon.transform.forward, (target - weapon.transform.position).normalized, 1f * Time.deltaTime, 0f));
            //weapon.transform.LookAt(target);
        }
    }

    public void PressWeaponTrigger(int weaponID)
    {
        m_weapons[weaponID].TriggerPressed(null);
    }

    public void ReleaseWeaponTrigger(int weaponID)
    {
        m_weapons[weaponID].TriggerReleased();
    }

    public void CeaseFire()
    {
        foreach (BaseWeapon weapon in m_weapons)
        {
            weapon.TriggerReleased();
        }
    }
    #endregion

    #region Updates
    protected virtual void Update()
    {
        //if (!m_destroyed)
        //{

        //}
    }

	void StartDeathFade()
	{
		StartCoroutine (DeathFade ());
	}

	public IEnumerator DeathFade()
	{
		float time = 0.0f;

		while( time < DeathfadeSpeed )
		{
			time += Time.deltaTime;
			material.SetFloat("_AlphaValue", Mathf.Lerp(1.0f, 0.0f, (time / DeathfadeSpeed))); 
			yield return null; 	
		}
	}

	public IEnumerator SpawnFade()
	{
		float time = 0.0f;

		while( time < DeathfadeSpeed )
		{
			time += Time.deltaTime;
			material.SetFloat("_AlphaValue", Mathf.Lerp(0.0f, 1.0f, (time / DeathfadeSpeed))); 
			yield return null; 	
		}
	}

	public IEnumerator Shock()
	{
		float time = 0.0f;
		Vector3 dir = (modelTransform.position - BaseMecha.instance.transform.position).normalized;
		Vector3 initPos = modelTransform.localPosition = Vector3.zero;
		modelTransform.localScale = Vector3.one;

		while( time < ShockDuration)
		{
			time += Time.deltaTime;
			modelTransform.localPosition = Vector3.Lerp( initPos, initPos - dir * ShockIntensity, Shockcurve.Evaluate(time / ShockDuration));
			modelTransform.localScale = Vector3.Lerp( Vector3.one, Vector3.one * ScaleIntensity, ScaleCurve.Evaluate(time / ShockDuration));
			yield return null;
		}
	}
    #endregion
}
