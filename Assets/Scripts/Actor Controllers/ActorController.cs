using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class ActorController : MonoBehaviour
{
    [SerializeField] protected PhotonView photonView;

    public abstract Movement Movement { get; }
    public abstract Combat Combat { get; }

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
}
