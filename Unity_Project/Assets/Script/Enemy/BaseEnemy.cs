using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("MechaVR/Enemies/BaseEnemy")]
public class BaseEnemy : BaseUnit
{

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
    protected virtual void Awake()
    {
        m_transform = transform;
        m_model = GetComponentInChildren<MeshRenderer>();
        m_currentTimeToAttack = m_timeToAttack;
    }

    public virtual void ResetUnit(Vector3 spawn, Vector3 movementTarget, Transform target)
    {
        m_transform.position = spawn;

        m_attackPosition = movementTarget;

        m_target = target;

        m_destroyed = false;

        m_enemyState = EnemyState.EnemyState_Sleep;
    }

    public virtual void TestUnit()
    {
        ResetUnit(new Vector3(15f,0f, 120f), new Vector3(5f, 0f, 50f), FindObjectOfType<Player>().transform);
    }
    #endregion

    #region HitPoints Related
    protected override void StartDying()
    {
        m_enemyState = EnemyState.EnemyState_Sleep;
        base.StartDying();
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
        foreach (Weapon weapon in m_weapons)
        {
            weapon.transform.LookAt(target);
        }
    }

    protected void FireWeapon(int weaponID)
    {
        m_weapons[weaponID].FireWeapon();
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
        foreach (Weapon weapon in m_weapons)
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
