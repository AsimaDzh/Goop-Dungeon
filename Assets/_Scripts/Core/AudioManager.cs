using UnityEngine;
using System;

public enum AudioType
{
    Interactables = 0,

    //Skills
    BubbleShield = 1,
    Fireball = 2,
    ThrowItem = 3,

    UI = 4,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioList[] soundList;
    
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
        //Instance._audioSource.PlayOneShot(Instance.soundList[(int)_audio], _volume);
    }


#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] _names = Enum.GetNames(typeof(AudioType));
        Array.Resize(ref soundList, _names.Length);

        for (int i = 0; i < soundList.Length; i++)
            soundList[i].name = _names[i];
    }
#endif
}


[Serializable]
public struct AudioList
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}

