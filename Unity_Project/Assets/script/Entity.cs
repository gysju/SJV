using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    [Header("Stats")]
    public float Life = 1.0f;
    public float DetectionRange = 10.0f;

    public enum Entity_State { Entity_State_Attack = 0, Entity_State_Waiting, Entity_State_Move, Entity_State_Death, Count}
    protected Entity_State currentEntityState = Entity_State.Entity_State_Waiting;

    public Entity Target { get; private set; }
    protected NavMeshAgent navMeshAgent;

    protected virtual void Start ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update ()
    {
        UpdateState();
    }

    void UpdateState()
    {
        switch (currentEntityState)
        {
            case Entity_State.Entity_State_Attack:
                OnAttackUpdate();
                break;
            case Entity_State.Entity_State_Death:
                OnDeathUpdate();
                break;
            case Entity_State.Entity_State_Move:
                OnMoveUpdate();
                break;
            case Entity_State.Entity_State_Waiting:
                OnWaitingUpdate();
                break;
        }
    }

    void SwitchState( Entity_State newState)
    {
        if (currentEntityState != newState)
        {
            onExitState();
            currentEntityState = newState;
            OnEnterState();
        }
    }

    void OnEnterState()
    {
        switch (currentEntityState)
        {
            case Entity_State.Entity_State_Attack:
                OnAttackEnter();
                break;
            case Entity_State.Entity_State_Death:
                OnDeathEnter();
                break;
            case Entity_State.Entity_State_Move:
                OnMoveEnter();
                break;
            case Entity_State.Entity_State_Waiting:
                OnWaitingEnter();
                break;
        }
    }

    void onExitState()
    {
        switch (currentEntityState)
        {
            case Entity_State.Entity_State_Attack:
                OnAttackEnter();
                break;
            case Entity_State.Entity_State_Death:
                OnDeathEnter();
                break;
            case Entity_State.Entity_State_Move:
                OnMoveEnter();
                break;
            case Entity_State.Entity_State_Waiting:
                OnWaitingEnter();
                break;
        }
    }

    public void ReceiveDamages( float amount)
    {
        Life -= amount;
        if (Life <= 0.0f)
        {
            SwitchState(Entity_State.Entity_State_Death);
        }
    }

    protected virtual void OnAttackEnter()
    {

    }

    protected virtual void OnAttackUpdate()
    {

    }

    protected virtual void OnAttackExit()
    {

    }

    protected virtual void OnMoveEnter()
    {

    }

    protected virtual void OnMoveUpdate()
    {

    }

    protected virtual void OnMoveExit()
    {

    }

    protected virtual void OnWaitingEnter()
    {

    }

    protected virtual void OnWaitingUpdate()
    {

    }

    protected virtual void OnWaitingExit()
    {

    }

    protected virtual void OnDeathEnter()
    {

    }

    protected virtual void OnDeathUpdate()
    {

    }

    protected virtual void OnDeathExit()
    {

    }
}
