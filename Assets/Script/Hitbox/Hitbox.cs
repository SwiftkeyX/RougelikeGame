using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// attach this to the weapon
/// add collider, rigidbody to weapon too
/// </summary>
public class Hitbox : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    private HashSet<IHealth> _hitTargets = new HashSet<IHealth>();
  
    /// <summary>
    /// collider that stick to weapon
    /// collider that stick to enemy (so enemy can punch, kick)
    /// collider that spawn in world space (so enemy can do big slam on the ground)
    /// collider that spawn with VFX (so enemy can shoot fireball and fireball can do damage)
    /// </summary>
    void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// I want this to contain logic for apply damage to other object
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        IHealth health = other.gameObject.GetComponent<IHealth>();
        if (health == null) return;

        // return, if already hit during this attack
        if (_hitTargets.Contains(health)) return;

        // add other to the alreadyHitList
        _hitTargets.Add(health);

        // apply dmg to the other
        health.TakeDamage(10f);
    }

    // ============================ Animator function ==============================
    // call this once when start attack
    public void EnableHitbox()
    {
        _hitTargets.Clear();
        _collider.enabled = true;
    }
    // called this once when attack end
    public void DisableHitbox()
    {
        _collider.enabled = false;
    }
}