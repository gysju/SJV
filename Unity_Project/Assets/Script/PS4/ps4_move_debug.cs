﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

#if  UNITY_PS4
using UnityEngine.PS4;
#endif
public class ps4_move_debug : MonoBehaviour {
	#if  UNITY_PS4
	public enum MoveButton { MoveButton_Trigger = 2, MoveButton_Move = 4, MoveButton_Start = 8, MoveButton_Triangle = 16, MoveButton_Circle = 32, MoveButton_Cross = 64, MoveButton_Square = 128, MoveButton_MaxAnalogueValue = 255, MoveButton_Count = 8}

	public bool isMoveController = false;
	public bool isSecondaryMoveController = false;

	private int currentButtons = 0;
	private int prevButtons = 0;

	string data;
	// Use this for initialization
	void Start () {
	
	}
	

	void Update()
	{
		prevButtons = currentButtons;
	}

	void LateUpdate()
	{
		currentButtons = 0;
	}

	void OnGUI() 
	{
		//int numDetected = 0;
		//for (int slot=0;slot<4;slot++)
		//{
		//	for (int controller=0; controller<1; controller++)
		//	{
		//		if (PS4Input.MoveIsConnected(slot,controller))
		//		{
		//			string data = String.Format("get button up : {0}, get button Down : {1} , get button : {2}, prevButtons : {3}, currentButtons : {4}" ,
		//				GetButtonUp(MoveButton.MoveButton_Move, controller),
		//				GetButtonDown(MoveButton.MoveButton_Move, controller),
		//				GetButton(MoveButton.MoveButton_Move, controller),
		//				prevButtons,
		//				currentButtons
		//					);
		//			GUI.Label(new Rect(64, 64 + slot*40 + controller*20, 1500, 20), data);
		//			numDetected++;
		//		}
		//	}
		//
		//}

		string data = String.Format("left axis y : {0}, right axis y : {1}, angle : {2}" , (float)System.Math.Round((Mathf.Clamp(getMoveRotation(0).y,-1,1) + 1) * 0.5f, 1), 
																						   (float)System.Math.Round((Mathf.Clamp(getMoveRotation(1).y,-1,1) + 1) * 0.5f, 1),
																							PSMoveRotation());
		GUI.Label(new Rect(64, 64 + 40 + 20, 1500, 20), data);

	}

	public bool GetButton( MoveButton button, int slot )
	{
		if (PS4Input.MoveIsConnected (0, slot) && PS4Input.MoveGetButtons (0, slot) == (int)button) 
		{	
			currentButtons = (int)button;
			return true;
		}
		return false;
	}

	public bool GetButtonUp(MoveButton button, int slot)
	{
		return ((prevButtons == (int)button) && !GetButton(button, slot));
	}

	public bool GetButtonDown(MoveButton button, int slot)
	{
		if (!GetButton(button, slot))
			return false;
		return ((prevButtons != (int)button) && ((currentButtons == (int)button)));
	}
		
	public Vector3 getMoveRotation(int index)
	{
		return PS4Input.GetLastMoveAcceleration (0, index);
	}

	float PSMoveRotation()
	{
		float leftHand =  (float)System.Math.Round((Mathf.Clamp(getMoveRotation(0).y, -1, 1) + 1) * 0.5f, 1);
		float RightHand = (float)System.Math.Round((Mathf.Clamp(getMoveRotation(1).y, -1, 1) + 1) * 0.5f, 1);

		if(leftHand != RightHand)
		{
			float diff = Math.Abs(leftHand - RightHand);
			diff *= 180.0f;

			if (leftHand < RightHand)
				diff = -diff;

			return Mathf.Round(diff);

		}
		return 0.0f;
	}
	#endif
}
