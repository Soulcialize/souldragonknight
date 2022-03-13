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

        public RangedAttackState(
            Combat owner, RangedProjectile projectilePrefab,
            Vector2 projectileOrigin, Vector2 attackDirection) : base(owner)
        {
            this.projectilePrefab = projectilePrefab;
            this.projectileOrigin = projectileOrigin;
            this.attackDirection = attackDirection;
        }

        public override void ExecuteAttackEffect()
        {
            RangedProjectile projectile = PhotonNetwork.Instantiate(
                projectilePrefab.name,
                projectileOrigin,
                RangedProjectile.GetRotationForDirection(attackDirection)).GetComponent<RangedProjectile>();

            projectile.Direction = attackDirection;
        }
    }
}
