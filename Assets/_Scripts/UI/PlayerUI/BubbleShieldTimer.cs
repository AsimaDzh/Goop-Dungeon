using System;
using UnityEngine;
using UnityEngine.UI;


public class BubbleShieldTimer : MonoBehaviour, IBubbleShield
{
    [SerializeField] private float cooldownTime; 
    private Image _bubbleImage;

    public event Action OnTimerEnded;
    public bool IsActive => gameObject.activeSelf;


    private void Awake()
    {
        _bubbleImage = GetComponentInChildren<Image>();
    }


    private void Update()
    {
        if(!_bubbleImage) return;

        _bubbleImage.fillAmount -= 1 / cooldownTime * Time.deltaTime;
        if (_bubbleImage.fillAmount <= 0)
        {
            _bubbleImage.fillAmount = 1;
            gameObject.SetActive(false);
            OnTimerEnded?.Invoke();
        }
    }


    public void ResetTimer()
    {
        if (_bubbleImage) 
            _bubbleImage.fillAmount = 1;
    }


    public void ReduceTime(float _damage)
    {
        if (_bubbleImage) 
            _bubbleImage.fillAmount -= _damage / 100f;
    }
}
