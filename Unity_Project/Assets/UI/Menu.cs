using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
    #region static&constant
    #endregion

    #region variables

    public bool bOnClick = false;
    public string NextState { get; private set;}
    public string PreviousState;

    #endregion

    #region fields
    #endregion

    #region functions

    public void OnClick(string StateSelected)
    {
        bOnClick = true;
        NextState = StateSelected;
    }
    void Update()
    {
		if (Input.GetButtonDown("Start") && PreviousState != null && PreviousState != "")
		{
		   bOnClick = true;
		   NextState = PreviousState;
		}
    }
    #endregion

    #region events
    #endregion
}
