using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : ActorController
{
    private float moveDirection;

    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        if (combat.CombatStateMachine.CurrState == null && movement.MovementStateMachine.CurrState is GroundedState)
        {
            movement.Move(moveDirection);
        }
    }

    public void Move(float direction)
    {
        moveDirection = direction;
    }

    public void JumpGrounded()
    {
        movement.JumpGrounded();
    }

    public void Attack()
    {
        combat.Attack(movement.IsFacingRight);
    }

    public void OnExecuteAttackEffect()
    {
        combat.ExecuteAttackEffect();
    }
}
