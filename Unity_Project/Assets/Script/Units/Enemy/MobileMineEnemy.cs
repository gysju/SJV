using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileMineEnemy : GroundEnemy
{
    [Header("Explosive")]
    public float m_explosionRange = 2f;
    public int m_damages = 2;
    public string ExplosiveSound;

    #region Initialization
    protected override void Start()
    {
        m_weaponsTarget = BaseMecha.instance.m_transform;
    }

    protected override void ChooseTargets()
    {
        m_weaponsTarget = m_player.m_transform;
    }
    #endregion

    #region HitPoints Related
    /// <summary>A appeler à la mort de l'unité.</summary>
    protected override void StartDying()
    {
        base.StartDying();
    }
    #endregion
    #region Attack related
    protected override bool IsTargetInRange()
    {
        return (Vector3.Distance(m_transform.position, m_weaponsTarget.position) < m_explosionRange);
    }

    protected void Explode(BaseUnit target)
    {
        SoundManager.Instance.PlaySound(ExplosiveSound, audioSource);
        target.ReceiveDamages(m_damages, 1);
        StartDying();

    }
    #endregion

    #region Updates
    protected override void MovingUpdate()
    {
        if (IsTargetInRange())
        {
            Explode(m_player);
        }
    }

    protected override void AttackUpdate()
{
        if (IsTargetInRange())
        {
            Explode(m_player);
        }
    }

    protected override void StateUpdate()
    {
        switch (m_enemyState)
        {
            case EnemyState.EnemyState_Sleep:
                if (m_weaponsTarget)
                {
                    //MoveTo(m_attackPosition.Value);
                    ChaseMode();
                }
                break;
            case EnemyState.EnemyState_Moving:
                MoveToTarget();
                MovingUpdate();
                break;
            case EnemyState.EnemyState_Attacking:
                MoveToTarget();
                AttackUpdate();
                break;
            default:
                break;
        }
    }
    #endregion
}
