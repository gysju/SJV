using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [System.Serializable]
    public struct Sound
    {
        public string Name;
        public AudioClip audioClip;

        [Range(0, 1)]
        public float Volume;
        public Vector2 Pitch;
        private float currentPitch; // not currently used

        [Tooltip("Spatail blend, 2D to 3D")]
        [Range(0, 1)]
        public float SpatialBlend;
    };

    [Header("Sounds")]
    public List<Sound> Sounds = new List<Sound>();

    public static SoundManager Instance = null;
    private AudioSource[] audioSources;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitAudioSource();
            InitSounds();
        }
        else
        {
            Destroy(this);
        }
    }

    void InitAudioSource() // clean and refill when a scene is changed
    {
        audioSources = FindObjectsOfType<AudioSource>();
    }

    void InitSounds()
    {
        for (int i = 0; i < Sounds.Count; i++)
        {
            Sound s = Sounds[i];
            s.audioClip = (AudioClip)Resources.Load("Sounds/" + s.Name);

            if (s.audioClip == null)
            {
                continue;
            }
            Sounds[i] = s;
        }
    }

    public void PlaySound(string name, AudioSource source, bool FadeIn = false, float fadeDuration = 1.0f)
    {
        Sound s = findSound(name);
        if (s.audioClip != null)
        {
            source.clip = s.audioClip;
            s.Volume = source.volume;
            source.pitch = Random.Range(s.Pitch.x, s.Pitch.y);
            source.spatialBlend = s.SpatialBlend;

            source.Play();
            if (FadeIn)
                StartCoroutine(FadeVolume(source, s.Volume, fadeDuration, true));
        }
    }

    public void StopSound(AudioSource source, bool FadeOut = false, float fadeDuration = 1.0f)
    {
        if (source.isPlaying)
        {
            if (FadeOut)
                StartCoroutine(FadeVolume(source, 0.0f, fadeDuration, false));
            source.Stop();
        }
    }

    public void PlaySoundOnShot(string name, AudioSource source)
    {
        Sound s = findSound(name);
        if (s.audioClip != null)
        {
            source.volume = s.Volume;
            source.pitch = Random.Range(s.Pitch.x, s.Pitch.y);
            source.spatialBlend = s.SpatialBlend;

            source.PlayOneShot(s.audioClip, s.Volume);
        }
    }

    public void SpawnPlaySound(string name)
    {
        Sound s = findSound(name);
        if (s.audioClip != null)
        {
            AudioSource.PlayClipAtPoint(s.audioClip, Camera.main.transform.position + Camera.main.transform.forward * 5.0f, s.Volume);
        }
    }

    public Sound findSound(string name)
    {
        foreach (Sound sound in Sounds)
        {
            if (sound.Name == name)
                return sound;
        }
        return new Sound();
    }

    IEnumerator FadeVolume(AudioSource audioSource, float volume, float duration, bool IsFadeIn)
    {
        if (audioSource != null)
            yield return null;

        float vol = volume;

        if (!IsFadeIn)
            vol = audioSource.volume;

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            if (IsFadeIn)
                audioSource.volume = Mathf.Lerp(0, volume, time / duration);
            else
                audioSource.volume = Mathf.Lerp(volume, 0, time / duration);
            yield return null;
        }
    }

    public void RegenerateList()
    {
        AudioClip[] audioClip = Resources.LoadAll<AudioClip>("Sounds");
        if ( Sounds == null)
            Sounds = new List<Sound>();

        foreach (Sound s in Sounds) // Remove
        {
            bool check = true;
            for (int i = 0; i < audioClip.Length; i++)
            {
                if (audioClip[i].name == s.Name)
                {
                    check = false;
                    break;
                }
            }

            if (check)
                Sounds.Remove(s);
        }

        for (int i = 0; i < audioClip.Length; i++)
        {
            bool check = true;
            foreach (Sound s in Sounds)
            {
                if (audioClip[i].name == s.Name)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                Sound snd = new Sound { Name = audioClip[i].name };
                snd.audioClip = audioClip[i];
                Sounds.Add(snd);
            }
        }
    }

    public IEnumerator PauseAudioSource()
    {
        for( int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Pause();
            yield return null;
        }
    }

    public IEnumerator UnPauseAudioSource()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].UnPause();
            yield return null;
        }
    }
}
