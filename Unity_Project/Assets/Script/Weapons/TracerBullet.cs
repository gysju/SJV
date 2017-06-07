using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerBullet : MonoBehaviour
{
    public float m_speed;
    protected bool active;
    protected Vector3 m_target;
    protected Transform m_transform;
    protected TrailRenderer trail;
    protected float time = 0f;

    void Start()
    {
        m_transform = transform;
        trail = GetComponent<TrailRenderer>();
        ResetTracer();
    }

    public void ResetTracer()
    {
        active = false;
        time = 0f;
        m_transform.localPosition = Vector3.zero;
        m_target = m_transform.position;
        trail.Clear();
        trail.enabled = false;
    }

    public void Use(Vector3 spawn, Vector3 target)
    {
        ResetTracer();
        active = true;
        m_transform.position = spawn;
        m_target = target;
        trail.enabled = true;
    }
    
    void Update()
    {
        if (active)
        {
            time += Time.deltaTime;
            m_transform.position = Vector3.MoveTowards(m_transform.position, m_target, m_speed);
            if (m_transform.position == m_target || time >= 0.5f)
            {
                ResetTracer();
            }
        }
    }
}
