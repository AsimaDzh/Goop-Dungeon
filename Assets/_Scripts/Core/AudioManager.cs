using UnityEngine;

public enum AudioType
{
    //Interactables
    DoorLock = 0,
    GateButton = 1,
    LavaHit = 2,

    //Skills
    BubbleShield = 3,
    FireballLaunch = 4,
    FireballHit = 5,
    ThrowItem = 6,

    //UI
    ButtonSelect = 7,
    GameOver = 8,
    GamePause = 9,
    GameWin = 10,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip[] soundList;
    
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
}


