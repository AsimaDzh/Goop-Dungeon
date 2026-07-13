using UnityEngine;

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
}


public enum UIAudioType
{
    ButtonSelect = 0,
    ButtonPress = 1,
    GameOver = 2,
    GamePause = 3,
    GameWin = 4
}


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private AudioClip[] UIsoundList;

    private AudioSource _audioSource;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public static void PlaySound(AudioType _audio, float _volume = 1)
    {
        Instance._audioSource.PlayOneShot(Instance.soundList[(int)_audio], _volume);
    }


    public static void PlayUISound(UIAudioType _audio, float _volume = 1)
    {
        Instance._audioSource.PlayOneShot(Instance.UIsoundList[(int)_audio], _volume);
    }


    public static void SelectButton()
    {
        PlayUISound(UIAudioType.ButtonSelect, 1f);
    }


    public static void PressButton()
    {
        PlayUISound(UIAudioType.ButtonPress, 1f);
    }


    public static void StopSound()
    {
        Instance._audioSource.Stop();
    }
}


