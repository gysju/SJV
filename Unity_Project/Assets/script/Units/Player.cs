using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Camera m_mainCamera;
    public Unit m_meca;

    public Weapon m_leftWeapon;
    public Weapon m_rightWeapon;

    void Start()
    {
        m_mainCamera = Camera.main;
    }

	void Update ()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
        m_mainCamera.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y"));

        if (Input.GetMouseButtonDown(0))
            m_leftWeapon.TriggerPressed();
        if (Input.GetMouseButtonDown(1))
            m_rightWeapon.TriggerPressed();

        if (Input.GetMouseButtonUp(0))
            m_leftWeapon.TriggerReleased();
        if (Input.GetMouseButtonUp(1))
            m_rightWeapon.TriggerReleased();
    }
}
