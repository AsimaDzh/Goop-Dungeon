using UnityEngine;
using UnityEngine.UI;

public class GameLoopFlowController : MonoBehaviour
{
    [Header("========== GameOver Panel =========")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button menuButton;

    [Header("========== Win Panel ==========")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button continueButton;

    [Header("========== Shared UI ==========")]
    [SerializeField] private GameObject pauseMenu;

    [Header("========== Win Condition ==========")]
    [SerializeField] private GameObject exitActivationObjectOverride;

    private GoopStats _playerStats;
    private bool _flowFinished;
}
