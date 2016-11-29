using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class GameStateBaseAnimatorBehaviour : StateMachineBehaviour
{
    public GameObject m_RelatedMenuPrefab;
    public float m_StateFadeOutSpeed;
    protected GameObject m_RelatedMenu;
    private GameObject m_Fade;
    private Animator m_FadeAnimator;
    protected FadeManager m_FadeManager;
    private bool m_FirstFrame = true;

    [System.Serializable]
    public class OnBehaviourEvent : UnityEvent { };

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex) && ! m_FirstFrame)
        {
            m_RelatedMenu.SetActive(true);

            if (stateInfo.normalizedTime > 0.75f && !m_FadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"))
                m_FadeManager.GoBlack();
        }
        m_FirstFrame = false;
    }
    public void FadeToBlack()
    {
        m_FadeManager.GoBlack();
    }

    public bool fadeToBlackIsFinish()
    {
        AnimatorStateInfo stateInfo = m_FadeAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1.0f && stateInfo.IsName("FadeIn"))
        {
            return true;
        }
        return false;
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_RelatedMenu == null)
        {
			m_RelatedMenu = GameObject.Instantiate(m_RelatedMenuPrefab) as GameObject;
            m_RelatedMenu.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.FindChild("ParentMenu").transform);

			setCanvasSize ();
        }
        else
        {
            m_RelatedMenu.SetActive(true);
        }
        if (m_Fade == null)
        {
            m_Fade = FadeManager.INSTANCE.gameObject;
            //m_Fade.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);

            m_FadeManager = m_Fade.GetComponent<FadeManager>();
            m_FadeAnimator = m_Fade.GetComponent<Animator>();

            if (m_FadeManager != null)
                m_FadeManager.SetSpeedFadeOutAndFadeIn(m_StateFadeOutSpeed);
        }

        if ( !m_FadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeOut") )
                m_FadeManager.GoTransparent();

        Button ButtonSelected = m_RelatedMenu.GetComponentInChildren<Button>();

        if (ButtonSelected != null)
        {
            ButtonSelected.Select();
        }

		m_RelatedMenu.transform.localPosition = Vector3.zero;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		if (m_RelatedMenu != null) 
		{
        	SetInteractableButtonValue( true );
        	m_RelatedMenu.SetActive(false);
		}
    }

    public void SetInteractableButtonValue( bool bValue)
    {
		List<Button> buttons = m_RelatedMenu.GetComponentsInChildren<Button>().ToList<Button>();

		foreach (Button button in buttons) 
		{
			button.interactable = bValue;
		}
    }

    public Menu GetMenu()
    {
        return m_RelatedMenu.GetComponent<Menu>();
    }

	public void setCanvasSize()
	{
		// set size of rect transform, i d'ont know why but when i set the size in the prefab, thats didn't work.
		RectTransform rectTransfrom = m_RelatedMenu.GetComponent<RectTransform>();

		rectTransfrom.offsetMin = new Vector2(rectTransfrom.offsetMin.x, 0.0f);
		rectTransfrom.offsetMax = new Vector2(rectTransfrom.offsetMax.x, 0.0f);
		rectTransfrom.offsetMin = new Vector2(rectTransfrom.offsetMin.y, 0.0f);
		rectTransfrom.offsetMax = new Vector2(rectTransfrom.offsetMax.y, 0.0f);

		m_RelatedMenu.transform.localScale = Vector3.one;
	}
}
