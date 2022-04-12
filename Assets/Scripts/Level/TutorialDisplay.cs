using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using PlayerType = RoleSelectManager.PlayerType;

public class TutorialDisplay : MonoBehaviour
{
    [SerializeField] private List<GameObject> knightTutorials;
    [SerializeField] private List<GameObject> dragonTutorials;

    private Dictionary<PlayerType, List<GameObject>> playerTypeToTutorialsDictionary;

    private void Awake()
    {
        playerTypeToTutorialsDictionary = new Dictionary<PlayerType, List<GameObject>>()
        {
            { PlayerType.KNIGHT, knightTutorials },
            { PlayerType.DRAGON, dragonTutorials }
        };

        PlayerType localPlayerType = PlayerSpawner.GetLocalPlayerType();
        foreach (var typeToTutorials in playerTypeToTutorialsDictionary)
        {
            bool isActive = localPlayerType == typeToTutorials.Key;

            foreach (GameObject tutorialObj in typeToTutorials.Value)
            {
                tutorialObj.SetActive(isActive);
            }
        }
    }
}
