using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MechaLegs : MonoBehaviour
{
    public static MechaLegs Instance = null;

    protected BaseMecha m_baseMecha;

    protected Transform m_legsTransform;

    public NavMeshAgent m_navmeshAgent;
    protected NavMeshHit m_navMeshHit;

    [Header("Movement")]
    public float m_speed;

    [Header("Dash")]
    [Range(0.1f, 1.0f)]
    public float DashSpeed = 0.5f;

    public enum MoveSystem { moveSystem_teleport = 0, moveSystem_dash, moveSystem_count };
    public MoveSystem moveSystem = MoveSystem.moveSystem_teleport;

    void Start ()
    {
        if (Instance == null)
        {
            Instance = this;
            m_legsTransform = transform;
            m_baseMecha = m_legsTransform.GetComponentInParent<BaseMecha>();
            m_navmeshAgent = m_legsTransform.GetComponentInParent<NavMeshAgent>();
            if (!m_navmeshAgent)
                m_navmeshAgent = m_baseMecha.gameObject.AddComponent<NavMeshAgent>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public bool CheckDestination(RaycastHit hit)
    {
        return NavMesh.SamplePosition(hit.point, out m_navMeshHit, 2.0f, 1 << NavMesh.GetAreaFromName("Walkable"));
    }

    public void SwitchMoveSystem()
    {
        moveSystem = (MoveSystem)(((int)moveSystem + 1) % (int)MoveSystem.moveSystem_count);
    }

    public void ConfirmTeleport()
    {
        if (m_navMeshHit.hit)
        {
            switch (moveSystem)
            {
                case MoveSystem.moveSystem_teleport:
                    Teleport(m_navMeshHit.position);
                    break;
                case MoveSystem.moveSystem_dash:
                    InitDash(m_navMeshHit.position);
                    break;
                default:
                    break;
            }
        }
    }

    private void Teleport(Vector3 pos)
    {
        m_baseMecha.m_transform.position = pos;
    }

    private void InitDash(Vector3 pos)
    {
        StartCoroutine(Dash(pos));
    }

    IEnumerator Dash(Vector3 pos)
    {
        Vector3 initialPos = transform.position;
        float time = 0.0f;
        while (time < DashSpeed)
        {
            m_baseMecha.m_transform.position = Vector3.Lerp(initialPos, pos, time / DashSpeed);
            time += Time.deltaTime;
            yield return null;
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
