using UnityEngine;
using DG.Tweening;

public class GameUIManager : MonoBehaviour
{
    [Header("========== GameOver UI ==========")]
    [SerializeField] private CanvasGroup gameOverBackground;
    [SerializeField] private CanvasGroup gameOverText;
    [SerializeField] private CanvasGroup gameOverButtons;

    private float _gameOverDur = 1f;
    private Sequence _gameOverSequence;

    [Header("========== Win UI ==========")]
    [SerializeField] private CanvasGroup winBackground;
    [SerializeField] private CanvasGroup winText;
    [SerializeField] private CanvasGroup menuButton;

    private float _winDur = 0.5f;
    private Sequence _winSequence;


    private void Awake()
    {
        gameOverBackground.alpha = 0f;
        gameOverText.alpha = 0f;
        gameOverButtons.alpha = 0f;
        winBackground.alpha = 0f;
        winText.alpha = 0f;
        menuButton.alpha = 0f;
    }


    public void ShowGameOverUI()
    {
       _gameOverSequence = DOTween.Sequence();

       _gameOverSequence
           .Append(gameOverBackground.DOFade(1f, _gameOverDur).SetUpdate(true))
           .Append(gameOverText.DOFade(1f, _gameOverDur).SetUpdate(true))
           .Append(gameOverButtons.DOFade(1f, _gameOverDur).SetUpdate(true));
    }


    public void ShowWinUI()
    {
        _winSequence = DOTween.Sequence();

        _winSequence
            .Append(winBackground.DOFade(1f, _winDur).SetUpdate(true))
            .Append(winText.DOFade(1f, _winDur).SetUpdate(true))
            .Append(menuButton.DOFade(1f, _winDur).SetUpdate(true));
    }


    private void OnDestroy()
    {
        ClearAnimations();
    }


    private void ClearAnimations()
    {
        _gameOverSequence?.Kill();
        _winSequence?.Kill();
    }
}
