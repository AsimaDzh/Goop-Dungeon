using System;
using UnityEngine;

public class GoopStats : MonoBehaviour
{
    [Header("========== Player Data ===========")]
    public GoopData playerData;

    [Header("========== Current Stats (Runtime) ==========")]
    [SerializeField] private float currentHealth;

    public float CurrentHealth => currentHealth;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;


    private void Awake()
    {
        InitializeFromData();
    }


    public void InitializeFromData()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerStats: PlayerData is null!", this);
            return;
        }
        currentHealth = Mathf.Clamp(playerData.MaxHealth, 1f, float.MaxValue);

        OnHealthChanged?.Invoke(currentHealth, playerData.MaxHealth);
    }


    public void TakeDamage(float amount)
    {
        if (amount <= 0f || currentHealth <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, playerData.MaxHealth);

        OnHealthChanged?.Invoke(currentHealth, playerData.MaxHealth);

        if (currentHealth <= 0f) OnDeath?.Invoke();
    }
}
