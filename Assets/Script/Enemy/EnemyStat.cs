using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "EnemyStat", menuName = "SO/Enemy/EnemyStat")]
public class EnemyStat : ScriptableObject
{
    // general
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;

    // combat
    [Header("Combat")]
    [SerializeField] private EnemyAttackData _enemyAttackdata;
    [SerializeField] private float _considerAttackRange;
    // private bool[] _currentCooldown = new bool[5] { false, false, false, false, false };
    private float[] _currentCooldown;

    // movement
    [Header("Movement")]
    [SerializeField] private float _speedPerFrame = 0.5f;
    [SerializeField] private float _rotationPerFrame = 15f;

    // setter and getter
    public float CurrentHealth { get { return _currentHealth; } }
    public float MaxHealth { get { return _maxHealth; } }
    public float SpeedPerFrame { get { return _speedPerFrame; } }
    public float RotationPerFrame { get { return _rotationPerFrame; } }
    public EnemyAttackType GetEnemyAttack(ENEMYATTACKTYPE type) => _enemyAttackdata.Get(type);
    public float ConsiderAttackRange { get { return _considerAttackRange; } }

    /// <summary>
    /// Get and Set Cooldown of enemy's attack
    /// </summary>
    public void StartCooldown(ENEMYATTACKTYPE type)
    {
        _currentCooldown[(int)type] = GetEnemyAttack(type).maxCooldown;

    }
    public void TickCooldown(ENEMYATTACKTYPE type)
    {
        _currentCooldown[(int)type] -= Time.deltaTime;
        _currentCooldown[(int)type] = Mathf.Max(0f, _currentCooldown[(int)type]); // cap the cooldown at 0
    }
    public bool IsFinishCooldown(ENEMYATTACKTYPE type)
    {
        float currentCooldown = _currentCooldown[(int)type];

        if (currentCooldown == 0f) return true;

        return false;
    }
    public float GetCurrentCooldownPercentage(ENEMYATTACKTYPE type)
    {
        return _currentCooldown[(int)type] / GetEnemyAttack(type).maxCooldown;
    }
    public void GetAllCurrentCooldown()
    {
        Debug.Log("cooldown: " + string.Join(", ", _currentCooldown.Select(w => w.ToString())));
    }

    /// <summary>
    /// Health related
    /// </summary>
    public void TakeDamge(float damage)
    {
        this._currentHealth -= damage;
    }
    public float GetHealthPercentage()
    {
        return _currentHealth / _maxHealth;
    }

    /// <summary>
    /// because the cooldown is on SO, the SO will never reset itself even after we stop the play mode, 
    /// so we reset cooldown manaully using OnEnable()
    /// </summary>
    private void OnEnable()
    {
        _currentCooldown = new float[5] { 0f, 0f, 0f, 0f, 0f };

        Debug.Log("cooldown: " + string.Join(", ", _currentCooldown.Select(w => w.ToString())));
    }
}

