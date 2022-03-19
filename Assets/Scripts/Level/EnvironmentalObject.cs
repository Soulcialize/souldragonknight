using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private EnvironmentalObjectData spritesData;

    private void Awake()
    {
        spriteRenderer.sprite = PlayerSpawner.GetLocalPlayerType() == RoleSelectManager.PlayerType.KNIGHT
            ? spritesData.KnightPovSprite
            : spritesData.DragonPovSprite;
    }
}
