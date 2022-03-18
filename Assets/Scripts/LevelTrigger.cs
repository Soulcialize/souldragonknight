using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent playerEnteredEvent;

    private int numEntries = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (numEntries == 0)
        {
            ActorController player = ActorController.GetActorFromCollider(collision);
            if (player != null && player.IsNetworkOwner)
            {
                numEntries++;
                playerEnteredEvent.Invoke();
            }
        }
    }
}
