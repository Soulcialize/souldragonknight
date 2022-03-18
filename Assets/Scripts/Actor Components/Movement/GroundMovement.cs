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
    [Tooltip("The maximum height a point can be at before it is considered unreachable by the grounded actor.")]
    [SerializeField] private float maxReachableHeight;

    [Header("Mount")]

    [SerializeField] private AnimatorOverrideController mountAnimatorOverride;

    private RuntimeAnimatorController defaultAnimatorController;

    private GroundMovementStateMachine movementStateMachine;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }
    public float HorizontalMoveSpeed { get => horizontalMoveSpeed; }
    public float JumpForce { get => jumpForce; }
    public float MaxReachableHeight { get => maxReachableHeight; }

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

    public void Mount(Transform mount, Movement mountMovement, Vector2 localOffset, string mountedSortingLayer, int mountedSortingLayerOrder)
    {
        if (MovementStateMachine.CurrState is GroundedState || MovementStateMachine.CurrState is AirborneState)
        {
            MovementStateMachine.ChangeState(new MountedState(this, mountMovement));
            photonView.RPC("RPC_Mount", RpcTarget.All,
                mount.GetComponent<PhotonView>().ViewID, localOffset.x, localOffset.y, mountedSortingLayer, mountedSortingLayerOrder);
        }
    }

    public void Dismount()
    {
        if (MovementStateMachine.CurrState is MountedState mountedState)
        {
            MovementStateMachine.ChangeState(new AirborneState(this));
            photonView.RPC("RPC_Dismount", RpcTarget.All,
                mountedState.OriginalSortingLayerName, mountedState.OriginalSortingLayerOrder);
        }
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
        GeneralUtility.SwapAnimatorController(animator, mountAnimatorOverride, false);
    }

    [PunRPC]
    private void RPC_Dismount(string updatedSortingLayer, int updatedSortingLayerOrder)
    {
        transform.parent = null;
        
        rigidbody2d.isKinematic = false;

        spriteRenderer.sortingLayerName = updatedSortingLayer;
        spriteRenderer.sortingOrder = updatedSortingLayerOrder;

        GeneralUtility.SwapAnimatorController(animator, defaultAnimatorController, true);
    }
}
