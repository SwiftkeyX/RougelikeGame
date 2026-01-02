using UnityEngine;

public class GizmoContext : MonoBehaviour
{
    private EnemyStateMachine _ctx;

    void Awake()
    {
        _ctx = GetComponent<EnemyStateMachine>();
    }

    public void OnDrawGizmosSelected()
    {
        // gizmo run even in editor mode, so it print out a lot of error bc it dont have _ctx
        // it annoy me, so let's disable it
        if (!Application.isPlaying) return;

        /// <summary>
        /// Draw IsGroundedSphereCast() from groundedState
        /// </summary>
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_ctx.SphereOrigin, _ctx.SphereOrigin + Vector3.down * _ctx.GroundCheckDistance);
        Gizmos.DrawWireSphere(_ctx.SphereOrigin, _ctx.GroundedSphereRadius);
        Gizmos.DrawWireSphere(_ctx.SphereOrigin + Vector3.down * _ctx.GroundCheckDistance, _ctx.GroundedSphereRadius);
    }
}
