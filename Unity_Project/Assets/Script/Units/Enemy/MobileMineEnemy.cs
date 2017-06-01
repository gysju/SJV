using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileMineEnemy : GroundEnemy
{
    [Header("Explosive")]
    public float m_explosionRange = 2f;
    public int m_damages = 2;

    #region Initialization
    protected override void Start()
    {
        m_weaponsTarget = FindObjectOfType<BaseMecha>().transform;
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
        target.ReceiveDamages(m_damages, 1);
        CompleteStop();
        m_destroyed = true;
        HUD_Radar.Instance.RemoveInfo(this);
        FinishDying();

        SoundManager.Instance.PlaySoundOnShot("mecha_kamikaze_explosion", audioSource);
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        if (!m_destroyed)
        {
            switch (m_enemyState)
            {
                case EnemyState.EnemyState_Sleep:
                    if (m_weaponsTarget)
                    {
                        MoveToTarget();
                    }
                    break;
                case EnemyState.EnemyState_Moving:
                    if (IsTargetInRange())
                    {
                        Explode(m_weaponsTarget.GetComponent<BaseUnit>());
                    }
                    break;
                case EnemyState.EnemyState_Attacking:
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
}
