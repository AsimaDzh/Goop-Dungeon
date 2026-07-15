using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [Header("========== GameOver UI ==========")]
    [SerializeField] private CanvasGroup gameOverBackground;
    [SerializeField] private CanvasGroup gameOverText;
    [SerializeField] private CanvasGroup gameOverButtons;
    private float _gameOverDur = 1f;

    [Header("========== Win UI ==========")]
    [SerializeField] private CanvasGroup winBackground;
    [SerializeField] private CanvasGroup winText;
    [SerializeField] private CanvasGroup menuButton;
    private float _winDur = 0.5f;
}
