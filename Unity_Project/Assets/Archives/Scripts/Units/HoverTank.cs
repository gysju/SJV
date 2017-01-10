using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Units/Hover Tank")]
public class HoverTank : MobileGroundUnit
{
    [Header("Turret specifics")]
    public Transform m_turretBase;

    [Tooltip("Rotation speed in degrees of the turret")]
    [Range(0.1f, 360.0f)]
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

    #region Initialisation
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
    #endregion

    #region Attack Related
    private void PointTurretAt(Vector3 target)
    {
        Quaternion qTurret;
        Quaternion qGun;

        float distanceToTarget = Vector3.Dot(m_turretBase.transform.up, target - m_turretBase.position);
        Vector3 planePoint = target - m_turretBase.transform.up * distanceToTarget;

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

    public override void AimWeaponAt(Vector3 target)
    {
        PointTurretAt(target);
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        if (!m_destroyed)
        {
            base.Update();
        }
    }
    #endregion

	#region Destroy

	void OnDestroy()
	{
		
	}
	#endregion
}
