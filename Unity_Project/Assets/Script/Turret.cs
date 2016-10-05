using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	private Weapon weapon;

	[Range(0.1f, 15.0f)]
	public float DetectionRange = 5;
	private SphereCollider DetectionSphere;

	public enum Turret_state {Turret_state_Idle, Turret_state_Detected, Turret_state_Shoot}
	public Turret_state currentState = Turret_state.Turret_state_Idle;
	public Unit.UnitFaction m_faction;

    [Range(0.1f,10.0f)]
    public float AimingSpeed = 1.0f;

	private Unit target = null;
	private Transform Base_canon;
    private Quaternion dir = Quaternion.identity;

    void Awake()
	{
		weapon = GetComponentInChildren<Weapon>();
		DetectionSphere = GetComponent<SphereCollider> ();
		Base_canon = transform.FindChild ("Base_canon");
	}

	void Start () 
	{
		DetectionSphere.radius = DetectionRange;
	}
	
	void Update () 
	{
		switch(currentState)
		{
			case Turret_state.Turret_state_Idle:
				break;
			case Turret_state.Turret_state_Detected:
				Aim ();
				break;
			case Turret_state.Turret_state_Shoot:
				Shoot();	
				break;
		}
	}

	void Aim()
	{
        dir = Quaternion.LookRotation(target.transform.position - Base_canon.position);
        Base_canon.rotation = Quaternion.Lerp(Base_canon.rotation, dir, Time.deltaTime * AimingSpeed);

        if (CheckDirection())
            currentState = Turret_state.Turret_state_Shoot;
    }

    void Shoot()
	{
		weapon.TriggerPressed ();

		if(!CheckDirection())
		{
			weapon.TriggerReleased();
            currentState = Turret_state.Turret_state_Detected;
		}
	}

    bool CheckDirection()
    {
        RaycastHit hit;
        if (Physics.Raycast(weapon.m_muzzle.position, weapon.m_muzzle.forward, out hit))
        {
            Unit unit = hit.collider.GetComponent<Unit>();

            if (unit == target)
                return true;
        }

        return false;
    }

	void OnTriggerEnter(Collider col)
	{
		Unit unit = col.GetComponent<Unit> ();
		if(unit  != null && unit.m_faction != m_faction)
		{
            currentState = Turret_state.Turret_state_Detected;
			target = unit;
        }
	}

	void OnTriggerExit(Collider col)
	{
		Unit unit = col.GetComponent<Unit> ();
		if(unit  != null && unit.m_faction != m_faction)
		{
			currentState = Turret_state.Turret_state_Idle;
			target = null;
		}
	}
}
