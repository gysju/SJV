using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyBarPosition : MonoBehaviour
{
    public RectTransform uiPlane;
    private BaseEnemy enemy = null;
    private RectTransform rt;
    private CanvasGroup canvasGroup;
    private Camera mainCam;

    //private Image im;
    public Image gauge;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rt = GetComponent<RectTransform>();
        mainCam = Camera.main;
        //im = GetComponent<Image>();
    }

    public void SetEnemy(GameObject newEnemy)
    {
        enemy = newEnemy.GetComponent<BaseEnemy>();
    }

    public void EraseEnemy()
    {
        enemy = null;
    }

    void Update ()
	{
        if (enemy)
        {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(mainCam, enemy.transform.position);
            Vector2 newPos;
            if (enemy.GetCurrentHitPoints() > 0 && RectTransformUtility.ScreenPointToLocalPointInRectangle(uiPlane, pos, mainCam, out newPos) && Vector3.Dot(mainCam.transform.forward, (enemy.transform.position - mainCam.transform.forward)) > 0)
            {
                canvasGroup.alpha = 1f;
                //im.CrossFadeAlpha(1f, 0f, false);
                //gauge.CrossFadeAlpha(1f, 0f, false);
                gauge.fillAmount = enemy.GetCurrentHitPoints() * 1f / enemy.m_maxHitPoints;
                rt.anchoredPosition = newPos;
				rt.LookAt (mainCam.transform.position);
            }
            else
            {
                canvasGroup.alpha = 0f;
                //im.CrossFadeAlpha(0f, 0f, false);
                //gauge.CrossFadeAlpha(0f, 0f, false);
            }
        }
		else
		{
            canvasGroup.alpha = 0f;
            //im.CrossFadeAlpha(0f, 0f, false);
            //gauge.CrossFadeAlpha(0f, 0f, false);
        }
    }
}
