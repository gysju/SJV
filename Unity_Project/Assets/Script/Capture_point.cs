using UnityEngine;
using System.Collections;

public class Capture_point : MonoBehaviour 
{
	public enum Capture_Point_state {Capture_Point_state_Neutre, Capture_Point_state_Loading, Capture_Point_state_Waiting, Capture_Point_state_Loaded}
	public Capture_Point_state currentState = Capture_Point_state.Capture_Point_state_Neutre ;

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
		GetComponent<SphereCollider> ().radius = Range;
	}

	void Update () 
	{
		UpdateState (currentState);	
		Debug.Log (time);
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

	void OnTriggerExit(Collider col)
	{
		Unit unit = col.GetComponent<Unit> ();

		if(unit != null && currentState == Capture_Point_state.Capture_Point_state_Loading )
		{
			ChangeState (Capture_Point_state.Capture_Point_state_Neutre);
		}
		else if (unit != null && currentState == Capture_Point_state.Capture_Point_state_Waiting)
		{
			if( unit.m_faction == faction)
			{
				time = 0.0f;
				faction = (faction == Unit.UnitFaction.FirstTeam) ? Unit.UnitFaction.SecondTeam: Unit.UnitFaction.FirstTeam; 
			}
			ChangeState (Capture_Point_state.Capture_Point_state_Loading);
		}
	}
}