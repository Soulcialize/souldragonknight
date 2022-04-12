using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using PlayerType = RoleSelectManager.PlayerType;

[System.Serializable]
public class RoleSelectEvent : UnityEvent<PlayerType> { }

public class RoleSelectButton : MonoBehaviour
{
    [SerializeField] private PlayerType playerType;

    [Space(10)]

    [SerializeField] private RoleSelectEvent roleSelectEvent;

    public void HandleRoleButtonClick()
    {
        AudioManagerSynced.Instance.PlaySoundFx(true, SoundFx.LibraryIndex.ROLE_LEVEL_BUTTON);
        roleSelectEvent.Invoke(playerType);
    }
}
