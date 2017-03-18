﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitBunker : MonoBehaviour
{
    protected List<Material> m_materials = new List<Material>();
    public float m_bunkerTransitionSpeed = 1f;
    
    void Start ()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            m_materials.Add(renderer.material);
        }
    }

    public void ActivateBunkerMode()
    {
        StartCoroutine(ShowBunker());
    }

    public void DeactivateBunkerMode()
    {
        StartCoroutine(FadeBunker());
    }

    public IEnumerator FadeBunker()
    {
        float time = 0.0f;

        while (time < m_bunkerTransitionSpeed)
        {
            time += Time.deltaTime;
            foreach (Material material in m_materials)
            {
                material.SetFloat("_AlphaValue", Mathf.Lerp(1.0f, 0.0f, (time / m_bunkerTransitionSpeed)));
            }
            yield return null;
        }
    }

    public IEnumerator ShowBunker()
    {
        float time = 0.0f;

        while (time < m_bunkerTransitionSpeed)
        {
            time += Time.deltaTime;
            foreach (Material material in m_materials)
            {
                material.SetFloat("_AlphaValue", Mathf.Lerp(0.0f, 1.0f, (time / m_bunkerTransitionSpeed)));
            }
            yield return null;
        }
    }
}
