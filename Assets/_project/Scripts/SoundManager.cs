using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    public List<AudioSource> sfxAudioSource = new List<AudioSource>();
    public AudioSource bgmAudioSource;
    public SoundCollection soundCollection;

    private Coroutine bgmLoopCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void PlaySFX(string soundName)
    {
        var freeAudioSource = GetFreeAudioSource();
        var audioClip = soundCollection.GetClip(soundName);

        if (audioClip != null)
        {
            freeAudioSource.loop = false;
            freeAudioSource.PlayOneShot(audioClip);
        }
    }

    public AudioSource PlaySFXLoop(string soundName)
    {
        var freeAudioSource = GetFreeAudioSource();
        var audioClip = soundCollection.GetClip(soundName);

        if (audioClip != null)
        {
            freeAudioSource.loop = true;
            freeAudioSource.clip = audioClip;
            freeAudioSource.Play();
        }

        return freeAudioSource;
    }

    public void PlayBGM(string soundName, float loopDelay)
    {
        var audioClip = soundCollection.GetClip(soundName);
        if (audioClip != null)
        {
            if (bgmLoopCoroutine != null)
            {
                StopCoroutine(bgmLoopCoroutine);
            }

            bgmLoopCoroutine = StartCoroutine(PlaySoundOnLoop(bgmAudioSource, audioClip, loopDelay));
        }
    }

    private IEnumerator PlaySoundOnLoop(AudioSource source, AudioClip audio, float loopDelay)
    {
        if (source == null)
        {
            yield break;
        }

        while (true)
        {
            source.PlayOneShot(audio);

            while (source.time < audio.length)
            {
                yield return null;
            }

            yield return new WaitForSeconds(loopDelay);
        }
    }

    private AudioSource GetFreeAudioSource()
    {
        for (int i = 0; i < sfxAudioSource.Count; ++i)
        {
            if (!sfxAudioSource[i].isPlaying)
            {
                return sfxAudioSource[i];
            }
        }

        return CreateNewAudioSource();
    }

    private AudioSource CreateNewAudioSource()
    {
        var newObj = new GameObject("Audiosource");
        newObj.transform.parent = this.transform;
        var audioSource = newObj.AddComponent<AudioSource>();
        audioSource.loop = false;
        sfxAudioSource.Add(audioSource);
        return audioSource;
    }
}
