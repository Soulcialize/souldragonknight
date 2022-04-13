using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryCanvas;

    public void DeclareVictory()
    {
        StartCoroutine(InitiateVictory());
    }

    private IEnumerator InitiateVictory()
    {
        yield return new WaitForSeconds(5f);

        victoryCanvas.SetActive(true);
        LevelAudioManager.Instance.PlayLevelVictoryMusic(false);
    }
}
