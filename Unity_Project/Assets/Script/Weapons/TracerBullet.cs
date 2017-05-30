﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerBullet : MonoBehaviour
{
    public float m_speed;
    protected Vector3 m_target;
    protected Transform m_transform;
    protected TrailRenderer trail;

    void Start()
    {
        m_transform = transform;
        m_target = m_transform.position;
        trail = GetComponent<TrailRenderer>();
    }

    public void ResetTracer()
    {
        m_transform.localPosition = Vector3.zero;
        trail.Clear();
    }

    public void Use(Vector3 spawn, Vector3 target)
    {
        ResetTracer();
        m_transform.position = spawn;
        m_target = target;
    }
    
    void Update()
    {
        m_transform.position = Vector3.MoveTowards(m_transform.position, m_target, m_speed);
    }
}
