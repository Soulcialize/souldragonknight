using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    [Space(10)]

    [SerializeField] private AttackEffectArea attackEffectArea;
    [SerializeField] private LayerMask attackEffectLayer;

    private bool isFacingRight;

    public bool IsAttacking { get; private set; }

    public void Attack(bool isFacingRight)
    {
        IsAttacking = true;
        this.isFacingRight = isFacingRight;
        animator.SetBool("isAttacking", IsAttacking);
    }

    public void ExecuteAttackEffect()
    {
        print(isFacingRight);
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            (isFacingRight ? attackEffectArea.RightOrigin : attackEffectArea.LeftOrigin).position,
            attackEffectArea.Size,
            attackEffectArea.LeftOrigin.eulerAngles.z,
            attackEffectLayer);

        print($"hits: {hits.Length}");
        foreach (Collider2D hit in hits)
        {
            ActorController actorHit = hit.GetComponent<ActorController>();
            if (actorHit != null)
            {
                actorHit.Combat.Hurt();
            }
        }
    }

    public void OnAttackEnd()
    {
        IsAttacking = false;
        animator.SetBool("isAttacking", IsAttacking);
    }

    public void Hurt()
    {
        Debug.Log($"{gameObject.name} hurt");
    }
}
