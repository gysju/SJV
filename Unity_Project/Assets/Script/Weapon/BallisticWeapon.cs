using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallisticWeapon : Weapon
{

    [Tooltip("Ammo type the weapon fire.")]
    public BallisticAmmo m_ammo;

    protected List<GameObject> m_ammoPool = new List<GameObject>();

    void Start ()
	{
        Debug.Assert(m_ammo, "No ammo assigned to weapon");
	}

    public override void FireWeapon()
    {
        GameObject newAmmo = (GameObject) Instantiate(m_ammo, m_muzzle.position, m_muzzle.rotation * GetSpread());
        m_ammoPool.Add(newAmmo);
        Instantiate(m_muzzleFlash, m_muzzle.position, m_muzzle.rotation);
    }

    void Update ()
	{
		
	}
}
