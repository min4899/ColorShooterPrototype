using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Music
{
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.5f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;
}

public class MusicManager : MonoBehaviour {

    public static MusicManager instance;

    [SerializeField]
    Music[] inGameMusic;

    public Music bossMusic;

    private AudioSource source;

    void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }


    void Start()
    {
        int select = Random.Range(0, inGameMusic.Length); // choose a random music from list
        source.clip = inGameMusic[select].clip;
        source.volume = inGameMusic[select].volume;
        source.pitch = inGameMusic[select].pitch;
        source.Play();
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Unpause()
    {
        source.UnPause();
    }

    public void PlayBossMusic()
    {
        // Replace ingame music with boss music and start it
        source.clip = bossMusic.clip;
        source.volume = bossMusic.volume;
        source.pitch = bossMusic.pitch;
        source.Play();
    }
}
