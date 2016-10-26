using UnityEngine;
using System.Collections;

public class IA : MonoBehaviour
{
    Unit m_unit;

    bool m_combatUnit;
    bool m_mobileUnit;

	void Start ()
    {
        m_unit = transform.GetComponent<Unit>();
	}
	
	void Update ()
    {
	    
	}
}
