using UnityEngine;
using System.Collections;

public class FadeManager : MonoBehaviour 
{
    #region static&constant
    public static FadeManager INSTANCE;
    #endregion

    #region variables
    Animator m_ThisAnimator;

    #endregion

    #region fields
    #endregion

    #region functions
    public void Start()
    {
        //This is suposed to be a singleton but is not implemented yet
        INSTANCE = this;

        m_ThisAnimator = GetComponent<Animator>();
    }

    public void GoBlack()
    {
        m_ThisAnimator.SetTrigger("FadeIn");
    }

    public void GoTransparent()
    {
        m_ThisAnimator.SetTrigger("FadeOut");
    }

    public void SetSpeedFadeOutAndFadeIn(float Speed)
    {
        m_ThisAnimator.speed = Speed;
    }
    #endregion

    #region events
    #endregion
}
