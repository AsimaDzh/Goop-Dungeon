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

        _audioSource = GetComponent<AudioSource>();
    }


    public static void PlaySound(AudioType _audio, float _volume = 1)
    {
        if (Instance == null)
        {
            Debug.LogWarning("AudioManager: Instance is null. Call ignored.");
            return;
        }

        if (Instance._audioSource == null)
        {
            Debug.LogWarning("AudioManager: AudioSource is null. Call ignored.");
            return;
        }

        Instance._audioSource.PlayOneShot(Instance.soundList[(int)_audio], _volume);
    }


    public static void PlayUISound(UIAudioType _audio, float _volume = 1)
    {
        if (Instance == null)
        {
            Debug.LogWarning("AudioManager: Instance is null. Call ignored.");
            return;
        }

        if (Instance._audioSource == null)
        {
            Debug.LogWarning("AudioManager: AudioSource is null. Call ignored.");
            return;
        }

        Instance._audioSource.PlayOneShot(Instance.uiSoundList[(int)_audio], _volume);
    }


    public static void SelectButton()
    {
        PlayUISound(UIAudioType.ButtonSelect, 1f);
    }


    public static void PressButton()
    {
        PlayUISound(UIAudioType.ButtonPress, 1f);
    }


    public static void StopMusic()
    {
        if (Instance == null || Instance._audioSource == null) return;

        Instance._audioSource.Stop();
    }
}


