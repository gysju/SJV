using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_PS4
using UnityEngine.PS4;

public class MoveController : MonoBehaviour {

	public enum MoveButton { MoveButton_Trigger = 2, MoveButton_Move = 4, MoveButton_Start = 8, MoveButton_Triangle = 16, MoveButton_Circle = 32, MoveButton_Cross = 64, MoveButton_Square = 128, MoveButton_MaxAnalogueValue = 255, MoveButton_Count = 8}

    public bool isMoveController = false;
    public bool isSecondaryMoveController = false;

	private int currentButtons = 0;
	private int prevButtons = 0;

	void Start () 
	{
	
	}
	
	void Update () {
	
	}

	void LateUpdate()
	{
		currentButtons = 0;
	}

	public bool GetButton( MoveButton button )
	{
		for (int slot = 0; slot < 4; slot++) 
		{
			if (!isSecondaryMoveController && PS4Input.MoveIsConnected (slot, 0) && PS4Input.MoveGetButtons (slot, 0) == (int)button) 
			{
				currentButtons = (int)button;
				return true;
			} 
			else if (PS4Input.MoveIsConnected (slot, 1) && PS4Input.MoveGetButtons (slot, 1) == (int)button) 
			{	
				currentButtons = (int)button;
				return true;
			}
		}
		return false;
	}

	public bool GetButtonUp(MoveButton button)
	{
		if (!GetButton(button))
			return false;
		return ((prevButtons != (int)button) && ((currentButtons == (int)button)));
	}

	public bool GetButtonDown(MoveButton button)
	{
		if (!GetButton(button))
			return false;
		return ((prevButtons == (int)button) && ((currentButtons != (int)button)));
	}

	public Vector3 getMoveRotation()
	{
		for (int slot = 0; slot < 4; slot++) 
		{
			if (!isSecondaryMoveController && PS4Input.MoveIsConnected (slot, 0))
				return PS4Input.GetLastMoveAcceleration (slot, 0);
			else if ( PS4Input.MoveIsConnected (slot, 1))
				return PS4Input.GetLastMoveAcceleration (slot, 1);
		}
		return Vector3.zero;
	}
}
#endif
