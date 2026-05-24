using UnityEngine;
using UnityEngine.UI;
using System;


public class GameLoopFlowController : MonoBehaviour
{
    [Header("========== GameOver Panel =========")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button menuButton;

    [Header("========== Win Panel ==========")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button winMenuButton;

    [Header("========== Shared UI ==========")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GoopStats _playerStats;


    private void Awake()
    {
        if (_playerStats == null)
            _playerStats = FindFirstObjectByType<GoopStats>();

        ValidateReferences();

        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
    }


    private void OnEnable()
    {
        SubscribeToPlayerDeath();
        BindButtons();
    }


    private void OnDisable()
    {
        UnsubscribeFromPlayerDeath();
        UnbindButtons();
    }


    public void RequestWinFromExit()
    {
        if (!CanTriggerWin()) return;

        TriggerWin();
    }


    private void ValidateReferences()
    {
        if (gameOverPanel == null)
            Debug.LogError($"{name}: losePanel is not assigned.", this);

        if (tryAgainButton == null)
            Debug.LogError($"{name}: loseRestartButton is not assigned.", this);

        if (menuButton == null)
            Debug.LogError($"{name}: loseMenuButton is not assigned.", this);

        if (winPanel == null)
            Debug.LogError($"{name}: winPanel is not assigned.", this);

        if (winMenuButton == null)
            Debug.LogError($"{name}: winMenuButton is not assigned.", this);

        if (pausePanel == null)
            Debug.LogError($"{name}: pausePanel is not assigned.", this);
    }


    private void BindButtons()
    {
        tryAgainButton.onClick.AddListener(HandleLoseRestartClicked);
        menuButton.onClick.AddListener(HandleMenuClicked);
        winMenuButton.onClick.AddListener(HandleMenuClicked);
    }


    private void UnbindButtons()
    {
        tryAgainButton.onClick.RemoveListener(HandleLoseRestartClicked);
        menuButton.onClick.RemoveListener(HandleMenuClicked);
        winMenuButton.onClick.RemoveListener(HandleMenuClicked);
    }


    private void SubscribeToPlayerDeath()
    {
        if (_playerStats == null)
        {
            Debug.LogWarning($"{name}: PlayerStats not found. Lose on death is disabled.", this);
            return;
        }

        _playerStats.OnDeath += HandlePlayerDeath;
    }


    private void UnsubscribeFromPlayerDeath()
    {
        if (_playerStats != null)
            _playerStats.OnDeath -= HandlePlayerDeath;
    }


    private bool CanTriggerWin()
    {
        if (GameManager.Instance == null) 
            return false;

        return GameManager.Instance.CurrentState == GameState.Playing;
    }


    private void HandlePlayerDeath()
    {
        pausePanel.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.EnterLoseState();

        gameOverPanel.SetActive(true);

        Debug.Log($"{name}: lose screen shown.", this);
    }


    private void TriggerWin()
    {
        pausePanel.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.EnterWinState();

        winPanel.SetActive(true);

        Debug.Log($"{name}: win screen shown.", this);
    }


    private void HandleLoseRestartClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartGameScene();
    }


    private void HandleMenuClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.GoToMenu();
    }
}
