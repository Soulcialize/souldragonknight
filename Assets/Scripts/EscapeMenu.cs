using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] protected GameObject escapeMenuUi;
    [SerializeField] protected PlayerInput playerInput;
    [SerializeField] private string roomSceneName;
    [SerializeField] private string mainMenuSceneName;

    private InputAction menuAction;
    public static bool isMenuOpen = false;

    protected virtual void Awake()
    {
        menuAction = playerInput.actions["Menu"];
    }

    protected virtual void OnEnable()
    {
        if (playerInput.inputIsActive)
        {
            BindInputActionHandlers();
        }
    }

    protected virtual void OnDisable()
    {
        if (playerInput.inputIsActive)
        {
            UnbindInputActionHandlers();
        }
    }

    protected void BindInputActionHandlers()
    {
        menuAction.performed += HandleMenuInput;
    }

    protected void UnbindInputActionHandlers()
    {
        menuAction.performed += HandleMenuInput;
    }

    private void HandleMenuInput(InputAction.CallbackContext context)
    {
        if (isMenuOpen)
        {
            escapeMenuUi.SetActive(false);
        } else
        {
            escapeMenuUi.SetActive(true);
        }
        isMenuOpen = !isMenuOpen;
    }

    public void RestartLevel()
    {
        /*if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(roomSceneName);
            isMenuOpen = !isMenuOpen;
        }*/

        Debug.Log("Restarting level...");
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Exiting room...");
    }
}
