using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractablesManager : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;

    private Dictionary<Interactable.Interaction, Interactable> interactables
        = new Dictionary<Interactable.Interaction, Interactable>();

    private void Awake()
    {
        Interactable[] interactablesList = GetComponents<Interactable>();
        foreach (Interactable interactable in interactablesList)
        {
            interactables[interactable.InteractableInteraction] = interactable;
        }
    }

    [PunRPC]
    private void RPC_SetInteractableIsEnabled(Interactable.Interaction interactable, bool isEnabled)
    {
        interactables[interactable].SetIsEnabledWithoutSync(isEnabled);
    }
}
