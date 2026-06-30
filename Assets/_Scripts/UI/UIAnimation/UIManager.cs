using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup
        titleFade, 
        playButton, 
        optionsButton, 
        extrasButton, 
        leaveButton;

    private float _fadeDurTitle = 2f;
    private float _fadeDurButtons = 1f;
    private float _fadeDurPlayOptionsCreditsLeave = 1f;
    private float _delayBetweenButtons = 0.1f;
}
