using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyBarPosition : MonoBehaviour
{
    public RectTransform uiPlane;
    public Vector3? enemyPosition = null;
    private RectTransform rt;
    private Image im;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        im = GetComponent<Image>();
    }

    void Update ()
	{
        if (enemyPosition.HasValue)
        {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, enemyPosition.Value);
            Vector2 newPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(uiPlane, pos, Camera.main, out newPos) && Vector3.Dot(Camera.main.transform.forward, (enemyPosition.Value - Camera.main.transform.forward)) > 0)
            {
                im.CrossFadeAlpha(1f, 0f, false);
                rt.anchoredPosition = newPos;
            }
            else
            {
                im.CrossFadeAlpha(0f, 0f, false);
            }
        }
		else
		{
			im.CrossFadeAlpha(0f, 0f, false);
		}
    }
}
