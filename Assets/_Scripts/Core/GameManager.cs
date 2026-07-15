using UnityEngine;


public enum GameState
{
    Menu = 0,
    Playing = 1,
    Paused = 2,
    Lost = 3,
    Won = 4
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.Menu;
    
    [SerializeField] private GameUIManager gameUIManager;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    public void StartGame()
    {
        RestartGameScene();
    }


    public void GoToMenu()
    {
        CurrentState = GameState.Menu;
        Time.timeScale = 1f;
        SceneLoader.Instance.Load(SceneNames.MainMenu);

        if (InputManager.Instance != null)
            InputManager.Instance.EnableUIInput();

        Debug.Log("Go to Main Menu");
    }


    public void Pause()
    {
        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.Paused;
        Time.timeScale = 0f;
        EventBus.Instance.RaiseGamePaused();

        AudioManager.PlaySound(AudioType.GamePause);
        Debug.Log("Game Paused");
    }


    public void Resume()
    {
        if (CurrentState != GameState.Paused) return;

        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        EventBus.Instance.RaiseGameResumed();

        Debug.Log("Game resumed");
    }


    public void RestartGameScene()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        SceneLoader.Instance.Load(SceneNames.GameScene);
        
        if (InputManager.Instance != null)
            InputManager.Instance.EnablePlayerInput();

        Debug.Log("Game scene restart requested");
    }


    public void EnterLoseState()
    {
        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.Lost;
        Time.timeScale = 0f;

        if (InputManager.Instance != null)
            InputManager.Instance.EnableUIInput();

        if (gameUIManager == null)
            FindFirstObjectByType<GameUIManager>();

        gameUIManager.ShowGameOverUI();
        AudioManager.PlaySound(AudioType.GameOver);
        AudioManager.StopMusic();
        Debug.Log("Game lost");
    }


    public void EnterWinState()
    {
        if (CurrentState != GameState.Playing) return;

        CurrentState = GameState.Won;
        Time.timeScale = 0f;

        if (InputManager.Instance != null)
            InputManager.Instance.EnableUIInput();

        if (gameUIManager == null)
            FindFirstObjectByType<GameUIManager>();

        gameUIManager.ShowWinUI();
        AudioManager.PlaySound(AudioType.GameWin);
        AudioManager.StopMusic();
        Debug.Log("Game won");
    }
}

