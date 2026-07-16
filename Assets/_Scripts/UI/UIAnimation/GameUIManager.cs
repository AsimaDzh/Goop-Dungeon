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
            .Append(winBackground.DOFade(1f, _winDur))
            .Append(winText.DOFade(1f, _winDur))
            .Append(menuButton.DOFade(1f, _winDur));

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
