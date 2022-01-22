using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Movement")]

    [SerializeField] private float moveSpeed;
    [SerializeField] private float crouchSpeedModifier;
    [SerializeField] private bool isFacingRightOnAwake = true;

    [Header("Jump")]

    [SerializeField] private float jumpForce;
    [SerializeField] private float defaultGravityScale;

    [Header("Ground Detection")]

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SurfaceDetector groundDetectorLeft;
    [SerializeField] private SurfaceDetector groundDetectorMid;
    [SerializeField] private SurfaceDetector groundDetectorRight;

    [Header("Movement Events")]

    [SerializeField] private UnityEvent enterGroundedStateEvent;
    [SerializeField] private UnityEvent enterAirborneStateEvent;

    public Rigidbody2D Rigidbody { get => rb; }
    public Animator Animator { get => anim; }

    public float MoveSpeed { get => moveSpeed; }
    public float CrouchSpeedModifier { get => crouchSpeedModifier; }
    public bool IsFacingRight { get; private set; }
    public bool IsCrouched { get; private set; }

    public float JumpForce { get => jumpForce; }
    public float DefaultGravityScale { get => defaultGravityScale; }

    public LayerMask GroundLayer { get => groundLayer; }

    public UnityEvent EnterGroundedStateEvent { get => enterGroundedStateEvent; }
    public UnityEvent EnterAirborneStateEvent { get => enterAirborneStateEvent; }

    public MovementStateMachine MovementStateMachine { get; private set; }

    void Awake()
    {
        MovementStateMachine = new MovementStateMachine();

        // before startup, the actor is facing right by default
        IsFacingRight = true;
        if (!isFacingRightOnAwake)
        {
            Flip();
        }
    }

    void Start()
    {
        Rigidbody.gravityScale = defaultGravityScale;
        if (IsTouchingGround())
        {
            MovementStateMachine.ChangeState(new GroundedState(this));
        }
        else
        {
            MovementStateMachine.ChangeState(new AirborneState(this));
        }
    }

    void FixedUpdate()
    {
        MovementStateMachine.Update();
    }

    public bool IsTouchingGround()
    {
        return groundDetectorLeft.IsInContact || groundDetectorMid.IsInContact || groundDetectorRight.IsInContact;
    }

    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 localScale = anim.gameObject.transform.localScale;
        localScale.x *= -1f;
        anim.gameObject.transform.localScale = localScale;
    }

    public void Move(float direction)
    {
        ((GroundedState)MovementStateMachine.CurrState).PostMoveRequest(direction);
    }

    public void JumpGrounded()
    {
        ((GroundedState)MovementStateMachine.CurrState).PostJumpRequest();
    }

    public void KnockBack(float force, bool isForceFromRight)
    {
        MovementStateMachine.ChangeState(new KnockbackState(this, force, isForceFromRight));
    }
}
