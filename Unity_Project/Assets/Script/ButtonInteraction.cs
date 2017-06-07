using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonInteraction : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource audioSource;

	void Start () {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(() => Clicked());
	}

    public void OnPointerEnter(PointerEventData ped)
    {
        SoundManager.Instance.PlaySoundOnShot("mecha_button_hover", audioSource);
    }

    void Clicked()
    {
        SoundManager.Instance.PlaySoundOnShot("mecha_button_press", audioSource);
    }

    public void Selected()
    {
        SoundManager.Instance.PlaySoundOnShot("mecha_button_hover", audioSource);
    }
}
