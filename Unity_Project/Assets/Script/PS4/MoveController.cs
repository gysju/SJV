using UnityEngine;
using System.Collections;

#if UNITY_PS4
using UnityEngine.PS4;
#endif
public class MoveController : MonoBehaviour {

    public bool isMoveController = false;
    public bool isSecondaryMoveController = false;

	void Start () {
	
	}
	
	void Update () {
	
	}

    public bool CheckForInput( int button)
    {
#if UNITY_PS4
        if (isMoveController )
        {
            if (!isSecondaryMoveController && PS4Input.MoveGetButtons(0,0) == button)
            {
                return (PS4Input.MoveGetAnalogButton(0, 0) > 0 ? true : false);
            }
            else if ( PS4Input.MoveGetButtons(0,1) == button )
            {
                return (PS4Input.MoveGetAnalogButton(0, 1) > 0 ? true : false);
            }
        }

        return false;
#else
		return Input.GetButton("Fire1");
#endif
    }
}
