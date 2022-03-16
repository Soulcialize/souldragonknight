using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using GroundMovementStates;
using Photon.Pun;

public class GroundMovement : Movement
{
    [Header("Ground Movement")]

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Mount")]

    [SerializeField] private AnimatorOverrideController mountAnimatorOverride;

    private RuntimeAnimatorController defaultAnimatorController;

    private GroundMovementStateMachine movementStateMachine;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }
    public float HorizontalMoveSpeed { get => horizontalMoveSpeed; }
    public float JumpForce { get => jumpForce; }

    public override MovementStateMachine MovementStateMachine { get => movementStateMachine; }

    protected override void Awake()
    {
        base.Awake();
        movementStateMachine = new GroundMovementStateMachine();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
        if (groundDetector.IsInContact)
        {
            MovementStateMachine.ChangeState(new GroundedState(this));
        }
        else
        {
            MovementStateMachine.ChangeState(new AirborneState(this));
        }
    }

    public override void UpdateMovement(Vector2 direction)
    {
        CachedMovementDirection = direction;
        if (MovementStateMachine.CurrState is GroundedState groundedState)
        {
            groundedState.UpdateHorizontalMovement(direction.x);
        }
    }

    public void Jump()
    {
        if (MovementStateMachine.CurrState is GroundedState groundedState)
        {
            groundedState.PostJumpRequest();
        }
    }

    public void Mount(Transform mount, Vector2 localOffset, string newSortingLayer, int newSortingLayerOrder)
    {
        photonView.RPC("RPC_Mount", RpcTarget.All,
            mount.GetComponent<PhotonView>().ViewID, localOffset.x, localOffset.y, newSortingLayer, newSortingLayerOrder);
    }

    public void Dismount(string updatedSortingLayer, int updatedSortingLayerOrder)
    {
        photonView.RPC("RPC_Dismount", RpcTarget.All, updatedSortingLayer, updatedSortingLayerOrder);
    }

    [PunRPC]
    private void RPC_Mount(int mountViewId, float localOffsetX, float localOffsetY, string updatedSortingLayer, int updatedSortingLayerOrder)
    {
        rigidbody2d.isKinematic = true;
        
        transform.parent = PhotonView.Find(mountViewId).transform;
        transform.localPosition = new Vector2(localOffsetX, localOffsetY);

        spriteRenderer.sortingLayerName = updatedSortingLayer;
        spriteRenderer.sortingOrder = updatedSortingLayerOrder;

        defaultAnimatorController = animator.runtimeAnimatorController;
        animator.runtimeAnimatorController = mountAnimatorOverride;
    }

    [PunRPC]
    private void RPC_Dismount(string updatedSortingLayer, int updatedSortingLayerOrder)
    {
        transform.parent = null;
        
        rigidbody2d.isKinematic = false;

        spriteRenderer.sortingLayerName = updatedSortingLayer;
        spriteRenderer.sortingOrder = updatedSortingLayerOrder;

        animator.runtimeAnimatorController = defaultAnimatorController;
    }
}
