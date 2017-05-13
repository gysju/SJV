using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour {

    private AudioSource audioSource;

	void Start () {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(() => Clicked());
	}

    void Clicked()
    {
        SoundManager.Instance.PlaySoundOnShot("mecha_placeholder_UI_1", audioSource);
    }
}
