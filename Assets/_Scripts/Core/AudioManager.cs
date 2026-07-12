using UnityEngine;


public enum AudioType
{
    Music,
    Skills,
    UI,
    Interactables
}

[RequireComponent(typeof(AudioSource))]
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

