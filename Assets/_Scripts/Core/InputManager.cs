using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputActionAsset inputActions;

    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction interactAction;

    private InputAction pauseAction;
    private InputAction cancelAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public float ZoomInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool CrouchHeld { get; private set; }

    public System.Action OnJumpPressed;
    public System.Action OnAttackPressed;
    public System.Action OnInteractPressed;
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

        playerActionMap = inputActions.FindActionMap("Player");
        uiActionMap = inputActions.FindActionMap("UI");

        if (playerActionMap == null)
        {
            Debug.LogError("InputManager: Action Map 'Player' is not found!");
            return;
        }

        moveAction = playerActionMap.FindAction("Move");
        jumpAction = playerActionMap.FindAction("Jump");
        attackAction = playerActionMap.FindAction("Attack");
        interactAction = playerActionMap.FindAction("Interact");

        pauseAction = playerActionMap.FindAction("Pause");
        cancelAction = uiActionMap.FindAction("Cancel");


        if (jumpAction != null)
            jumpAction.performed += OnJumpPerformed;
        if (attackAction != null)
            attackAction.performed += OnAttackPerformed;
        if (interactAction != null)
            interactAction.performed += OnInteractPerformed;
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
        if (jumpAction != null)
            jumpAction.performed -= OnJumpPerformed;
        if (attackAction != null)
            attackAction.performed -= OnAttackPerformed;
        if (interactAction != null)
            interactAction.performed -= OnInteractPerformed;
        if (pauseAction != null)
            pauseAction.performed -= OnPausePerformed;
        if (cancelAction != null)
            cancelAction.performed -= OnCancelPerformed;
    }


    private void Update()
    {
        UpdateInputValues();
    }


    private void UpdateInputValues()
    {
        MoveInput = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
    }


    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpPressed = true;
        OnJumpPressed?.Invoke();
    }


    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        AttackPressed = true;
        OnAttackPressed?.Invoke();
    }


    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        InteractPressed = true;
        OnInteractPressed?.Invoke();
    }


    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        OnPausePressed?.Invoke();
    }


    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        OnCancelPressed?.Invoke();
    }


    public void ResetButtonFlags()
    {
        JumpPressed = false;
        AttackPressed = false;
        InteractPressed = false;
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


    public Vector2 GetMoveInput()
    {
        return MoveInput;
    }


    public bool IsJumpPressed()
    {
        return JumpPressed;
    }


    public bool IsInteractPressed()
    {
        return InteractPressed;
    }
}
