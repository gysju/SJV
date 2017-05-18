using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxSound : MonoBehaviour {

    public string Sound = "";
    private AudioSource audioSource;
    private float time = 0.0f;
    private ParticleSystem particle;

	void Start () {
        audioSource = GetComponent<AudioSource>();
        particle = GetComponent<ParticleSystem>();
        StartCoroutine(WaitDelay());
    }
	
	void Update ()
    {
        if (time >= particle.main.duration)
        {
            time = 0.0f;
            SoundManager.Instance.PlaySoundOnShot(Sound, audioSource);
        }
        time += Time.deltaTime;
    }

    IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(particle.main.startDelay.Evaluate(0));
        time = 0.0f;
        SoundManager.Instance.PlaySoundOnShot(Sound, audioSource);
    }
}
