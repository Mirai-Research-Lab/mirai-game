using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] tracks;
    public static AudioManager instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
        foreach (Sound s in tracks)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.clip = s.audioClip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
        if(PlayerPrefs.HasKey("Volume"))
        {
            tracks[0].audioSource.volume = PlayerPrefs.GetFloat("Volume");
        }
    }
    private void Start()
    {
        Play("Theme");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(tracks, track => track.name == name);
        if(s != null)
            s.audioSource.Play();
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(tracks, track => track.name == name);
        s.audioSource.PlayOneShot(s.audioClip);
    }

    public Sound[] GetTracks()
    {
        return tracks;
    }
}
