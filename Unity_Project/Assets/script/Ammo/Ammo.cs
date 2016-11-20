using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour
{
    public GameObject m_metalHit;
    public GameObject m_dirtHit;

    [Header("Damages")]
    [Tooltip("Ammo's damages on hit.")]
    public int m_directDamages;
    [Tooltip("Armor the ammo ignore.")]
    public int m_armorPenetration;

    [Header("Ammo's specs")]
    [Tooltip("Is the ammo explosive.")]
    public bool m_explosive;
    public GameObject m_explosionPrefab;

    void Start ()
    {

    }
	
	void Update ()
	{
		
	}
}
