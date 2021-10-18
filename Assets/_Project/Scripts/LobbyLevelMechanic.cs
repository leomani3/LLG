using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyLevelMechanic : MonoBehaviour, ILevelMechanic
{
    private LobbyUI _lobbyUI;

    public void Interact()
    {
        _lobbyUI.OnReadySelected();
    }

    void Awake()
    {
        _lobbyUI = FindObjectOfType<LobbyUI>();

        if (_lobbyUI == null)
        {
            Debug.LogError("C'est cheum ça, LobbyLevelMechanic ne trouve pas LobbyUI");
        }
    }
}
