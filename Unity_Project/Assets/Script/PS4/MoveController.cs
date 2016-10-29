using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_PS4
using UnityEngine.PS4;

public class MoveController : MonoBehaviour {

	public enum MoveButton { MoveButton_Trigger = 2, MoveButton_Move = 4, MoveButton_Start = 8, MoveButton_Triangle = 16, MoveButton_Circle = 32, MoveButton_Cross = 64, MoveButton_Square = 128, MoveButton_MaxAnalogueValue = 255, MoveButton_Count = 8}

    public bool isMoveController = false;
    public bool isSecondaryMoveController = false;

	private List< MoveButton > ButtonsUp = new List<MoveButton>();
	private List< MoveButton > ButtonsDown = new List<MoveButton>();

	private List< MoveButton > ButtonsUpCurrentFrame = new List<MoveButton>();
	private List< MoveButton > ButtonsDownCurrentFrame = new List<MoveButton>();

	void Start () 
	{
	
	}
	
	void Update () {
	
	}

	void LateUpdate()
	{
		TransferInput ();
	}

	public bool GetButton( MoveButton button )
    {
		for (int slot = 0; slot < 4; slot++) 
		{
			if (PS4Input.MoveIsConnected (slot, 0) && !isSecondaryMoveController && PS4Input.MoveGetButtons (slot, 0) == (int)button) 
			{
				return true;
			} 
			else if (PS4Input.MoveIsConnected (slot, 1) && PS4Input.MoveGetButtons (slot, 1) == (int)button) 
			{	
				return true;
			}
		}
		return false;
    }

	public bool GetButtonUp(MoveButton button)
	{

		foreach(MoveButton moveButton in ButtonsUp)
		{
			if (moveButton == button)
				return false;
		}
		if (GetButton (button)) 
		{
			ButtonsUpCurrentFrame.Add ( button );
			return true;
		}
		return false;
	}

	public bool GetButtonDown(MoveButton button)
	{
		foreach(MoveButton moveButton in ButtonsDown)
		{
			if (moveButton == button)
				return false;
		}
		if (GetButton (button)) 
		{
			ButtonsDownCurrentFrame.Add ( button );
			return true;
		}
		return false;
	}

	void TransferInput()
	{
		ButtonsUp.Clear();
		ButtonsDown.Clear();

		foreach(MoveButton buttonCurrentFrame in ButtonsUpCurrentFrame)
		{
			bool test = false;
			foreach(MoveButton button in ButtonsUp)
			{
				if (buttonCurrentFrame == button)
					test = true;
			}
			if (!test) 
			{
				ButtonsUp.Add(buttonCurrentFrame);
			}
		}

		foreach(MoveButton buttonCurrentFrame in ButtonsDownCurrentFrame)
		{
			bool test = false;
			foreach(MoveButton button in ButtonsDown)
			{
				if (buttonCurrentFrame == button)
					test = true;
			}
			if (!test) 
			{
				ButtonsDown.Add(buttonCurrentFrame);
			}
		}
		ButtonsDownCurrentFrame.Clear();
		ButtonsUpCurrentFrame.Clear();
	}

	public Vector3 getMoveRotation()
	{
		if(!isSecondaryMoveController)
			return PS4Input.GetLastMoveAcceleration (0, 0);
		else
			return PS4Input.GetLastMoveAcceleration (0, 1);
	}
}
#endif
