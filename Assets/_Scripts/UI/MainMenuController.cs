using UnityEngine;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonExit;


    private void Start()
    {
        buttonPlay.onClick.AddListener(() => GameManager.Instance.StartGame());
        buttonExit.onClick.AddListener(() => Application.Quit());
    }
}
