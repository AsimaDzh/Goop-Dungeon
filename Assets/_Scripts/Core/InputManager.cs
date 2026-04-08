using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputActionAsset inputActions;

    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;

    private InputAction pauseAction;
    private InputAction cancelAction;

    public System.Action OnPausePressed;
    public System.Action OnCancelPressed;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        InitializeInputSystem();
    }


    private void InitializeInputSystem()
    {
        if (inputActions == null)
        {
            Debug.LogError("InputManager: there's no Input Actions Asset!");
            return;
        }

        uiActionMap = inputActions.FindActionMap("UI");
        playerActionMap = inputActions.FindActionMap("Player");

        if (playerActionMap == null || uiActionMap == null)
        {
            Debug.LogError("InputManager: Action Map 'Player' or 'UI' is not found!");
            return;
        }

        pauseAction = playerActionMap.FindAction("Pause");
        cancelAction = uiActionMap.FindAction("Cancel");

        if (pauseAction != null)
            pauseAction.performed += OnPausePerformed;
        if (cancelAction != null)
            cancelAction.performed += OnCancelPerformed;

        EnablePlayerInput();
    }


    private void OnEnable()
    {
        if (inputActions != null)
            inputActions.Enable();

        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnGamePaused += HandleGamePaused;
            EventBus.Instance.OnGameResumed += HandleGameResumed;
        }
    }


    private void OnDisable()
    {
        if (inputActions != null)
            inputActions.Disable();

        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnGamePaused -= HandleGamePaused;
            EventBus.Instance.OnGameResumed -= HandleGameResumed;
        }
    }


    private void OnDestroy()
    {
        if (pauseAction != null)
            pauseAction.performed -= OnPausePerformed;
        if (cancelAction != null)
            cancelAction.performed -= OnCancelPerformed;
    }


    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        OnPausePressed?.Invoke();
    }


    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        OnCancelPressed?.Invoke();
    }

   
    public void EnablePlayerInput()
    {
        if (playerActionMap != null)
            playerActionMap.Enable();
        if (uiActionMap != null)
            uiActionMap.Disable();
    }


    public void EnableUIInput()
    {
        if (playerActionMap != null)
            playerActionMap.Disable();
        if (uiActionMap != null)
            uiActionMap.Enable();
    }


    private void HandleGamePaused()
    {
        if (playerActionMap != null)
            playerActionMap.Disable();

        Debug.Log("InputManager: Player input disabled (game paused)");
    }


    private void HandleGameResumed()
    {
        if (playerActionMap != null)
            playerActionMap.Enable();

        Debug.Log("InputManager: Player input enabled (game resumed)");
    }
}
