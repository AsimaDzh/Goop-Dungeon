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

    private Sequence _menuSequence;


    private void Awake()
    {
        titleFade.alpha = 0f;
        playButton.alpha = 0f;
        optionsButton.alpha = 0f;
        extrasButton.alpha = 0f;
        leaveButton.alpha = 0f;
    }


    private void Start()
    {
        ShowMenu();
    }


    private void ShowMenu()
    {
        _menuSequence = DOTween.Sequence();

        _menuSequence
            .Append(titleFade.DOFade(1f, _fadeDurTitle))
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
        _menuSequence?.Kill();
    }
}
