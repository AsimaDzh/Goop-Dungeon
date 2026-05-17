using UnityEngine;
using UnityEngine.UI;


public class BubbleShieldTimer : MonoBehaviour
{
    [SerializeField] private WaterGoop waterGoop;
    [SerializeField] private float cooldownTime; 
    private Image _bubbleImage;
    private bool _isCooldown;


    private void Awake()
    {
        _bubbleImage = GetComponentInChildren<Image>();
    }


    private void Start()
    {
        _isCooldown = true;
    }


    private void Update()
    {
        if (_isCooldown)
        {
            _bubbleImage.fillAmount -= 1 / cooldownTime * Time.deltaTime;
            if (_bubbleImage.fillAmount <= 0)
            {
                _bubbleImage.fillAmount = 1;
                _isCooldown = false;
                gameObject.SetActive(false);
                waterGoop.BubbleShield.SetActive(false);
            }
        }
    }


    public void ResetTimer()
    {
        _bubbleImage.fillAmount = 1;
    }


    public void ReduceTime(float _damage)
    {
        _bubbleImage.fillAmount -= _damage / 100f;
    }
}
