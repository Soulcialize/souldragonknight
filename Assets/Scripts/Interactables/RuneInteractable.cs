using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RuneInteractable : Interactable
{
    [SerializeField] private SpriteRenderer currentRune;
    [SerializeField] private Sprite[] runes;

    private int currentRuneIndex;
    private int runeCount;

    public int CurrentRuneIndex { get => currentRuneIndex; }

    private void Start()
    {
        runeCount = runes.Length;
        currentRuneIndex = Random.Range(0, runeCount - 1);
        currentRune.sprite = runes[currentRuneIndex];
    }

    public override Interaction InteractableInteraction { get => Interaction.SWITCH; }

    public override Interactor InteractableInteractor { get => Interactor.KNIGHT; }

    public override void Interact(ActorController initiator, UnityAction endInteractionCallback)
    {
        currentRuneIndex = (currentRuneIndex + 1) % runeCount;
        currentRune.sprite = runes[currentRuneIndex];
    }

    public void HideRune()
    {
        currentRune.gameObject.SetActive(false);
    }
}
