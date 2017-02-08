using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

#if UNITY_PS4
using UnityEngine.PS4;
#endif
public class MoveController : MonoBehaviour {
	public enum MoveButton { MoveButton_Trigger = 2, MoveButton_Move = 4, MoveButton_Start = 8, MoveButton_Triangle = 16, MoveButton_Circle = 32, MoveButton_Cross = 64, MoveButton_Square = 128, MoveButton_MaxAnalogueValue = 255, MoveButton_Count = 8}

    private int currentButtons = 0;
	private int prevButtons = 0;
	
    public Vector3 lookAtHit;
	public int MoveIndex = 0;

    protected Coroutine currentVibration = null;
    protected int currentVibrationPower = 0;
    protected float currentVibrationDuration = 0f;

	void Update () 
	{
		prevButtons = currentButtons;
	}

	public bool GetButton( MoveButton button )
	{
		#if UNITY_PS4
		
		for (int slot = 0; slot < 4; slot++)  
		{
			if (PS4Input.MoveIsConnected (slot, MoveIndex) 
				&& (PS4Input.MoveGetButtons (slot, MoveIndex) == (int)button)) 
			{
				currentButtons = (int)button;
				return true;
			} 
		}
		#endif
		return false;
	}

	public bool GetButtonUp(MoveButton button)
	{
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
		#if UNITY_PS4

		for (int slot = 0; slot < 4; slot++) 
		{
			if (PS4Input.MoveIsConnected (slot, MoveIndex))
				return PS4Input.GetLastMoveAcceleration (slot, MoveIndex);
		}
		#endif
		return Vector3.zero;
	}

#if UNITY_PS4
    IEnumerator Vibration(int power, float duration)
    {
        currentVibrationDuration = duration;

        while (currentVibrationDuration > 0f)
        {
            currentVibrationPower = (int)Mathf.Lerp(0, 100, currentVibrationDuration / duration);
            for (int slot = 0; slot < 4; slot++)
            {
                if (PS4Input.MoveIsConnected(slot, MoveIndex))
                {
                    PS4Input.MoveSetVibration(slot, MoveIndex, currentVibrationPower);
                }
            }
            currentVibrationDuration -= Time.deltaTime;
            yield return null;
        }

        for (int slot = 0; slot < 4; slot++)
        {
            if (PS4Input.MoveIsConnected(slot, MoveIndex))
            {
                PS4Input.MoveSetVibration(slot, MoveIndex, 0);
            }
        }
        currentVibrationDuration = 0f;
        currentVibrationPower = 0;
    }

    IEnumerator ConstantVibration(int power, float duration)
    {
        currentVibrationDuration = duration;
        currentVibrationPower = power;
        
        for (int slot = 0; slot < 4; slot++)
        {
            if (PS4Input.MoveIsConnected(slot, MoveIndex))
            {
                PS4Input.MoveSetVibration(slot, MoveIndex, currentVibrationPower);
            }
        }

        yield return new WaitForSeconds(duration);

        for (int slot = 0; slot < 4; slot++)
        {
            if (PS4Input.MoveIsConnected(slot, MoveIndex))
            {
                PS4Input.MoveSetVibration(slot, MoveIndex, 0);
            }
        }

        currentVibrationDuration = 0f;
        currentVibrationPower = 0;
    }

    public void StartVibration(int power, float duration)
    {
        if(currentVibrationDuration < duration || currentVibrationPower < power)
        {
            StopCoroutine(currentVibration);
            currentVibration = StartCoroutine(Vibration(power, duration));
        }
    }
#endif
}
