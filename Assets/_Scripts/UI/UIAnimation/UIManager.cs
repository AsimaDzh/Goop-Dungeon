using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup
        titleFade, 
        playButton, 
        optionsButton, 
        extrasButton, 
        leaveButton;

    private float _fadeDurTitle = 2f;
    private float _fadeDurAllButtons = 1f;
    private float _fadeDurForEachButton = 1f;
    private float _delayBetweenButtons = 0.1f;

    private Tween 
        _animTitle, 
        _animPlayButton, 
        _animOptionsButton, 
        _animExtrasButton, 
        _animLeaveButton;
}
