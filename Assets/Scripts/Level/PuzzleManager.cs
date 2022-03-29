using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

using PlayerType = RoleSelectManager.PlayerType;

public class PuzzleManager : MonoBehaviour
{
    [Header("Runes")]

    [SerializeField] public Sprite[] runeSprites;
    [SerializeField] private SpriteRenderer[] answerRunes;
    [SerializeField] private RuneInteractable[] runeRocks;

    [Header("Events")]

    [SerializeField] private UnityEvent puzzleCompleteEvent;

    [Header("Photon")]

    [SerializeField] private PhotonView photonView;

    private int answerLength;
    private int[] runeAnswer;

    private void OnValidate()
    {
        if (answerRunes.Length > runeSprites.Length)
        {
            Debug.LogWarning("There are not enough unique sprites to support the answer length!");
        }
    }

    private void Start()
    {
        answerLength = answerRunes.Length;

        if (PhotonNetwork.IsMasterClient)
        {
            GenerateRandomAnswer();
            photonView.RPC("RPC_SetPuzzleAnswer", RpcTarget.All, runeAnswer);
        }

        HidePuzzleHalf();
    }

    private void OnEnable()
    {
        foreach (RuneInteractable runeRock in runeRocks)
        {
            runeRock.RuneUpdateEvent.AddListener(HandleRuneUpdate);
        }
    }

    private void OnDisable()
    {
        foreach (RuneInteractable runeRock in runeRocks)
        {
            runeRock.RuneUpdateEvent.RemoveListener(HandleRuneUpdate);
        }
    }

    private void GenerateRandomAnswer()
    {
        runeAnswer = new int[answerLength];
        List<int> values = new List<int>();

        for (int i = 0; i < runeSprites.Length; i++)
        {
            values.Add(i);
        }

        for (int i = 0; i < answerLength; i++)
        {
            int randomIndex = Random.Range(0, values.Count);
            runeAnswer[i] = values[randomIndex];

            values.RemoveAt(randomIndex);
        }
    }

    private void HidePuzzleHalf()
    {
        PlayerType playerType = PlayerSpawner.GetLocalPlayerType();

        if (playerType == PlayerType.KNIGHT)
        {
            HideAnswerRunes();
        }
        else if (playerType == PlayerType.DRAGON)
        {
            HideRockRunes();
        }
        else
        {
            throw new System.ArgumentException("Unknown player type");
        }
    }

    private void HideAnswerRunes()
    {
        foreach (SpriteRenderer sprite in answerRunes)
        {
            sprite.gameObject.SetActive(false);
        }
    }

    private void HideRockRunes()
    {
        foreach (RuneInteractable runeRock in runeRocks)
        {
            runeRock.HideRune();
        }
    }

    private void HandleRuneUpdate()
    {
        for (int i = 0; i < answerLength; i++)
        {
            if (runeRocks[i].CurrentRuneIndex != runeAnswer[i])
            {
                return;
            }
        }
        
        DisablePuzzleInput();
        photonView.RPC("RPC_SyncPuzzleCompleted", RpcTarget.All);
    }

    private void DisablePuzzleInput()
    {
        foreach (RuneInteractable runeRock in runeRocks)
        {
            runeRock.SetIsEnabledWithoutSync(false);
        }
    }

    [PunRPC]
    private void RPC_SetPuzzleAnswer(int[] answer)
    {
        runeAnswer = answer;

        for (int i = 0; i < answerLength; i++)
        {
            answerRunes[i].sprite = runeSprites[answer[i]];
        }
    }

    [PunRPC]
    private void RPC_SyncPuzzleCompleted() 
    {
        Debug.Log("The puzzle was solved");
        puzzleCompleteEvent.Invoke();
    }
}
