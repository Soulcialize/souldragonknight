using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PuzzleManager : MonoBehaviour
{
    [Header("Runes")]

    [SerializeField] public Sprite[] runeSprites;
    [SerializeField] private SpriteRenderer[] answerRunes;

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

    [PunRPC]
    private void RPC_SetPuzzleAnswer(int[] answer)
    {
        runeAnswer = answer;

        for (int i = 0; i < answerLength; i++)
        {
            answerRunes[i].sprite = runeSprites[answer[i]];
        }
    }
}
