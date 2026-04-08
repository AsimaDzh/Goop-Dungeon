using UnityEngine;
using UnityEngine.InputSystem;


public class BootstrapManager : MonoBehaviour
{
    private static bool _isInitialized;


    private void Awake()
    {
        if (_isInitialized)
        {
            Destroy(gameObject);
            return;
        }

        _isInitialized = true;
        DontDestroyOnLoad(gameObject);

        CreateGameManager();
        CreateSceneLoader();
        CreateEventBus();
        CreateInputManager();

        SceneLoader.Instance.Load(SceneNames.MainMenu);
    }


    private void CreateGameManager()
    {
        GameManager existing = FindFirstObjectByType<GameManager>();
        if (existing != null)
        {
            DontDestroyOnLoad(existing.gameObject);
            return;
        }

        GameObject go = new GameObject("GameManager");
        go.AddComponent<GameManager>();
        DontDestroyOnLoad(go);
    }


    private void CreateSceneLoader()
    {
        SceneLoader existing = FindFirstObjectByType<SceneLoader>();
        if (existing != null)
        {
            DontDestroyOnLoad(existing.gameObject);
            return;
        }

        GameObject go = new GameObject("SceneLoader");
        go.AddComponent<SceneLoader>();
        DontDestroyOnLoad(go);
    }


    private void CreateEventBus()
    {
        EventBus existing = FindFirstObjectByType<EventBus>();
        if (existing != null)
        {
            DontDestroyOnLoad(existing.gameObject);
            return;
        }

        GameObject go = new GameObject("EventBus");
        go.AddComponent<EventBus>();
        DontDestroyOnLoad(go);
    }


    private void CreateInputManager()
    {
        InputManager existing = FindFirstObjectByType<InputManager>();
        if (existing != null)
        {
            DontDestroyOnLoad(existing.gameObject);
            return;
        }
        GameObject go = new GameObject("InputManager");
        InputManager inputManager = go.AddComponent<InputManager>();
        inputManager.inputActions = Resources.Load<InputActionAsset>("PlayerInputAction");

        if (inputManager.inputActions == null)
            Debug.LogError("BootstrapManager: Failed to load PlayerInputAction asset from Resources folder");

        DontDestroyOnLoad(go);
    }
}

