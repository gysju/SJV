using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    
    public int collidersNbr{ get; protected set; }

    public Transform m_transform;

    void Start()
    {
        m_transform = transform;
        collidersNbr = 0;
    }

    void OnTriggerEnter(Collider col)
    {
        collidersNbr++;
    }

    void OnTriggerExit(Collider col)
    {
        collidersNbr--;
    }
}
