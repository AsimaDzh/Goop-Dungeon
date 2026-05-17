using System;
using UnityEngine;

public class GoopStats : MonoBehaviour, IDamageable
{
    [Header("========== References ===========")]
    [SerializeField] private GoopData playerData;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private WaterGoop waterGoop;
    [SerializeField] private BubbleShieldTimer bubbleShieldTimer;

    [Header("========== Current Stats (Runtime) ==========")]
    [SerializeField] private float currentHealth;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0f;


    private void Awake()
    {
        InitializeFromData();
    }


    private void InitializeFromData()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerStats: PlayerData is null!", this);
            return;
        }
        currentHealth = Mathf.Clamp(playerData.MaxHealth, 1f, float.MaxValue);

        OnHealthChanged?.Invoke(currentHealth, playerData.MaxHealth);
    }


    private void Start()
    {
        healthBar.SetMaxHealth(playerData.MaxHealth);
    }


    public void TakeDamage(float damage)
    {
        if (damage <= 0f || currentHealth <= 0f) return;

        if (waterGoop.BubbleTimerUI.activeInHierarchy)
        {
            bubbleShieldTimer.ReduceTime(damage);
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, playerData.MaxHealth);

        OnHealthChanged?.Invoke(currentHealth, playerData.MaxHealth);
        healthBar.SetHealth(currentHealth);
        
        Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{playerData.MaxHealth}");

        if (currentHealth <= 0f) OnDeath?.Invoke();

    }
}
