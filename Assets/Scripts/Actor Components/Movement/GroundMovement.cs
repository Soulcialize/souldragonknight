using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using GroundMovementStates;
using Photon.Pun;

public class GroundMovement : Movement
{
    [Header("Ground Movement")]

    [SerializeField] private SpriteLayer spriteLayer;
    [SerializeField] private float jumpForce;
    [Tooltip("The horizontal velocity an actor can move while airborne (on top of whatever horizontal velocity they started with before being airborne).")]
    [SerializeField] private float airborneHorizontalMoveSpeed;
    [Tooltip("The maximum height a point can be at before it is considered unreachable by the grounded actor.")]
    [SerializeField] private float maxReachableHeight;

    [Header("Mount")]

    [SerializeField] private AnimatorOverrideController mountAnimatorOverride;

    private RuntimeAnimatorController defaultAnimatorController;
    private GroundMovementStateMachine movementStateMachine;

    public float JumpForce { get => jumpForce; }
    public float AirborneHorizontalMoveSpeed { get => airborneHorizontalMoveSpeed; }
    public float MaxReachableHeight { get => maxReachableHeight; }

    public override MovementStateMachine MovementStateMachine { get => movementStateMachine; }

    protected override void Awake()
    {
        base.Awake();
        movementStateMachine = new GroundMovementStateMachine();
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
        else if (MovementStateMachine.CurrState is AirborneState airborneState)
        {
            airborneState.UpdateHorizontalMovement(direction.x);
        }
    }

    public override void SetMovementMode(MovementSpeedData.Mode mode)
    {
        if (MovementStateMachine.CurrState is GroundedState groundedState)
        {
            MovementMode = mode;
            groundedState.UpdateMovementMode(mode);
        }
    }

    public void Jump()
    {
        if (MovementStateMachine.CurrState is GroundedState groundedState)
        {
            groundedState.PostJumpRequest();
        }
    }

    public void Mount(Transform mount, Movement mountMovement, Vector2 localOffset,
        SpriteLayer.Layer mountedSortingLayer, int mountedSortingLayerOrder)
    {
        if (MovementStateMachine.CurrState is GroundedState || MovementStateMachine.CurrState is AirborneState)
        {
            MovementStateMachine.ChangeState(new MountedState(this, mountMovement));
            photonView.RPC("RPC_MountRider", RpcTarget.All,
                mount.GetComponent<PhotonView>().ViewID, localOffset.x, localOffset.y, mountedSortingLayer, mountedSortingLayerOrder);
        }
    }

    public void Dismount()
    {
        if (MovementStateMachine.CurrState is MountedState)
        {
            MovementStateMachine.ChangeState(new AirborneState(this));
            photonView.RPC("RPC_DismountRider", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_MountRider(int mountViewId, float localOffsetX, float localOffsetY,
        SpriteLayer.Layer mountedSortingLayer, int mountedSortingLayerOrder)
    {
        rigidbody2d.isKinematic = true;
        
        transform.parent = PhotonView.Find(mountViewId).transform;
        transform.localPosition = new Vector2(localOffsetX, localOffsetY);

        spriteLayer.SetLayer(mountedSortingLayer, mountedSortingLayerOrder);

        defaultAnimatorController = animator.runtimeAnimatorController;
        GeneralUtility.SwapAnimatorController(animator, mountAnimatorOverride, false);
    }

    [PunRPC]
    private void RPC_DismountRider()
    {
        transform.parent = null;
        rigidbody2d.isKinematic = false;
        spriteLayer.ResetLayer();
        GeneralUtility.SwapAnimatorController(animator, defaultAnimatorController, true);
    }
}
