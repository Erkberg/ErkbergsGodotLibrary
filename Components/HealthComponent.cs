using Godot;
using System;

public partial class HealthComponent : Node
{
    [Export] public float MaxHealth { get; set; } = 1;
    [Export] public float CurrentHealth { get; set; } = 1;

    public event Action Died;
    public event Action HealthChanged;
    public event Action HealthIncreased;
    public event Action HealthDecreased;

    public void Damage(float amount)
    {
        float previousHealth = CurrentHealth;
        CurrentHealth -= amount;
        ClampCurrentHealth();

        if (CurrentHealth != previousHealth)
        {
            HealthDecreased?.Invoke();
            HealthChanged?.Invoke();
        }

        if (CurrentHealth <= 0)
        {
            Died?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        float previousHealth = CurrentHealth;

        CurrentHealth += amount;
        ClampCurrentHealth();

        if (CurrentHealth != previousHealth)
        {
            HealthIncreased?.Invoke();
            HealthChanged?.Invoke();
        }
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth;
    }

    private void ClampCurrentHealth()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
    }
}