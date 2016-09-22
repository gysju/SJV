using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class GraphicalElement : MonoBehaviour
{
    protected const float INVISIBLE = 0f;
    protected const float OPAQUE = 1f;

    private Renderer m_renderer;

    protected Animator m_animator;
    protected bool m_animated = false;

    protected virtual void Awake()
	{
        m_renderer = GetComponent<Renderer>();
        m_animator = GetComponent<Animator>();
        m_animated = m_animator;
    }

    protected virtual void Start()
    {

    }

    /// <summary>Coroutine permetant de changer progressivement l'alpha de la couleur principale d'un matériel.</summary>
    /// <param name ="material">Matériel à modifier.</param>
    /// <param name ="valueToFade">Valeur d'alpha à atteindre (entre 0 et 1).</param>
    /// <param name ="timeToFade">Temps nécessaire pour atteindre la valeur d'alpha.</param>
    IEnumerator Fade(Material material, float valueToFade, float timeToFade)
    {
        Color materialColor = material.color;

        float clampedValueToFade = Mathf.Clamp01(valueToFade);

        float alpha = materialColor.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / timeToFade)
        {
            Color newColor = new Color(materialColor.r, materialColor.g, materialColor.b, Mathf.Lerp(alpha, clampedValueToFade, t));
            material.color = newColor;
            yield return null;
        }

        Color finalColor = new Color(materialColor.r, materialColor.g, materialColor.b, clampedValueToFade);
        material.color = finalColor;
    }

    /// <summary>Permet de faire disparaître/apparaître progressivement l'objet.</summary>
    /// <param name ="valueToFade">Valeur d'alpha à atteindre (entre 0 et 1).</param>
    /// <param name ="timeToFade">Temps nécessaire pour atteindre la valeur d'alpha.</param>
    protected void StartFade(float valueToFade, float timeToFade)
    {
        StopCoroutine("Fade");
        float clampedValueToFade = Mathf.Clamp01(valueToFade);
        foreach (Material item in m_renderer.materials)
        {
            StartCoroutine(Fade(item, clampedValueToFade, timeToFade));
        }
    }

    void Update()
	{
	    
	}
}
