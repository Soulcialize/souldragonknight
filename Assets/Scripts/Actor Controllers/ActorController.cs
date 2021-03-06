using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class ActorController : MonoBehaviour
{
    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected Combat combat;
    [SerializeField] protected PathfindingUnit pathfinder;

    public abstract Movement Movement { get; }
    public Combat Combat { get => combat; }
    public PathfindingUnit Pathfinder { get => pathfinder; }

    public int NetworkViewId { get => photonView.ViewID; }
    public bool IsNetworkOwner { get => photonView.IsMine; }

    public static ActorController GetActorFromCollider(Collider2D collider)
    {
        return collider.GetComponentInParent<ActorController>();
    }

    protected virtual void OnEnable()
    {
        if (photonView.IsMine)
        {
            Combat.HurtEvent.AddListener(HandleHurtEvent);
            Combat.DeathEvent.AddListener(HandleDeathEvent);
            Combat.ReviveStartEvent.AddListener(HandleReviveStartEvent);
            Combat.ReviveFinishEvent.AddListener(HandleReviveFinishEvent);
        }
    }

    protected virtual void OnDisable()
    {
        if (photonView.IsMine)
        {
            Combat.HurtEvent.RemoveListener(HandleHurtEvent);
            Combat.DeathEvent.RemoveListener(HandleDeathEvent);
            Combat.ReviveStartEvent.RemoveListener(HandleReviveStartEvent);
            Combat.ReviveFinishEvent.RemoveListener(HandleReviveFinishEvent);
        }
    }

    protected virtual void Update()
    {
        if (photonView.IsMine)
        {
            Combat.ActionStateMachine.Update();
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
        if (!interactable.IsInteracting)
        {
            combat.ActionStateMachine.ChangeState(new CombatStates.InteractState(combat, this, interactable));
        }
    }

    public void InterruptInteraction()
    {
        if (combat.ActionStateMachine.CurrState is CombatStates.InteractState interactState)
        {
            interactState.InterruptInteraction();
        }
    }

    protected virtual void HandleHurtEvent() { }

    protected virtual void HandleDeathEvent() { }

    protected virtual void HandleReviveStartEvent() { }

    protected virtual void HandleReviveFinishEvent() { }
}
