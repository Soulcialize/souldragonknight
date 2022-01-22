using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : CombatState
{
    private readonly bool isFacingRight;

    [System.Serializable]
    public class AttackEffectArea
    {
        [SerializeField] private Transform leftOrigin;
        public Transform LeftOrigin { get => leftOrigin; }

        [SerializeField] private Transform rightOrigin;
        public Transform RightOrigin { get => rightOrigin; }

        [SerializeField] private Vector2 size;
        public Vector2 Size { get => size; }
    }

    public bool IsReadying { get; private set; }

    public float AttackReadyTime { get; private set; }

    private int currAttackNumber;

    private Collider2D[] hits;

    public bool IsRecovering { get; private set; }

    public AttackState(Combat owner, bool isFacingRight) : base(owner)
    {
        this.isFacingRight = isFacingRight;
        currAttackNumber = 1;
    }

    public override void Enter()
    {
        IsReadying = true;
        AttackReadyTime = Time.time;
        owner.Rigidbody.velocity = Vector2.zero;
        owner.Animator.SetTrigger("Attack");

        BroadcastImminentAttack();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        owner.Animator.ResetTrigger("Attack");
    }

    private void GetCollidersInAttackRange()
    {
        AttackEffectArea attackEffectAreaData = owner.AttacksEffectAreas[currAttackNumber - 1];
        hits = Physics2D.OverlapBoxAll(
            (isFacingRight ? attackEffectAreaData.RightOrigin : attackEffectAreaData.LeftOrigin).position,
            attackEffectAreaData.Size,
            attackEffectAreaData.LeftOrigin.eulerAngles.z,
            owner.AttackEffectLayer);
    }

    private void BroadcastImminentAttack()
    {
        GetCollidersInAttackRange();

        foreach (Collider2D hit in hits)
        {
            ActorController actorHit = ActorController.GetActorControllerFromActorCollider(hit);
            // TODO: inform actor of imminent attack
        }
    }

    public void ExecuteAttackEffect()
    {
        IsReadying = false;

        GetCollidersInAttackRange();

        List<ActorController> actorsHit = new List<ActorController>();
        foreach (Collider2D hit in hits)
        {
            ActorController actor = ActorController.GetActorControllerFromActorCollider(hit);
            if (actor == null)
            {
                continue;
            }

            actorsHit.Add(actor);
        }

        foreach (ActorController actor in actorsHit)
        {
            if (actor is EnemyController)
            {
                actor.Die();
            }
        }
    }

    public void Recover()
    {
        IsRecovering = true;
    }

    public void InterruptAttack()
    {
        owner.Animator.SetTrigger("InterruptAttack");
    }
}
