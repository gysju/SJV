using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Units/Turret")]
public class Turret : CombatUnit
{
    [Header("Turret specifics")]
    public Transform m_turretBase;

    [Tooltip("Rotation speed in degrees of the turret")]
    [Range(0.1f,360.0f)]
    public float m_turretDegreesPerSecond = 45.0f;

    [Tooltip("Rotation speed in degrees of the cannon")]
    [Range(0.1f, 360.0f)]
    public float m_cannonDegreesPerSecond = 45.0f;

    [Tooltip("Turret's cannon's max angle.")]
    [Range(0.0f, 180f)]
    public float m_maxCannonAngle = 45.0f;

    [Tooltip("Turret's imprecision angle.")]
    [Range(0.1f, 10.0f)]
    public float m_imprecisioAngle = 10.0f;

    protected override void Awake()
	{
        base.Awake();
	}

    protected override void Start () 
	{
        base.Start();
    }

    #region Targeting Related
    protected void TargetClosestEnemy()
    {
        if (m_detectedEnemies.Count > 0)
        {
            for (int i = m_detectedEnemies.Count - 1; i > -1; i--)
            {
                Unit potentialTarget = m_detectedEnemies[i];
                if (potentialTarget)
                {
                    if (!m_currentTarget) m_currentTarget = potentialTarget;
                    else
                    {
                        float currentTargetDistance = Vector3.Distance(m_currentTarget.m_targetPoint.position, transform.position);
                        float potentialTargetDistance = Vector3.Distance(potentialTarget.m_targetPoint.position, transform.position);

                        if (potentialTargetDistance < currentTargetDistance) m_currentTarget = potentialTarget;
                    }
                }
                else
                {
                    m_detectedEnemies.Remove(potentialTarget);
                }
            }

            //foreach (Unit potentialTarget in m_detectedEnemies)
            //{
            //    if (potentialTarget)
            //    {
            //        if (!m_currentTarget) m_currentTarget = potentialTarget;
            //        else
            //        {
            //            float currentTargetDistance = Vector3.Distance(m_currentTarget.m_targetPoint.position, transform.position);
            //            float potentialTargetDistance = Vector3.Distance(potentialTarget.m_targetPoint.position, transform.position);

            //            if (potentialTargetDistance < currentTargetDistance) m_currentTarget = potentialTarget;
            //        }
            //    }
            //    else
            //    {
            //        m_detectedEnemies.Remove(potentialTarget);
            //    }
            //}
        }
        else
        {
            m_currentTarget = null;
            CeaseFire();
        }
    }

    protected override void CheckCurrentTarget()
    {
        base.CheckCurrentTarget();

        TryAttack();
    }
    #endregion

        #region Attack Related
    private void AimTarget()
	{
        Quaternion qTurret;
        Quaternion qGun;

        float distanceToTarget = Vector3.Dot(m_turretBase.transform.up, m_currentTarget.m_targetPoint.position - m_turretBase.position);
        Vector3 planePoint = m_currentTarget.m_targetPoint.position - m_turretBase.transform.up * distanceToTarget;

        qTurret = Quaternion.LookRotation(planePoint - m_turretBase.position, transform.up);
        m_turretBase.rotation = Quaternion.RotateTowards(m_turretBase.rotation, qTurret, m_turretDegreesPerSecond * Time.deltaTime);

        Vector3 v3 = new Vector3(0.0f, distanceToTarget, (planePoint - m_turretBase.position).magnitude);
        qGun = Quaternion.LookRotation(v3);

        foreach (Weapon weapon in m_weapons)//le pivot de l'arme doit être au point d'ancrage
        {
            if (Quaternion.Angle(weapon.transform.localRotation, qGun) <= m_maxCannonAngle)
                weapon.transform.localRotation = Quaternion.RotateTowards(weapon.transform.localRotation, qGun, m_cannonDegreesPerSecond * Time.deltaTime);
        }
    }

    private bool IsTargetInAim(Weapon weapon)
    {
        Vector3 targetDir = m_currentTarget.m_targetPoint.position - weapon.m_muzzle.position;
        float angle = Vector3.Angle(targetDir, weapon.m_muzzle.forward);

        if (angle <= m_imprecisioAngle)
            return true;

        return false;
    }

    private void Shoot()
	{
        foreach (Weapon weapon in m_weapons)
        {
            if (IsTargetInAim(weapon) && weapon.IsTargetInOptimalRange(m_currentTarget.m_targetPoint.position) && m_currentTarget)
            {
                weapon.TriggerPressed();
            }
            else weapon.TriggerReleased();
        }
	}

    protected void TryAttack()
    {
        if (m_currentTarget)
        {
            AimTarget();
            Shoot();
        }
    }

    protected void CeaseFire()
    {
        foreach (Weapon weapon in m_weapons)
        {
            weapon.TriggerReleased();
        }
    }
    #endregion
    
    #region IA Related
    protected void IA()
    {
        TargetClosestEnemy();
        CheckCurrentTarget();
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        if(!m_destroyed)
        {
            base.Update();
            IA();
        }
    }
    #endregion
}
