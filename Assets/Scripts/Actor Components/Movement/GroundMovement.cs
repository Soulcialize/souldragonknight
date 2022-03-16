using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;
using GroundMovementStates;
using Photon.Pun;

public class GroundMovement : Movement
{
    [Header("Ground Movement")]

    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float jumpForce;

    private GroundMovementStateMachine movementStateMachine;

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

    public void Mount(Transform mount)
    {
        photonView.RPC("RPC_Mount", RpcTarget.All, mount.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    private void RPC_Mount(int mountViewId)
    {
        rigidbody2d.simulated = false;
        transform.parent = PhotonView.Find(mountViewId).transform;
    }
}
