using UnityEngine;
using System.Collections;

public class Laser : Ammo
{
    public float m_range = 50f;
    public bool m_continuous = false;

    protected override void Awake()
    {
        base.Awake();
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
            GetComponent<LineRenderer>().SetPosition(1, hit.point);
        }
        else
        {
            GetComponent<LineRenderer>().SetPosition(1, transform.position + transform.forward * m_range);
        }
    }

    void Update ()
    {
	
	}
}
