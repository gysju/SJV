using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : BaseUnit
{
    public EnemiesManager m_manager;

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

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        m_model = GetComponentInChildren<MeshRenderer>();
        m_currentTimeToAttack = m_timeToAttack;
        if(!m_manager) m_manager = FindObjectOfType<EnemiesManager>();
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
        m_manager.PoolUnit(this);
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
    #endregion
}
