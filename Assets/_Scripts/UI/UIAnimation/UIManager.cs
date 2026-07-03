using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup title, playButton, optionsButton, extrasButton, leaveButton;
    [SerializeField] private RectTransform allButtons;
    
    private float _titleDur = 0.7f;
    private float _titleFadeDur = 0.5f;
    private float _buttonsDur = 2.5f;
    private float _buttonsFadeDur = 0.3f;
    private Sequence _menuSequence;
    private Tween _titleTween, _buttonsTween;


    private void Awake()
    {
        title.alpha = 0f;
        playButton.alpha = 0f;
        optionsButton.alpha = 0f;
        extrasButton.alpha = 0f;
        leaveButton.alpha = 0f;
    }


    private void Start()
    {
        ShowTitle();
        ShowMenuButtons();
    }


    private void ShowTitle()
    {
        _titleTween = title.DOFade(1f, _titleFadeDur);
    }


    private void ShowMenuButtons()
    {
        _buttonsTween = allButtons
            .DOAnchorPosY(0, _buttonsDur).From(new Vector2(0, 100))
            .SetEase(Ease.OutBack);

        _menuSequence = DOTween.Sequence();

        _menuSequence
            .Append(title.transform.DOScale(1, _titleDur).From(5).SetEase(Ease.OutBack))
            .Append(playButton.DOFade(1f, _buttonsFadeDur))
            .Append(optionsButton.DOFade(1f, _buttonsFadeDur))
            .Append(extrasButton.DOFade(1f, _buttonsFadeDur))
            .Append(leaveButton.DOFade(1f, _buttonsFadeDur));
    }


    private void OnDestroy()
    {
        ClearAnimations();
    }


    public void ClearAnimations()
    {
        _titleTween?.Kill();
        _buttonsTween?.Kill();
        _menuSequence?.Kill();
    }
}
