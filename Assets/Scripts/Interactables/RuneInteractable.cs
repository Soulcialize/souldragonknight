using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RuneInteractable : Interactable
{
    [SerializeField] private SpriteRenderer currentRune;
    [SerializeField] private PuzzleManager puzzleManager;

    private int currentRuneIndex;
    private int runeCount;

    private void Start()
    {
        runeCount = puzzleManager.runeSprites.Length;
        currentRuneIndex = Random.Range(0, runeCount);
        currentRune.sprite = puzzleManager.runeSprites[currentRuneIndex];
    }

    public override Interaction InteractableInteraction { get => Interaction.SWITCH; }

    public override Interactor InteractableInteractor { get => Interactor.KNIGHT; }

    public override void Interact(ActorController initiator, UnityAction endInteractionCallback)
    {
        currentRuneIndex = (currentRuneIndex + 1) % runeCount;
        currentRune.sprite = puzzleManager.runeSprites[currentRuneIndex];
    }

    public void HideRune()
    {
        currentRune.gameObject.SetActive(false);
    }
}
