using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_PS4
using UnityEngine.PS4;
#endif
public class MoveController : MonoBehaviour {
	#if UNITY_PS4

	public enum MoveButton { MoveButton_Trigger = 2, MoveButton_Move = 4, MoveButton_Start = 8, MoveButton_Triangle = 16, MoveButton_Circle = 32, MoveButton_Cross = 64, MoveButton_Square = 128, MoveButton_MaxAnalogueValue = 255, MoveButton_Count = 8}

	public int MoveIndex = 0;
	public Vector3 lookAtHit;

	private int currentButtons = 0;
	private int prevButtons = 0;


	void Start () 
	{
	
	}
	
	void Update () 
	{
		prevButtons = currentButtons;
	}

	public bool GetButton( MoveButton button )
	{
		for (int slot = 0; slot < 4; slot++) 
		{
			if (PS4Input.MoveIsConnected (slot, MoveIndex) 
				&& (PS4Input.MoveGetButtons (slot, MoveIndex) == (int)button)) 
			{
				currentButtons = (int)button;
				return true;
			} 
		}
		return false;
	}

	public bool GetButtonUp(MoveButton button)
	{
		//return !GetButton (button) && (prevButtons == currentButtons); 
		bool test = ((prevButtons == (int)button) && !GetButton(button));
		if ( test ) currentButtons = 0;
		return test;
	}

	public bool GetButtonDown(MoveButton button)
	{
		if (!GetButton(button))
			return false;
		return ((prevButtons != (int)button) && ((currentButtons == (int)button)));
	}

	public Vector3 getMoveRotation()
	{
		for (int slot = 0; slot < 4; slot++) 
		{
			if (PS4Input.MoveIsConnected (slot, MoveIndex))
				return PS4Input.GetLastMoveAcceleration (slot, MoveIndex);
		}
		return Vector3.zero;
	}
	#endif
}
