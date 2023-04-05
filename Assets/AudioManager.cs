using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip music;
    [SerializeField]
    AudioClip explosion;
    [SerializeField]
    AudioClip failed;
    [SerializeField]
    AudioClip click;
    [SerializeField]
    AudioSource musicSource;
    [SerializeField]
    AudioSource sfxSource;
    [SerializeField]
    AudioSource failedSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = music;
        musicSource.volume = 0.5f;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayExplosion()
    {
        sfxSource.clip = explosion;
        sfxSource.volume = 0.5f;
        sfxSource.Play();
    }

    public void PlayClick()
    {
        sfxSource.clip = click;
        sfxSource.volume = 0.1f;
        sfxSource.Play();
    }
    public void PlayFailed()
    {
        musicSource.volume = 0;
        failedSource.clip = failed;
        failedSource.Play();
    }
    public void PauseMusic()
    {
        musicSource.volume = 0;
    }

    public void PlayMusic()
    {
        musicSource.volume = 0.5f;
    }



}
