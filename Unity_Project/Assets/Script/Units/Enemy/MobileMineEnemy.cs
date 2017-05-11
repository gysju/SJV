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
        m_target = FindObjectOfType<BaseMecha>().transform;
    }
    #endregion

    #region HitPoints Related
    /// <summary>A appeler à la mort de l'unité.</summary>
    protected override void StartDying()
    {
        base.StartDying();

        // play death sound
        //SoundManager.Instance.PlaySoundOnShot("", audioSource);
    }
    #endregion
    #region Attack related
    protected bool TargetInRange()
    {
        return (Vector3.Distance(m_transform.position, m_target.position) < m_explosionRange);
    }

    protected void Explode(BaseUnit target)
    {
        target.ReceiveDamages(m_damages, 1);
        CompleteStop();
        m_destroyed = true;
        FinishDying();

        // play Attack sound
        //SoundManager.Instance.PlaySoundOnShot("", audioSource);
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
                    if (m_target)
                    {
                        MoveTo(m_target.position);
                    }
                    break;
                case EnemyState.EnemyState_Moving:
                    if (TargetInRange())
                    {
                        Explode(m_target.GetComponent<BaseUnit>());
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
