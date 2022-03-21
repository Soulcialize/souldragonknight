using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpriteLayer : MonoBehaviour
{
    public enum Layer
    {
        DEFAULT, ENVIRONMENT, DEAD, DRAGON, DRAGON_MOUNT, KNIGHT
    }

    private static readonly Dictionary<Layer, string> layerToNameDictionary = new Dictionary<Layer, string>()
    {
        { Layer.DEFAULT, "Default" },
        { Layer.ENVIRONMENT, "Environment" },
        { Layer.DEAD, "Dead" },
        { Layer.DRAGON, "Dragon" },
        { Layer.DRAGON_MOUNT, "Dragon Mount" },
        { Layer.KNIGHT, "Knight" }
    };

    [SerializeField] private PhotonView photonView;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private string originalLayerName;
    private int originalOrder;

    private void Awake()
    {
        originalLayerName = spriteRenderer.sortingLayerName;
        originalOrder = spriteRenderer.sortingOrder;
    }

    public void SetLayer(Layer layer, int order = 0)
    {
        photonView.RPC("RPC_SetSpriteRendererLayer", RpcTarget.All, layerToNameDictionary[layer], order);
    }

    public void ResetLayer()
    {
        photonView.RPC("RPC_SetSpriteRendererLayer", RpcTarget.All, originalLayerName, originalOrder);
    }

    [PunRPC]
    private void RPC_SetSpriteRendererLayer(string layerName, int order)
    {
        spriteRenderer.sortingLayerName = layerName;
        spriteRenderer.sortingOrder = order;
    }
}
