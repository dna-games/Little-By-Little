using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public SoundClip[] musicClips;
    public SoundClip[] sfxClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayMusic(string name)
    {
        SoundClip s = Array.Find(musicClips, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning($"Music clip '{name}' not found!");
            return;
        }

        musicSource.clip = s.clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        SoundClip s = Array.Find(sfxClips, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning($"SFX clip '{name}' not found!");
            return;
        }

        sfxSource.PlayOneShot(s.clip);
    }
}

[System.Serializable]
public class SoundClip
{
    public string name;
    public AudioClip clip;
}
