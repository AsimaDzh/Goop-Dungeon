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

    private float _titleDuration = 0.7f;
    private float _buttonsDuration = 0.3f;

    private Sequence _menuSequence;


    private void Awake()
    {
        playButton.alpha = 0f;
        optionsButton.alpha = 0f;
        extrasButton.alpha = 0f;
        leaveButton.alpha = 0f;
    }


    private void Start()
    {
        ShowMenuButtons();
    }


    private void ShowMenuButtons()
    {
        _menuSequence = DOTween.Sequence();

        _menuSequence
            .Append(titleFade.transform.DOScale(1, _titleDuration).From(5).SetEase(Ease.OutBack))
            .Append(playButton.DOFade(1f, _buttonsDuration))
            .Append(optionsButton.DOFade(1f, _buttonsDuration))
            .Append(extrasButton.DOFade(1f, _buttonsDuration))
            .Append(leaveButton.DOFade(1f, _buttonsDuration));
    }


    private void OnDestroy()
    {
        ClearAnimations();
    }


    public void ClearAnimations()
    {
        _menuSequence?.Kill();
    }
}
