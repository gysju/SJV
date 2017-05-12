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

        [Range(0, 1)]
        public float Volume;

        [HideInInspector]
        public AudioClip audioClip;
    };

    [Header("Sounds")]
    public List<Sound> Sounds = new List<Sound>();

    public static SoundManager Instance = null;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            InitSounds();
        }
        else
        {
            Destroy(this);
        }
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

    public void PlaySound(string name, AudioSource source, bool FadeIn = false)
    {
        Sound s = findSound(name);
        if (s.audioClip != null)
        {
            source.clip = s.audioClip;
            source.Play();
            if (FadeIn)
                StartCoroutine(FadeVolume(source, s.Volume, 1.0f, true));
        }
    }

    public void StopSound(AudioSource source, bool FadeOut = false)
    {
        if (source.isPlaying)
        {
            if (FadeOut)
                StartCoroutine(FadeVolume(source, 0.0f, 1.0f, false));
            source.Stop();
        }
    }

    public void PlaySoundOnShot(string name, AudioSource source)
    {
        Sound s = findSound(name);
        if (s.audioClip != null)
        {
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
        DirectoryInfo levelDirectoryPath = new DirectoryInfo(Application.dataPath);
        FileInfo[] fileInfo1 = levelDirectoryPath.GetFiles("*.wav", SearchOption.AllDirectories);
        FileInfo[] fileInfo2 = levelDirectoryPath.GetFiles("*.mp3", SearchOption.AllDirectories);

        Sounds = new List<Sound>();

        foreach (FileInfo fileinfo in fileInfo1)
        {
            Sound snd = new Sound { Name = Path.GetFileNameWithoutExtension(fileinfo.Name), Volume = 1.0f};

            Sounds.Add(snd);
        }

        foreach (FileInfo fileinfo in fileInfo2)
        {
            Sound snd = new Sound { Name = Path.GetFileNameWithoutExtension(fileinfo.Name), Volume = 1.0f };

            Sounds.Add(snd);
        }
    }
}
