using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.5f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.volume = volume;
    }

    public void Play()
    {
        source.Play();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;

    private bool soundOn;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        //instance = this;
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
    }

    void Start()
    {
        soundOn = SaveGame.Load<bool>("SoundOn");
    }

    public void PlaySound(string _name)
    {
        if (soundOn)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].name == _name)
                {
                    sounds[i].Play();
                    return;
                }
            }
            // No audio with this name is found.
            Debug.LogWarning("AudioManager: Sound not found in list: " + _name);
        }
    }

    public void SetSoundOption()
    {
        soundOn = SaveGame.Load<bool>("SoundOn");
    }
}
