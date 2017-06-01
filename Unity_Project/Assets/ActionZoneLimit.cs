﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionZoneLimit : MonoBehaviour
{
    Material m_material;
    BoxCollider m_collider;

    public float m_fadeSpeed = 1f;

    void Awake()
    {
        m_material = GetComponent<Renderer>().material;
        m_collider = GetComponent<BoxCollider>();
    }

    public void ShowWall()
    {
        m_collider.enabled = true;
        StartCoroutine(FadeWall(1f, true));
    }

    public void HideWall()
    {
        StartCoroutine(FadeWall(0f, false));
    }

    IEnumerator FadeWall(float target, bool collide)
    {
        float startAlpha = m_material.GetFloat("_AlphaValue");

        float time = 0.0f;

        while (time < m_fadeSpeed)
        {
            time += Time.deltaTime;
            m_material.SetFloat("_AlphaValue", Mathf.Lerp(startAlpha, target, (time / m_fadeSpeed)));
            yield return null;
        }

        m_collider.enabled = collide;
    }
}
