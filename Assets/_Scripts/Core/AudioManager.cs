using UnityEngine;
using System;
using System.Collections.Generic;


public enum AudioType
{
    //Interactables
    DoorOpen = 0,
    GateButton = 1,
    LavaHit = 2,

    //Skills
    BubbleShield = 3,
    FireballLaunch = 4,
    FireballHit = 5,
    ThrowItem = 6,

    //UI
    ButtonSelect = 7,
    ButtonPress = 8,
    GameOver = 9,
    GamePause = 10,
    GameWin = 11
}


[Serializable]
public class SoundData
{
    public AudioType type;
    public AudioClip clip;
}


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("========== AudioSources ==========")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("========== Sounds ==========")]
    [SerializeField] private SoundData[] sounds;

    private readonly Dictionary<AudioType, AudioClip> _soundDictionary = new();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);


        foreach (var _sound in sounds)
        { 
            if (!_soundDictionary.ContainsKey(_sound.type))
                _soundDictionary.Add(_sound.type, _sound.clip);
        }
    }


    public static void PlaySound(AudioType _type, float _volume = 1)
    {
        if (Instance == null)
        {
            Debug.LogWarning("AudioManager: Instance is null. Call ignored.");
            return;
        }

        if (Instance._soundDictionary.TryGetValue(_type, out AudioClip _clip))
            Instance.sfxSource.PlayOneShot(_clip, _volume);
    }


    public static void SelectButton()
    {
        PlaySound(AudioType.ButtonSelect, 1f);
    }


    public static void PressButton()
    {
        PlaySound(AudioType.ButtonPress, 1f);
    }


    public static void PlayMusic(AudioClip _clip, bool _loop = true)
    {
        if (Instance == null)
        {
            Debug.LogWarning("AudioManager: Instance is null. Call ignored.");
            return;
        }

        if (_clip == null)
        {
            Debug.LogWarning("AudioManager: Clip in Music Source is null. Call ignored.");
            return;
        }

        if (Instance.musicSource == null)
        {
            Debug.LogWarning("AudioManager: MusicSource is not assigned in the inspector. Call ignored.");
            return;
        }

        Instance.musicSource.clip = _clip;
        Instance.musicSource.loop = _loop;
        Instance.musicSource.Play();
    }


    public static void StopMusic()
    {
        if (Instance == null || Instance.musicSource == null) return;

        Instance.musicSource.Stop();
    }
}


