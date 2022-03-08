using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RangedProjectile : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask actorTargetsLayer;
    [SerializeField] private LayerMask obstaclesLayer;

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

    public static Quaternion GetRotationForDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
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

    private void EndLifecycle()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GeneralUtility.IsLayerInLayerMask(collision.gameObject.layer, actorTargetsLayer))
        {
            if (photonView.IsMine)
            {
                ActorController actorHit = ActorController.GetActorFromCollider(collision);
                actorHit.Movement.UpdateMovement(Vector2.zero);
                actorHit.Combat.Hurt();
            }

            EndLifecycle();
        }
        else if (GeneralUtility.IsLayerInLayerMask(collision.gameObject.layer, obstaclesLayer))
        {
            EndLifecycle();
        }
    }
}
