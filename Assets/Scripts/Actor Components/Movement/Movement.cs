using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StateMachines;
using Photon.Pun;

public abstract class Movement : MonoBehaviour
{
    public enum Direction { LEFT, RIGHT }

    [SerializeField] protected SoundFXIndexLibrary soundFXIndexLibrary;
    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SurfaceDetector groundDetector;
    [SerializeField] private Transform actorTransform; // mainly used for flipping

    [Space(10)]

    [SerializeField] protected MovementSpeedData movementSpeedData;
    [SerializeField] protected MovementSpeedData.Mode defaultMovementMode;

    [Space(10)]

    [Tooltip("Whether the actor's sprite faces right or left by default.")]
    [SerializeField] private bool isDefaultFacingRight = true;
    [SerializeField] private float defaultStoppingDistanceFromNavTargets;

    /*
     * By having a gap between the two thresholds, we avoid a situation where the actor could
     * flip between walking and running every frame, which can happen if there were a single
     * threshold and their navigation target constantly goes in and out of that threshold.
     */
    [Tooltip("Distance to a navigation target within which the actor will start moving slower.")]
    [SerializeField] private float navSlowDistanceThreshold;
    [Tooltip("Distance to a navigation target beyond which the actor will start moving faster.")]
    [SerializeField] private float navFastDistanceThreshold;

    [Space(10)]

    [SerializeField] private UnityEvent flipDirectionEvent;

    private Dictionary<MovementSpeedData.Mode, float> movementModeToSpeedDictionary;

    public SoundFXIndexLibrary SoundFXIndexLibrary { get => soundFXIndexLibrary; }
    public Rigidbody2D Rigidbody2d { get => rigidbody2d; }
    public Animator Animator { get => animator; }
    public SurfaceDetector GroundDetector { get => groundDetector; }

    public bool IsDefaultFacingRight { get => isDefaultFacingRight; }
    public float DefaultStoppingDistanceFromNavTargets { get => defaultStoppingDistanceFromNavTargets; }
    public float NavTargetSlowDistanceThreshold { get => navSlowDistanceThreshold; }
    public float NavTargetFastDistanceThreshold { get => navFastDistanceThreshold; }

    public int NetworkViewId { get => photonView.ViewID; }
    public bool IsFacingRight { get => actorTransform.localScale.x > 0f; }
    public Vector2 CachedMovementDirection { get; protected set; }

    public MovementSpeedData.Mode MovementMode { get; protected set; }
    public float MovementSpeed { get => movementModeToSpeedDictionary[MovementMode]; }

    public abstract MovementStateMachine MovementStateMachine { get; }

    protected virtual void Awake()
    {
        MovementMode = defaultMovementMode;
        movementModeToSpeedDictionary = movementSpeedData.GetModeToSpeedDictionary();
    }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }

    protected virtual void Start()
    {
        FlipDirection(isDefaultFacingRight ? Direction.RIGHT : Direction.LEFT);
    }

    public abstract void UpdateMovement(Vector2 direction);

    public abstract void SetMovementMode(MovementSpeedData.Mode mode);

    public float GetMovementSpeedForMode(MovementSpeedData.Mode mode)
    {
        return movementModeToSpeedDictionary[mode];
    }

    public void FlipDirection(float toDirection)
    {
        Vector3 localScale = actorTransform.localScale;
        if (toDirection < 0f && localScale.x > 0f || toDirection > 0f && localScale.x < 0f)
        {
            photonView.RPC("RPC_FlipDirection", RpcTarget.All);
        }
    }

    public void FlipDirection(Direction toDirection)
    {
        switch (toDirection)
        {
            case Direction.LEFT:
                FlipDirection(-1f);
                break;
            case Direction.RIGHT:
                FlipDirection(1f);
                break;
            default:
                throw new System.ArgumentException("Direction " +
                    $"{System.Enum.GetName(typeof(Direction), toDirection)} " +
                    "is not a valid direction to flip towards");
        }
    }

    // Needs to be flagged as [PunRPC] in concrete child class.
    protected virtual void RPC_FlipDirection()
    {
        Vector3 localScale = actorTransform.localScale;
        localScale.x = -localScale.x;
        actorTransform.localScale = localScale;
        flipDirectionEvent.Invoke();
    }
}
