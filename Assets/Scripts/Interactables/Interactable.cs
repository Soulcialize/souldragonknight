using UnityEngine;
using Photon.Pun;

public abstract class Interactable : MonoBehaviour
{
    public enum Interaction
    {
        MOUNT, REVIVE
    }

    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected bool isEnabledByDefault;

    public bool IsEnabled { get; protected set; }
    public abstract Interaction InteractableInteraction { get; }

    protected virtual void Awake()
    {
        IsEnabled = isEnabledByDefault;
    }

    public abstract void Interact(ActorController initiator);

    public void SetIsEnabledWithoutSync(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    public void SetIsEnabledWithSync(bool isEnabled)
    {
        photonView.RPC("RPC_SetInteractableIsEnabled", RpcTarget.All, InteractableInteraction, isEnabled);
    }
}
