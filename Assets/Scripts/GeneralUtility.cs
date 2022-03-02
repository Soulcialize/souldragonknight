using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralUtility
{
    public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
