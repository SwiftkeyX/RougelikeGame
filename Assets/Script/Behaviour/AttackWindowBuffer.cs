// using UnityEngine;

// /// <summary>
// /// Attach this to each attack state 
// /// </summary>
// public class AttackWindowBuffer : StateMachineBehaviour
// {
//     [SerializeField] private PlayerStateMachine _ctx;
//     private float attackWindowStart;
//     private float attackWindowEnd;

//     // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
//     override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {
//         int AttackID = animator.GetInteger("AttackID");

//         // get window timing for this specific AttackID
//         attackWindowStart = _ctx.Weapon.Buffer.AttackWindows[AttackID].start;
//         attackWindowEnd = _ctx.Weapon.Buffer.AttackWindows[AttackID].end;
//     }

//     // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
//     override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {
//         float t = stateInfo.normalizedTime;
//         float allowBufferWindow = Mathf.Max(0f, _ctx.Weapon.Buffer.AttackWindows[_ctx.AttackIDValue].start - 0.3f);

//         // chain-attack when AllowChainAttack and Attack is buffered
//         if (_ctx.AllowChainAttack && _ctx.IsAttackBufferValue)
//         {
//             _ctx.Animator.SetInteger(_ctx.AttackIDHash, _ctx.AttackIDValue + 1);
//         }

//         // allow buffer-window
//         else if (t <= allowBufferWindow) _ctx.Animator.SetBool(_ctx.IsAttackBufferHash, true);

//         // allow chain-attack if timing is correct
//         else if (t >= attackWindowStart && t <= attackWindowEnd) { _ctx.AllowChainAttack = true; }

//         // not allow chain attack
//         else if (t > attackWindowEnd) { _ctx.AllowChainAttack = false; }
//     }

//     // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
//     override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {
//         _ctx.Animator.SetBool(_ctx.IsAttackBufferHash, false);
//     }

//     // OnStateMove is called right after Animator.OnAnimatorMove()
//     //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     //{
//     //    // Implement code that processes and affects root motion
//     //}

//     // OnStateIK is called right after Animator.OnAnimatorIK()
//     //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     //{
//     //    // Implement code that sets up animation IK (inverse kinematics)
//     //}
// }
