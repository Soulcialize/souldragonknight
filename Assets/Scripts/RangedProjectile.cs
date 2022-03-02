using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RangedProjectile : MonoBehaviour
{
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
            RotateToDirection();
        }
    }

    /// <summary>
    /// Enables the projectile.
    /// </summary>
    /// <remarks>
    /// The projectile object starts disabled.
    /// It should only be enabled after its <c>Direction</c> has been set post-instantiation.
    /// </remarks>
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        rigidbody2d.velocity = Direction * speed;
        if (Vector2.Distance(startPos, transform.position) > maxDistance)
        {
            EndLifecycle();
        }
    }

    private void RotateToDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void EndLifecycle()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GeneralUtility.IsLayerInLayerMask(collision.gameObject.layer, actorTargetsLayer))
        {
            ActorController actorHit = collision.gameObject.GetComponent<ActorController>();
            actorHit.Movement.UpdateMovement(Vector2.zero);
            actorHit.Combat.Hurt();
            EndLifecycle();
        }
        else if (GeneralUtility.IsLayerInLayerMask(collision.gameObject.layer, obstaclesLayer))
        {
            EndLifecycle();
        }
    }
}
