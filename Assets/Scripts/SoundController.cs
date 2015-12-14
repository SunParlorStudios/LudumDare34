using UnityEngine;
using System.Collections.Generic;

public class SoundController : MonoBehaviour
{
    [HideInInspector]public static SoundController instance;

    public AudioClip[] audioClips;

    private List<AudioSource> audioSources;

    public void Awake()
    {
        instance = this;

        audioSources = new List<AudioSource>();
        for (int i = 0; i < audioClips.Length; i++)
        {
            GameObject gameobject = new GameObject("AudioSource");
            audioSources.Add(gameobject.AddComponent<AudioSource>());
            audioSources[i].clip = audioClips[i];
            audioSources[i].playOnAwake = false;
            gameobject.transform.SetParent(transform);
        }

        audioSources[0].volume = 0.3f;
        audioSources[4].volume = 0.3f;
        audioSources[5].volume = 0.15f;
        audioSources[7].volume = 0.7f;

        Play(0, true);
    }

    public void Play(int audioClipId, bool looped = false, bool overridePlay = false)
    {
        if (audioSources[audioClipId].isPlaying == false || overridePlay == true)
        {
            audioSources[audioClipId].loop = looped;
            audioSources[audioClipId].Play();
        }
    }

    public void Pause(int audioClipId)
    {
        audioSources[audioClipId].Pause();
    }

    public void UnPause(int audioClipId)
    {
        audioSources[audioClipId].UnPause();
    }

    public void Stop(int audioClipId)
    {
        audioSources[audioClipId].Stop();
    }
}
