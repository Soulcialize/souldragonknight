using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] protected Animator animator;
    [SerializeField] private bool isDefaultFacingRight = true;

    public Rigidbody2D Rigidbody2d { get => rigidbody2d; }
    public Animator Animator { get => animator; }

    public bool IsFacingRight { get; private set; }

    protected virtual void Awake() { }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }

    protected virtual void Start()
    {
        FlipDirection(isDefaultFacingRight ? 1f : -1f);
        IsFacingRight = isDefaultFacingRight;
    }

    protected virtual void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            UpdateMovement();
        }
    }

    protected abstract void UpdateMovement();

    public void FlipDirection(float toDirection)
    {
        Vector3 localScale = transform.localScale;
        if (toDirection < 0f && localScale.x > 0f || toDirection > 0f && localScale.x < 0f)
        {
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        }

        IsFacingRight = localScale.x > 0f;
    }
}
