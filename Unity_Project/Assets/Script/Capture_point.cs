using UnityEngine;
using System.Collections;

public class Capture_point : MonoBehaviour 
{
	public enum Capture_Point_state {Capture_Point_state_Neutre, Capture_Point_state_Loading, Capture_Point_state_Waiting, Capture_Point_state_Loaded}
	protected Capture_Point_state currentState = Capture_Point_state.Capture_Point_state_Neutre ;

	public enum Capture_Point_type { Capture_Point_type_TriggerZone, Capture_Point_type_Activation }
	public Capture_Point_type type = Capture_Point_type.Capture_Point_type_TriggerZone;

	[Range(0.1f, 10.0f)]
	public float Range = 5.0f;

	[Range(1.0f, 10.0f)]
	public float TimeToCapture = 1.0f;

	private Unit.UnitFaction faction = Unit.UnitFaction.Neutral;
	private float time = 0.0f;

	void Start () 
	{
		SetCollider (type);
	}

	void Update () 
	{
		UpdateState (currentState);	
		Debug.Log (time);
	}

	void SetCollider( Capture_Point_type state)
	{
		if(state == Capture_Point_type.Capture_Point_type_TriggerZone)
		{
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.radius = Range;
			sphereCollider.isTrigger = true;
		}
		else
		{
			BoxCollider boxCollider = gameObject.AddComponent <BoxCollider> ();
			boxCollider.size = Vector3.one * Range /2.0f;
			boxCollider.center += transform.forward * (boxCollider.size.x / 2.0f) + new Vector3( 0, boxCollider.size.x / 2.0f ,0);
			boxCollider.isTrigger = true;
		}
	}

	void UpdateState( Capture_Point_state state )
	{
		switch(state)
		{
			case Capture_Point_state.Capture_Point_state_Neutre:
				break;
			case Capture_Point_state.Capture_Point_state_Loading:
				CaptureThePoint ();
				break;
			case Capture_Point_state.Capture_Point_state_Waiting:
				break;
			case Capture_Point_state.Capture_Point_state_Loaded:
				break;
		}
	}

	void ChangeState( Capture_Point_state state )
	{
		Material mat = this.GetComponent<Renderer> ().material;

		// exit a state
		switch(currentState)
		{
			case Capture_Point_state.Capture_Point_state_Neutre:
				break;
			case Capture_Point_state.Capture_Point_state_Loading:
				break;
			case Capture_Point_state.Capture_Point_state_Waiting:
				break;
			case Capture_Point_state.Capture_Point_state_Loaded:
				break;
		}

		currentState = state;

		// enter in new state
		switch(currentState)
		{
		case Capture_Point_state.Capture_Point_state_Neutre:
			mat.color = Color.gray;
			time = 0.0f;
			break;
		case Capture_Point_state.Capture_Point_state_Loading:
			mat.color = Color.blue;
			break;
		case Capture_Point_state.Capture_Point_state_Waiting:
			mat.color = Color.red;
			break;
		case Capture_Point_state.Capture_Point_state_Loaded:
			mat.color = Color.green;
			break;
		}
	}

	void CaptureThePoint()
	{
		time += Time.deltaTime;

		if(time >= TimeToCapture)
		{
			ChangeState (Capture_Point_state.Capture_Point_state_Loaded);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		Unit unit = col.GetComponent<Unit> ();

		if(type == Capture_Point_type.Capture_Point_type_TriggerZone)
		{
			if(unit  != null && currentState == Capture_Point_state.Capture_Point_state_Neutre )
			{
				faction = unit.m_faction;
				ChangeState(Capture_Point_state.Capture_Point_state_Loading);
			}
			else if (unit  != null && currentState == Capture_Point_state.Capture_Point_state_Loading)
			{
				ChangeState(Capture_Point_state.Capture_Point_state_Waiting);
			}
		}	
	}

	void OnTriggerExit(Collider col)
	{
		Unit unit = col.GetComponent<Unit> ();

		if (type == Capture_Point_type.Capture_Point_type_TriggerZone) 
		{
			if (unit != null && currentState == Capture_Point_state.Capture_Point_state_Loading) 
			{
				ChangeState (Capture_Point_state.Capture_Point_state_Neutre);
			} 
			else if (unit != null && currentState == Capture_Point_state.Capture_Point_state_Waiting) 
			{
				if (unit.m_faction == faction) 
				{
					time = 0.0f;
                    faction = (faction == Unit.UnitFaction.Ally) ? Unit.UnitFaction.Enemy : Unit.UnitFaction.Ally;
                }
				ChangeState (Capture_Point_state.Capture_Point_state_Loading);
			}
		}
	}
}