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
    [ContextMenuItem("Test Unit", "TestUnit")]
    [Range(1f, 5f)]
    public float m_timeToAttack = 2f;
    protected float m_currentTimeToAttack;

    protected Transform m_target;

	public float DeathfadeSpeed = 1.0f;

	public Color EmissiveColor;
	private Material material;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        m_model = GetComponentInChildren<MeshRenderer>();
        m_currentTimeToAttack = m_timeToAttack;
		material = GetComponentInChildren<SkinnedMeshRenderer> ().material;

        if(!m_poolManager) m_poolManager = FindObjectOfType<EnemiesManager>();
    }

    public virtual void ResetUnit(Vector3 spawn, Vector3 movementTarget, Transform target)
    {
        m_transform.position = spawn;

        m_attackPosition = movementTarget;

        m_target = target;

        m_currentHitPoints = m_startingHitPoints;

        m_destroyed = false;

        m_enemyState = EnemyState.EnemyState_Sleep;
    }

    public virtual void TestUnit()
    {
        ResetUnit(new Vector3(15f,0f, 120f), new Vector3(5f, 0f, 50f), FindObjectOfType<Player>().transform);
    }
    #endregion

    #region HitPoints Related
    /// <summary>A appeler à la mort de l'unité.</summary>
    protected override void StartDying()
    {
        m_attackPosition = null;
        m_target = null;
        m_enemyState = EnemyState.EnemyState_Sleep;
        base.StartDying();
    }

    protected override void FinishDying()
    {
        m_poolManager.PoolUnit(this);
		material.SetFloat ("_AlphaValue", 1.0f);
    }
    #endregion

    #region Movement Related
    public virtual void StartMovement()
    {
        m_enemyState = EnemyState.EnemyState_Moving;
    }
    #endregion

    #region Attack Related
    public virtual void AimWeaponAt(Vector3 target)
    {
        foreach (BaseWeapon weapon in m_weapons)
        {
            weapon.transform.LookAt(target);
        }
    }

    public void PressWeaponTrigger(int weaponID)
    {
        m_weapons[weaponID].TriggerPressed();
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
        if (!m_destroyed)
        {

        }
    }

	void StartDeathFade()
	{
		StartCoroutine (DeathFade ());
	}

	IEnumerator DeathFade()
	{
		float time = 0.0f;

		while( time < DeathfadeSpeed )
		{
			time += Time.deltaTime;
			material.SetFloat("_AlphaValue", Mathf.Lerp(1.0f, 0.0f, (time / DeathfadeSpeed))); 
			yield return null; 	
		}
	}
    #endregion
}
