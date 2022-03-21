using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Environmental Object Data", menuName = "Scriptable Objects/Environmental Object Data")]
public class EnvironmentalObjectData : ScriptableObject
{
    [SerializeField] private Sprite knightPovSprite;
    [SerializeField] private Sprite dragonPovSprite;

    public Sprite KnightPovSprite { get => knightPovSprite; }
    public Sprite DragonPovSprite { get => dragonPovSprite; }
}
