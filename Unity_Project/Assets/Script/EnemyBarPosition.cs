using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyBarPosition : MonoBehaviour
{
    public RectTransform uiPlane;
    public Transform test;
    private RectTransform rt;
    private Image im;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        im = GetComponent<Image>();
    }

    void Update ()
	{
        
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, test.position);
        Vector2 newPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(uiPlane, pos, Camera.main, out newPos) && Vector3.Dot(Camera.main.transform.forward, (test.position - Camera.main.transform.forward)) > 0)
        {
            im.CrossFadeAlpha(1f, 0f, false);
            rt.anchoredPosition = newPos;
        }
        else
            im.CrossFadeAlpha(0f, 0f, false);
    }
}
