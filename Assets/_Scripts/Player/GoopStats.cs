using System;
using UnityEngine;

public class GoopStats : MonoBehaviour
{
    [Header("========== Player Data ===========")]
    public GoopData playerData;

    [Header("========== Current Stats (Runtime) ==========")]
    [SerializeField] private float currentHealth;

    [Tooltip("Current mana or player energy")]
    [SerializeField] private float currentMana;

    public float CurrentHealth => currentHealth;
    public float CurrentMana => currentMana;

    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnManaChanged;
    public event Action OnDeath;
}
