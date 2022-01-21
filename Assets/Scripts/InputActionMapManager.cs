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

    public bool IsInputActive { get => input.enabled; }

    private void Awake()
    {
        foreach (string actionMap in persistentActionMaps)
        {
            input.actions.FindActionMap(actionMap).Enable();
        }

        currentActionMap = input.actions.FindActionMap(input.defaultActionMap);
    }

    public void EnableInput()
    {
        input.enabled = true;
    }

    public void DisableInput()
    {
        input.enabled = false;
    }

    public void SwitchInputActionMapTo(string actionMap)
    {
        if (!IsInputActive || currentActionMap.name == actionMap || persistentActionMaps.Contains(actionMap))
        {
            return;
        }

        currentActionMap.Disable();
        currentActionMap = input.actions.FindActionMap(actionMap);
        currentActionMap.Enable();
    }
}
