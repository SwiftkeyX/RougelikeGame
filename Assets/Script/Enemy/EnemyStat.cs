using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    // general
    private Health _health;

    // combat
    private float _attackDamge;
    private float _attackRange = 3f;

    // movement
    private float _speedPerFrame = 0.5f;
    private float _rotationPerFrame = 15f;

    // setter and getter
    public Health Health { get { return _health; } }
    public float AttackDamage { get { return _attackDamge; } }
    public float AttackRange { get { return _attackRange; } }
    public float SpeedPerFrame { get { return _speedPerFrame; } }
    public float RotationPerFrame { get { return _rotationPerFrame; } }

    void Awake()
    {
        _health = GetComponent<Health>();
    }


}