using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MechaLegs : MonoBehaviour
{
    public static MechaLegs Instance = null;

    protected Transform m_legsTransform;

    protected NavMeshAgent m_navmeshAgent;

    public float m_speed;

    void Start ()
    {
        if (Instance == null)
        {
            Instance = this;
            m_legsTransform = transform;
            m_navmeshAgent = m_legsTransform.GetComponentInParent<NavMeshAgent>();
            if (!m_navmeshAgent)
                m_navmeshAgent = m_legsTransform.GetComponentInParent<BaseMecha>().gameObject.AddComponent<NavMeshAgent>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void MoveTo(Vector3 direction)
    {
        m_navmeshAgent.Move(direction * m_speed);
    }
	
	void Update ()
    {
		
	}
}
