using UnityEngine;
using System.Collections;

public class Capture_point : MonoBehaviour 
{
    public enum Capture_Type { Capture_type_Tactical, Capture_type_Ressources, Capture_type_Health }
    public Capture_Type type = Capture_Type.Capture_type_Tactical;

	public enum Capture_Mode { Capture_Mode_TriggerZone, Capture_Mode_Activation }
	public Capture_Mode mode = Capture_Mode.Capture_Mode_TriggerZone;

    public enum Capture_State {Capture_Point_state_Neutre, Capture_Point_state_Loading, Capture_Point_state_Waiting, Capture_Point_state_Loaded}
	protected Capture_State currentState = Capture_State.Capture_Point_state_Neutre ;

	[Range(0.1f, 10.0f)]
	public float Range = 5.0f;

	[Range(1.0f, 10.0f)]
	public float TimeToCapture = 1.0f;

    private Unit.UnitFaction faction = Unit.UnitFaction.Neutral;
	private CombatUnit combatUnitOnTarget;
	private float time = 0.0f;

	void Start () 
	{
		SetCollider (mode);
	}
    #region Update
    void Update () 
	{
		UpdateState (currentState);	
	}
    #endregion
    #region State
    void UpdateState(Capture_State state )
	{
		switch(state)
		{
			case Capture_State.Capture_Point_state_Neutre:
				break;
			case Capture_State.Capture_Point_state_Loading:
				CaptureThePoint ();
				break;
			case Capture_State.Capture_Point_state_Waiting:
				break;
			case Capture_State.Capture_Point_state_Loaded:
				break;
		}
	}

	void ChangeState(Capture_State state )
	{
		Material mat = this.GetComponent<Renderer> ().material;

		// exit a state
		switch(currentState)
		{
			case Capture_State.Capture_Point_state_Neutre:
				break;
			case Capture_State.Capture_Point_state_Loading:
				break;
			case Capture_State.Capture_Point_state_Waiting:
				break;
			case Capture_State.Capture_Point_state_Loaded:
				break;
		}

		currentState = state;

		// enter in new state
		switch(currentState)
		{
		case Capture_State.Capture_Point_state_Neutre:
			mat.color = Color.gray;
			time = 0.0f;
			break;
		case Capture_State.Capture_Point_state_Loading:
			mat.color = Color.blue;
			break;
		case Capture_State.Capture_Point_state_Waiting:
			mat.color = Color.red;
			break;
		case Capture_State.Capture_Point_state_Loaded:
			mat.color = Color.green;
            AddBuffByType(combatUnitOnTarget);

            break;
		}
	}

    public bool IsSameFaction(Unit.UnitFaction otherFaction)
    {
        return (otherFaction == faction);
    }
    #endregion
    #region Actions

    void CaptureThePoint()
    {
        time += Time.deltaTime;

        if (time >= TimeToCapture)
        {
            ChangeState(Capture_State.Capture_Point_state_Loaded);
        }
    }

    void AddBuffByType( CombatUnit combatUnit )
    {
        switch ( type )
        {
            case Capture_Type.Capture_type_Tactical:
                // Call a GameManager.
                break;
            case Capture_Type.Capture_type_Ressources:
                // reload weapons, call a method in combat unit.
                break;
            case Capture_Type.Capture_type_Health:
                combatUnit.Repair(100);
                break;
        }
    }

    #endregion
    #region Collider
    void OnTriggerEnter(Collider col)
	{
		CombatUnit combatUnit = col.GetComponent<CombatUnit> ();

		if(combatUnit != null && mode == Capture_Mode.Capture_Mode_TriggerZone && !col.isTrigger)
        {
			if( currentState == Capture_State.Capture_Point_state_Neutre )
			{
                combatUnitOnTarget = combatUnit;
                faction = combatUnitOnTarget.m_faction;
				ChangeState(Capture_State.Capture_Point_state_Loading);
			}
			else if ( currentState == Capture_State.Capture_Point_state_Loading)
			{
				ChangeState(Capture_State.Capture_Point_state_Waiting);
			}
		}	
	}

	void OnTriggerExit(Collider col)
	{
        CombatUnit combatUnit = col.GetComponent<CombatUnit> ();

		if (combatUnit != null && mode == Capture_Mode.Capture_Mode_TriggerZone && !col.isTrigger) 
		{
			if ( currentState == Capture_State.Capture_Point_state_Loading) 
			{
				ChangeState (Capture_State.Capture_Point_state_Neutre);
			} 
			else if ( currentState == Capture_State.Capture_Point_state_Waiting) 
			{
				if (combatUnit.m_faction == faction) 
				{
					time = 0.0f;
                    faction = (faction == Unit.UnitFaction.Ally) ? Unit.UnitFaction.Enemy : Unit.UnitFaction.Ally;
                }

				ChangeState (Capture_State.Capture_Point_state_Loading);
			}
		}
	}

    void SetCollider(Capture_Mode mode)
    {
        if (mode == Capture_Mode.Capture_Mode_TriggerZone)
        {
            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = Range;
            sphereCollider.isTrigger = true;
        }
        else
        {
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = Vector3.one * Range / 2.0f;
            boxCollider.center += transform.forward * (boxCollider.size.x / 2.0f) + new Vector3(0, boxCollider.size.x / 2.0f, 0);
            boxCollider.isTrigger = true;
        }
    }
    #endregion
}