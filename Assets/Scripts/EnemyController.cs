using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ActorController
{
    [SerializeField] private float speed;
    
    private LayerMask playerLayerMask;

    protected override void Start()
    {
        base.Start();

        GetComponentInParent<Rigidbody2D>().velocity = new Vector2(-1f * speed, 0f);
    }

    private void Update()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, ((CircleCollider2D)collider2d).radius, playerLayerMask);
        if (collider != null)
        {
            ActorController playerHit = GetActorControllerFromActorCollider(collider);
            if (!(playerHit.Combat.CombatStateMachine.CurrState is DeadState))
            {
                playerHit.Die();
            }
        }
    }

    public void SetPlayerLayerMask(LayerMask layerMask)
    {
        playerLayerMask = layerMask;
    }
}
