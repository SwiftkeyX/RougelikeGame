using UnityEngine;

public class PlayerStat : MonoBehaviour, IHealth
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _baseAttack;

    // getter and setter
    public float CurrentHealth { get { return _currentHealth; } }
    public float MaxHealth { get { return _maxHealth; } }
    public float CurrentHealthPercentage { get { return _currentHealth / _maxHealth; } }
    public float BaseAttack { get { return _baseAttack; } }

    // override
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
    }
}