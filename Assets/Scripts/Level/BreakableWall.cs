using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Collider2D collider2d;
    [SerializeField] private UnityEvent wallBrokenEvent;

    public void HandleHit()
    {
        AudioManagerSynced.Instance.PlaySoundFx(true, SoundFx.LibraryIndex.WALL_DESTROY);
        photonView.RPC("RPC_HandleHit", RpcTarget.All);
        wallBrokenEvent.Invoke();
    }

    [PunRPC]
    private void RPC_HandleHit()
    {
        // get collider bounds before disabling
        Vector2 updateRegionMinPoint = collider2d.bounds.min;
        Vector2 updateRegionMaxPoint = collider2d.bounds.max;

        collider2d.enabled = false;
        Pathfinding.NodeGridUpdater.Instance.RequestGridUpdate(updateRegionMinPoint, updateRegionMaxPoint);

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
