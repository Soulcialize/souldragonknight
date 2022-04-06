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
        private readonly ProjectileLauncher projectileLauncher;
        private readonly Vector2 attackDirection;
        private readonly LayerMask actorHitLayer;
        private readonly float attackCost;
        private readonly RangedProjectileEvent fireRangedProjectileEvent;

        public RangedAttackState(
            Combat owner,
            RangedProjectile projectilePrefab, Transform projectileOrigin, ProjectileLauncher projectileLauncher,
            Vector2 attackDirection, LayerMask actorHitLayer, float attackCost,
            RangedProjectileEvent fireRangedProjectileEvent) : base(owner)
        {
            this.projectilePrefab = projectilePrefab;
            this.projectileOrigin = projectileOrigin;
            this.projectileLauncher = projectileLauncher;
            this.attackDirection = attackDirection.normalized;
            this.actorHitLayer = actorHitLayer;
            this.attackCost = attackCost;
            this.fireRangedProjectileEvent = fireRangedProjectileEvent;
        }

        public override void Enter()
        {
            base.Enter();
            if (Vector2.Angle(Vector2.up, attackDirection) > 170f)
            {
                owner.Animator.SetBool("isAttackingDown", true);
            }

            AudioManagerSynced.Instance.PlaySoundFx(owner.SoundFXIndexLibrary.Attack);
        }

        public override void ExecuteAttackEffect()
        {
            owner.Resource.Consume(attackCost);

            RangedProjectile projectile = PhotonNetwork.Instantiate(
                projectilePrefab.name,
                projectileOrigin.position,
                RangedProjectile.GetRotationForDirection(attackDirection)).GetComponent<RangedProjectile>();

            if (projectileLauncher != null)
            {
                projectileLauncher.Animator.SetBool("isAttacking", true);
            }

            projectile.Direction = attackDirection;
            projectile.ActorTargetsLayer = actorHitLayer;
            fireRangedProjectileEvent.Invoke(projectile);
        }

        public override void Exit()
        {
            base.Exit();
            owner.Animator.SetBool("isAttackingDown", false);
            if (projectileLauncher != null)
            {
                projectileLauncher.HideProjectileLauncher();
                projectileLauncher.Animator.SetBool("isAttacking", false);
            }
        }
    }
}
