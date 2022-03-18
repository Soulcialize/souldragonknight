using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class ActorController : MonoBehaviour
{
    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected Combat combat;

    public abstract Movement Movement { get; }
    public Combat Combat { get => combat; }

    public static ActorController GetActorFromCollider(Collider2D collider)
    {
        return collider.GetComponentInParent<ActorController>();
    }

    protected virtual void Update()
    {
        if (photonView.IsMine)
        {
            Combat.CombatStateMachine.Update();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Movement.MovementStateMachine.Update();
        }
    }

    public void Interact(Interactable interactable)
    {
        interactable.Interact(this);
    }
}
