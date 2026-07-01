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
    private float _fadeDurForEachButton = 0.3f;

    private Tween _animTitle;
    private Sequence _buttonSequence;


    private void Awake()
    {
        titleFade.alpha = 0f;
    }


    private void Start()
    {
        AnimTitle();
        AnimButtons();
    }


    private void AnimTitle()
    {
        _animTitle = titleFade.DOFade(1f, _fadeDurTitle);
    }


    private void AnimButtons()
    {
        _buttonSequence = DOTween.Sequence();

        _buttonSequence
            .Append(playButton.DOFade(1f, _fadeDurForEachButton))
            .Append(optionsButton.DOFade(1f, _fadeDurForEachButton))
            .Append(extrasButton.DOFade(1f, _fadeDurForEachButton))
            .Append(leaveButton.DOFade(1f, _fadeDurForEachButton));
    }


    private void OnDestroy()
    {
        ClearAnimations();
    }


    public void ClearAnimations()
    {
        _animTitle?.Kill();
        _buttonSequence?.Kill();
    }
}
