using UnityEngine;
using UnityEngine.UI;

public class BubbleShieldTimer : MonoBehaviour
{
    [SerializeField] private WaterGoop waterGoop;
    [SerializeField] private float cooldownTime; 
    private Image _bubbleImage;
}
