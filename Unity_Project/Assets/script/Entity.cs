using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    public float Life = 1.0f;

    public enum Entity_State { Entity_State_Attack = 0, Entity_State_Waiting, Entity_State_MovedToTarget, Entity_State_Died, Count}
    protected Entity_State currentEntityState = Entity_State.Entity_State_Waiting;

    protected virtual void Start ()
    {

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
                break;
            case Entity_State.Entity_State_Died:
                break;
            case Entity_State.Entity_State_MovedToTarget:
                break;
            case Entity_State.Entity_State_Waiting:
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
                break;
            case Entity_State.Entity_State_Died:
                Die();
                break;
            case Entity_State.Entity_State_MovedToTarget:
                break;
            case Entity_State.Entity_State_Waiting:
                break;
        }
    }

    void onExitState()
    {
        switch (currentEntityState)
        {
            case Entity_State.Entity_State_Attack:
                break;
            case Entity_State.Entity_State_Died:
                break;
            case Entity_State.Entity_State_MovedToTarget:
                break;
            case Entity_State.Entity_State_Waiting:
                break;
        }
    }

    public void ReceiveDamages( float amount)
    {
        Life -= amount;
        if (Life <= 0.0f)
        {
            SwitchState(Entity_State.Entity_State_Died);
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
