using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionMapManager : MonoBehaviour
{
    [SerializeField] private PlayerInput input;

    [Tooltip("Action maps that should remain enabled.")]
    [SerializeField] private List<string> persistentActionMaps;

    private InputActionMap currentActionMap;

    void Awake()
    {
        foreach (string actionMap in persistentActionMaps)
        {
            input.actions.FindActionMap(actionMap).Enable();
        }

        currentActionMap = input.actions.FindActionMap(input.defaultActionMap);
    }

    public void EnableInput()
    {
        input.ActivateInput();
    }

    public void DisableInput()
    {
        input.DeactivateInput();
    }

    public void SwitchInputActionMapTo(string actionMap)
    {
        if (currentActionMap.name == actionMap || persistentActionMaps.Contains(actionMap))
        {
            return;
        }

        currentActionMap.Disable();
        currentActionMap = input.actions.FindActionMap(actionMap);
        currentActionMap.Enable();
    }
}
