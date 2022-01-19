using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    [SerializeField] protected Movement movement;
    [SerializeField] protected Combat combat;
    [SerializeField] protected Collider2D collider2d;

    public Movement Movement { get => movement; }
    public Combat Combat { get => combat; }
    public Collider2D Collider2D { get => collider2d; }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    public static string GetActorControllerObjectName(ActorController actor)
    {
        return actor.transform.parent.name;
    }

    public static ActorController GetActorControllerFromActorCollider(Collider2D collider)
    {
        if (collider.transform.parent is null || collider.transform.parent.parent is null)
        {
            return null;
        }

        return collider.transform.parent.parent.GetComponentInChildren<ActorController>();
    }

    public void Hurt()
    {
        if (movement.MovementStateMachine.CurrState is AirborneState)
        {
            // blink
        }
        else
        {
            // grounded
            ((GroundedState)movement.MovementStateMachine.CurrState)?.PostMoveRequest(0f);
            combat.Hurt();
        }
    }

    public void Die()
    {
        if (movement.MovementStateMachine.CurrState is AirborneState)
        {
            // blink, die upon reaching the ground
        }
        else
        {
            // grounded
            ((GroundedState)movement.MovementStateMachine.CurrState)?.PostMoveRequest(0f);
            combat.Die();
        }
    }
}
