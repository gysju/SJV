using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionZoneLimit : MonoBehaviour
{
    Material m_material;
    BoxCollider m_collider;

    public float m_fadeSpeed = 1f;

    [Header("Sounds")]
    public string StaticSound;
    public string OpeningSound;
    private AudioSource audioSource;

    void Awake()
    {
        m_material = GetComponent<Renderer>().material;
        m_collider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        SoundManager.Instance.PlaySound( StaticSound, audioSource, true);
    }
    public void ShowWall()
    {
        m_collider.enabled = true;
        SoundManager.Instance.PlaySound(StaticSound, audioSource, true);

        StartCoroutine(FadeWall(1f, true));
    }

    public void HideWall()
    {
        SoundManager.Instance.PlaySound(OpeningSound, audioSource, true);
        StartCoroutine(FadeWall(0f, false));
    }

    IEnumerator FadeWall(float target, bool collide)
    {
        float startAlpha = m_material.GetFloat("_Alpha");

        float time = 0.0f;

        while (time < m_fadeSpeed)
        {
            time += Time.deltaTime;
            m_material.SetFloat("_Alpha", Mathf.Lerp(startAlpha, target, (time / m_fadeSpeed)));
            yield return null;
        }

        m_collider.enabled = collide;
        if (target == 0.0f)
            Destroy(gameObject);
    }
}
