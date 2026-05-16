using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;


    private void Start()
    {
        _slider = GetComponent<Slider>();
    }


    public void SetHealth(int _health)
    {
        _slider.value = _health;
    }
}
