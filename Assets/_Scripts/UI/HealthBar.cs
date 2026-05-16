using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;


    private void Start()
    {
        _slider = GetComponent<Slider>();
    }


    public void SetMaxHealth(float _health)
    {
        _slider.maxValue = _health;
        _slider.value = _health;
    }


    public void SetHealth(float _health)
    {
        _slider.value = _health;
    }
}
