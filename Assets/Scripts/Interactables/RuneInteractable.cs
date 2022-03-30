using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RuneInteractable : Interactable
{
    [Header("Puzzle")]

    [SerializeField] private SpriteRenderer currentRune;
    [SerializeField] private PuzzleManager puzzleManager;

    [Header("Puzzle Events")]

    [SerializeField] private UnityEvent runeUpdateEvent;

    private int currentRuneIndex;
    private int runeCount;

    public UnityEvent RuneUpdateEvent { get => runeUpdateEvent; }
    public int CurrentRuneIndex { get => currentRuneIndex; }
    public override Interaction InteractableInteraction { get => Interaction.SWITCH; }

    private void Start()
    {
        runeCount = puzzleManager.runeSprites.Length;
        currentRuneIndex = Random.Range(0, runeCount);
        currentRune.sprite = puzzleManager.runeSprites[currentRuneIndex];
    }

    public override void Interact(ActorController initiator, UnityAction endInteractionCallback)
    {
        currentRuneIndex = (currentRuneIndex + 1) % runeCount;
        currentRune.sprite = puzzleManager.runeSprites[currentRuneIndex];
        RuneUpdateEvent.Invoke();
        endInteractionCallback();
    }

    public void HideRune()
    {
        currentRune.gameObject.SetActive(false);
    }
}
