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

    private float _fadeDurTitle = 1f;
    private float _fadeDurAllButtons = 1f;
    private float _fadeDurForEachButton = 1f;
    private float _delayBetweenButtons = 0.1f;

    private Tween 
        _animTitle, 
        _animPlayButton, 
        _animOptionsButton, 
        _animExtrasButton, 
        _animLeaveButton;


    private void Awake()
    {
        titleFade.alpha = 0f;
    }


    private void Start()
    {
        AnimTitle();
    }


    private void AnimTitle()
    {
        _animTitle = titleFade.DOFade(1f, _fadeDurTitle);
    }


    private void OnDestroy()
    {
        ClearAnimations();
    }


    public void ClearAnimations()
    {
        _animTitle?.Kill();
        _animPlayButton?.Kill();
        _animOptionsButton?.Kill();
        _animExtrasButton?.Kill();
        _animLeaveButton?.Kill();
    }
}
