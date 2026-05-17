using UnityEngine;
using UnityEngine.UI;


public class BubbleShieldTimer : MonoBehaviour
{
    [SerializeField] private WaterGoop waterGoop;
    [SerializeField] private float cooldownTime; 
    private Image _bubbleImage;


    private void Awake()
    {
        _bubbleImage = GetComponentInChildren<Image>();
    }


    private void Update()
    {
        _bubbleImage.fillAmount -= 1 / cooldownTime * Time.deltaTime;
        if (_bubbleImage.fillAmount <= 0)
        {
            _bubbleImage.fillAmount = 1;
            gameObject.SetActive(false);
            waterGoop.BubbleShield.SetActive(false);
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
