using UnityEngine;
using DG.Tweening;

public class GameUIManager : MonoBehaviour
{
    [Header("========== Game Over UI ==========")]
    [SerializeField] private CanvasGroup gameOverBackground;
    [SerializeField] private CanvasGroup gameOverText;
    [SerializeField] private CanvasGroup gameOverButtons;

    private float _gameOverDur = 1f;
    private Sequence _gameOverSequence;

    [Header("========== Game Win UI ==========")]
    [SerializeField] private CanvasGroup winBackground;
    [SerializeField] private CanvasGroup winTextAndButton;

    private float _winBackgroundDur = 2f;
    private float _textAndButtonDur = 0.2f;
    private Sequence _winSequence;


    private void Awake()
    {
        gameOverBackground.alpha = 0f;
        gameOverText.alpha = 0f;
        gameOverButtons.alpha = 0f;
        winBackground.alpha = 0f;
        winTextAndButton.alpha = 0f;
    }


    public void ShowGameOverUI()
    {
        Debug.Log("GameUIManager: GameOver animation called");

        _gameOverSequence?.Kill();

        _gameOverSequence = DOTween.Sequence().SetUpdate(true);

       _gameOverSequence
           .Append(gameOverBackground.DOFade(1f, _gameOverDur))
           .Append(gameOverText.DOFade(1f, _gameOverDur))
           .Append(gameOverButtons.DOFade(1f, _gameOverDur));

        _gameOverSequence.Play();
    }


    public void ShowWinUI()
    {
        Debug.Log("GameUIManager: Win animation called");
        
        _winSequence?.Kill();

        _winSequence = DOTween.Sequence().SetUpdate(true);

        _winSequence
            .Append(winBackground.DOFade(1f, _winBackgroundDur))
            .Append(winTextAndButton.DOFade(1f, _textAndButtonDur));

        _winSequence.Play();
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
