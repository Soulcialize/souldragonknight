using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace CombatStates
{
    public class RangedAttackState : AttackState
    {
        private readonly RangedProjectile projectilePrefab;
        private readonly Vector2 projectileOrigin;
        private readonly Vector2 attackDirection;
        private RangedProjectileEvent fireRangedProjectileEvent;

        public Vector2 ProjectileOrigin { get => projectileOrigin; }
        public Vector2 AttackDirection { get => attackDirection; }

        public RangedAttackState(
            Combat owner, RangedProjectile projectilePrefab,
            Vector2 projectileOrigin, Vector2 attackDirection,
            RangedProjectileEvent fireRangedProjectileEvent) : base(owner)
        {
            this.projectilePrefab = projectilePrefab;
            this.projectileOrigin = projectileOrigin;
            this.attackDirection = attackDirection;
            this.fireRangedProjectileEvent = fireRangedProjectileEvent;
        }

        public override void ExecuteAttackEffect()
        {
            RangedProjectile projectile = PhotonNetwork.Instantiate(
                projectilePrefab.name,
                projectileOrigin,
                RangedProjectile.GetRotationForDirection(attackDirection)).GetComponent<RangedProjectile>();

            projectile.Direction = attackDirection;
            fireRangedProjectileEvent.Invoke(projectile);
        }
    }
}
