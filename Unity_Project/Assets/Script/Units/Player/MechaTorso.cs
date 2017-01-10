using UnityEngine;
using System.Collections;

public class MechaTorso : MonoBehaviour
{
    public Transform m_torsoTransform;
    public float m_torsoRotationSpeed = 0.5f;

    void Start()
    {
        m_torsoTransform = transform;
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
