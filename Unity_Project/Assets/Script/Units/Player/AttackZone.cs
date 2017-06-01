using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public LayerMask mask;
    public int collidersNbr { get; protected set; }
    public bool clearView;

    public Transform m_transform;
    protected BaseMecha m_player;
    protected 

    void Start()
    {
        m_transform = transform;
        m_player = BaseMecha.instance;
        collidersNbr = 0;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer != mask)
        {
            collidersNbr++;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer != mask)
        {
            collidersNbr--;
        }
    }

    void Update()
    {
        clearView = !(Physics.Raycast(m_transform.position, (m_player.m_torso.transform.position - m_transform.position).normalized, Vector3.Distance(m_player.m_transform.position, m_transform.position), mask));
    }
}
