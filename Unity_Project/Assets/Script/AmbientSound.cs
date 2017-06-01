using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour {

    public string[] soundName;
    [HideInInspector]
    public AudioSource[] audioSources;

    void Start () {
        audioSources = GetComponents<AudioSource>();
        for (int i = 0; i < audioSources.Length; i++)
        {
            SoundManager.Instance.PlaySound(soundName[i], audioSources[i], true);
        }
    }
}
