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
        private RangedProjectileEvent fireRangedProjectileEvent;

        public RangedAttackState(
            Combat owner, RangedProjectile projectilePrefab,
            Transform projectileOrigin, Vector2 attackDirection,
            RangedProjectileEvent fireRangedProjectileEvent) : base(owner)
        {
            this.projectilePrefab = projectilePrefab;
            this.projectileOrigin = projectileOrigin;
            this.attackDirection = attackDirection.normalized;
            this.fireRangedProjectileEvent = fireRangedProjectileEvent;
        }

        public override void Enter()
        {
            base.Enter();
            if (Vector2.Angle(Vector2.up, attackDirection) > 170f)
            {
                owner.Animator.SetBool("isAttackingDown", true);
            }
        }

        public override void ExecuteAttackEffect()
        {
            RangedProjectile projectile = PhotonNetwork.Instantiate(
                projectilePrefab.name,
                projectileOrigin.position,
                RangedProjectile.GetRotationForDirection(attackDirection)).GetComponent<RangedProjectile>();

            projectile.Direction = attackDirection;
            fireRangedProjectileEvent.Invoke(projectile);
        }

        public override void Exit()
        {
            base.Exit();
            owner.Animator.SetBool("isAttackingDown", false);
        }
    }
}
