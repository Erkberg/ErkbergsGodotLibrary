using Godot;
using System;

public partial class HealthComponent : Node
{
    [Export] public float MaxHealth { get; set; } = 1;
    [Export] public float CurrentHealth { get; set; } = 1;

    public Action Died;
    public Action HealthChanged;
    public Action HealthIncreased;
    public Action HealthDecreased;

    public void Damage(float amount)
    {
        if (CurrentHealth > 0)
        {
            HealthDecreased?.Invoke();
            HealthChanged?.Invoke();
        }

        CurrentHealth -= amount;
        ClampCurrentHealth();

        if (CurrentHealth <= 0)
        {
            Died?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (CurrentHealth < MaxHealth)
        {
            HealthIncreased?.Invoke();
            HealthChanged?.Invoke();
        }

        CurrentHealth += amount;
        ClampCurrentHealth();
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