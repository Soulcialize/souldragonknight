using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[System.Serializable]
public class RangedProjectileEvent : UnityEvent<RangedProjectile> { }

public class RangedProjectile : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private Collider2D collider2d;

    [Space(10)]

    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;

    [Space(10)]

    [SerializeField] private LayerMask obstaclesLayer;
    [SerializeField] private LayerMask friendliesLayer;

    [Space(10)]

    [SerializeField] private UnityEvent hitEvent;

    private Vector2 startPos;
    private Vector2 direction;

    public Vector2 Direction
    {
        get => direction;
        set
        {
            direction = value.normalized;
            transform.rotation = GetRotationForDirection(direction);
        }
    }

    public LayerMask ActorTargetsLayer { get; set; }
    public LayerMask FriendlyTargetsLayer { get => friendliesLayer; }

    public UnityEvent HitEvent { get => hitEvent; }

    public static Quaternion GetRotationForDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnDisable()
    {
        hitEvent.RemoveAllListeners();
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            rigidbody2d.velocity = Direction * speed;
            if (Vector2.Distance(startPos, transform.position) > maxDistance)
            {
                EndLifecycle();
            }
        }
    }

    public float GetHeight()
    {
        if (collider2d is CircleCollider2D circleCollider)
        {
            return circleCollider.radius * 2f;
        }
        else if (collider2d is BoxCollider2D boxCollider)
        {
            return boxCollider.size.y;
        }

        throw new System.ArgumentException($"Collider height calculation not implemented");
    }

    private void EndLifecycle()
    {
        photonView.RPC("RPC_EndProjectileLifecycle", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_EndProjectileLifecycle()
    {
        hitEvent.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (GeneralUtility.IsLayerInLayerMask(collision.gameObject.layer, ActorTargetsLayer))
        {
            ActorController actorHit = ActorController.GetActorFromCollider(collision);
            actorHit.Movement.UpdateMovement(Vector2.zero);

            if (actorHit.Combat.HasCombatAbility(CombatAbilityIdentifier.BLOCK)
                && ((BlockAbility)actorHit.Combat.GetCombatAbility(CombatAbilityIdentifier.BLOCK)).CanBlockProjectiles
                && actorHit.Combat.ActionStateMachine.CurrState is CombatStates.BlockState blockState)
            {
                // actor can block projectiles and is in block state, let block state handle hit
                blockState.HandleHit(actorHit.Movement.IsFacingRight, direction);
            }
            else
            {
                // actor not blocking, hurt actor
                actorHit.Combat.Hurt();
            }

            EndLifecycle();
        }
        else if (GeneralUtility.IsLayerInLayerMask(collision.gameObject.layer, friendliesLayer))
        {
            ActorController actorHit = ActorController.GetActorFromCollider(collision);

            // projectile hit friendly
            actorHit.Combat.Buff();
            EndLifecycle();
        }
        else if (GeneralUtility.IsLayerInLayerMask(collision.gameObject.layer, obstaclesLayer))
        {
            // projectile hit obstacle
            BreakableWall breakableWall = collision.GetComponent<BreakableWall>();
            if (breakableWall != null)
            {
                breakableWall.HandleHit();
            }

            EndLifecycle();
        } 
    }
}
