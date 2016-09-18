using UnityEngine;
using System.Collections;

public class Enemy : Entity {

	protected override void Start ()
    {
        base.Start();
	}
	
	protected override void Update ()
    {
        base.Update();
	}

    protected override void OnAttackEnter()
    {
        base.OnAttackEnter();
    }

    protected override void OnAttackUpdate()
    {
        base.OnAttackUpdate();
    }

    protected override void OnAttackExit()
    {
        base.OnAttackExit();
    }

    protected override void OnMoveEnter()
    {
        base.OnMoveEnter();
    }

    protected override void OnMoveUpdate()
    {
        base.OnMoveUpdate();
    }

    protected override void OnMoveExit()
    {
        base.OnMoveExit();
    }

    protected override void OnWaitingEnter()
    {
        base.OnWaitingEnter();
    }

    protected override void OnWaitingUpdate()
    {
        base.OnWaitingUpdate();
    }

    protected override void OnWaitingExit()
    {
        base.OnWaitingExit();
    }

    protected override void OnDeathEnter()
    {
        base.OnDeathEnter();
        Destroy(gameObject);
    }

    protected override void OnDeathUpdate()
    {
        base.OnDeathUpdate();
    }
}
