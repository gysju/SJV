using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyBarPosition : MonoBehaviour
{
    public RectTransform uiPlane;
    private BaseEnemy enemy = null;
    private RectTransform rt;
    private Image im;
    public Image gauge;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        im = GetComponent<Image>();
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
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, enemy.transform.position);
            Vector2 newPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(uiPlane, pos, Camera.main, out newPos) && Vector3.Dot(Camera.main.transform.forward, (enemy.transform.position - Camera.main.transform.forward)) > 0)
            {
                im.CrossFadeAlpha(1f, 0f, false);
                gauge.CrossFadeAlpha(1f, 0f, false);
                gauge.fillAmount = enemy.GetCurrentHitPoints() / enemy.m_maxHitPoints / 1f;
                rt.anchoredPosition = newPos;
            }
            else
            {
                im.CrossFadeAlpha(0f, 0f, false);
                gauge.CrossFadeAlpha(0f, 0f, false);
            }
        }
		else
		{
			im.CrossFadeAlpha(0f, 0f, false);
            gauge.CrossFadeAlpha(0f, 0f, false);
        }
    }
}
