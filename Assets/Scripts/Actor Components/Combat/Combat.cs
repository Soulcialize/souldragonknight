using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    public bool IsAttacking { get; private set; }

    public void Attack()
    {
        IsAttacking = true;
        animator.SetBool("isAttacking", IsAttacking);
    }

    public void OnAttackEnd()
    {
        IsAttacking = false;
        animator.SetBool("isAttacking", IsAttacking);
    }
}
