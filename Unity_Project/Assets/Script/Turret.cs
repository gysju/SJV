using UnityEngine;
using System.Collections;

public class Turret : CombatUnit
{
    [Header("Turret specifics")]
    public Transform m_turretBase;

    [Range(0.1f,10.0f)]
    public float m_aimingSpeed = 1.0f;
    [Range(0.1f, 10.0f)]
    public float m_precision = 10.0f;

    protected override void Awake()
	{
        base.Awake();
	}

    protected override void Start () 
	{
        base.Start();
    }

    #region Targeting Related
    protected void ChooseTarget()
    {
        if (m_possibleTargets.Count > 0)
        {
            foreach (Unit potentialTarget in m_possibleTargets)
            {
                if (!m_currentTarget) m_currentTarget = potentialTarget;
                else
                {
                    float currentTargetDistance = Vector3.Distance(m_currentTarget.transform.position, transform.position);
                    float potentialTargetDistance = Vector3.Distance(potentialTarget.transform.position, transform.position);

                    if (potentialTargetDistance < currentTargetDistance) m_currentTarget = potentialTarget;
                }
            }
        }
        else m_currentTarget = null;
    }
    #endregion

    #region Attack Related
    private void AimTarget()
	{
        Quaternion dir = Quaternion.LookRotation(m_currentTarget.transform.position - m_turretBase.position);
        dir.eulerAngles = new Vector3(0f, dir.eulerAngles.y, 0f);
        m_turretBase.rotation = Quaternion.Lerp(m_turretBase.rotation, dir, Time.deltaTime * m_aimingSpeed);

        //foreach (Weapon weapon in m_weapons)
        //{
        //    dir = Quaternion.LookRotation(m_currentTarget.transform.position - weapon.transform.position);
        //    dir.eulerAngles = new Vector3(dir.eulerAngles.x, 0f, 0f);
        //    weapon.transform.rotation = Quaternion.Lerp(weapon.transform.rotation, dir, Time.deltaTime * m_aimingSpeed);
        //}
    }

    private bool IsTargetInAim(Weapon weapon)
    {
        //RaycastHit hit;
        //if (Physics.Raycast(weapon.m_muzzle.position, weapon.m_muzzle.forward, out hit))
        //{
        //    Unit unit = hit.collider.GetComponent<Unit>();

        //    if (unit == m_currentTarget)
        //        return true;
        //}

        Vector3 targetDir = m_currentTarget.transform.position - weapon.m_muzzle.position;
        float angle = Vector3.Angle(targetDir, weapon.m_muzzle.forward);

        if (angle <= m_precision)
            return true;

        return false;
    }

    private void Shoot()
	{
        foreach (Weapon weapon in m_weapons)
        {
            if (IsTargetInAim(weapon) && weapon.IsTargetInOptimalRange(m_currentTarget.transform.position))
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
    #endregion

    #region Updates
    protected override void Update()
    {
        base.Update();

        ChooseTarget();
        TryAttack();
    }
    #endregion
}
