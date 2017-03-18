﻿using UnityEngine;
using System.Collections;

public class MechaTorso : MonoBehaviour
{
	public static MechaTorso Instance = null;

    protected Transform m_torsoTransform;
    public bool m_torsoConnected;
    public float m_torsoRotationSpeed = 0.5f;

    public bool m_weaponsConnected;

    void Start()
    {
		if( Instance == null )
		{
			Instance = this;
			m_torsoTransform = transform;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
    }
    
    protected void RotateTorsoHorizontaly(float horizontalAngle)
    {
        Quaternion currentRotation = m_torsoTransform.rotation;
        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
        m_torsoTransform.rotation = horizontalRotation * currentRotation;
    }

    public void RotateRight()
    {
        RotateTorsoHorizontaly(m_torsoRotationSpeed);
    }

    public void RotateLeft()
    {
        RotateTorsoHorizontaly(-m_torsoRotationSpeed);
    }
}
