using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace CombatStates
{
    public class RangedAttackState : AttackState
    {
        private readonly RangedProjectile projectilePrefab;
        private readonly Transform projectileOrigin;
        private readonly Vector2 attackDirection;
        private readonly LayerMask actorHitLayer;
        private RangedProjectileEvent fireRangedProjectileEvent;
        private readonly float attackCost;

        public RangedAttackState(
            Combat owner, RangedProjectile projectilePrefab,
            Transform projectileOrigin, Vector2 attackDirection, LayerMask actorHitLayer,
            RangedProjectileEvent fireRangedProjectileEvent,
            float attackCost) : base(owner)
        {
            this.projectilePrefab = projectilePrefab;
            this.projectileOrigin = projectileOrigin;
            this.attackDirection = attackDirection.normalized;
            this.actorHitLayer = actorHitLayer;
            this.fireRangedProjectileEvent = fireRangedProjectileEvent;
            this.attackCost = attackCost;
        }

        public override void Enter()
        {
            AudioManagerSynced.Instance.PlaySoundFx(owner.SoundFXIndexLibrary.Attack);
            base.Enter();
            if (Vector2.Angle(Vector2.up, attackDirection) > 170f)
            {
                owner.Animator.SetBool("isAttackingDown", true);
            }
        }

        public override void ExecuteAttackEffect()
        {
            owner.Resource.Consume(attackCost);

            RangedProjectile projectile = PhotonNetwork.Instantiate(
                projectilePrefab.name,
                projectileOrigin.position,
                RangedProjectile.GetRotationForDirection(attackDirection)).GetComponent<RangedProjectile>();

            projectile.Direction = attackDirection;
            projectile.ActorTargetsLayer = actorHitLayer;
            fireRangedProjectileEvent.Invoke(projectile);
        }

        public override void Exit()
        {
            base.Exit();
            owner.Animator.SetBool("isAttackingDown", false);
        }
    }
}
