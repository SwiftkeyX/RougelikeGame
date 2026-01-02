using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Collider _collider;

    /// <summary>
    /// collider that stick to weapon
    /// collider that stick to enemy (so enemy can punch, kick)
    /// collider that spawn in world space (so enemy can do big slam on the ground)
    /// collider that spawn with VFX (so enemy can shoot fireball and fireball can do damage)
    /// </summary>
    void Awake()
    {
        // _collider = GetComponentInChildren
    }
}