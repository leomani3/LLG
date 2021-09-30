using System;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.Spawning;
using MLAPI.NetworkVariable.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Button _startGameButton;
    //
    private NetworkList<LobbyPlayerState> _lobbyPlayers = new NetworkList<LobbyPlayerState>();

    public override void NetworkStart()
    {
        if (IsClient)
        {
            _lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
        }
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

            foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }
    }

    private void OnDestroy()
    {
        _lobbyPlayers.OnListChanged -= HandleLobbyPlayersStateChanged;

        if (NetworkManager.Singleton)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);

        if (!playerData.HasValue) { return; }

        _lobbyPlayers.Add(new LobbyPlayerState(
            clientId,
            playerData.Value.PlayerName,
            false,
            playerData.Value.NumberColor
        ));
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        for (int i = 0; i < _lobbyPlayers.Count; i++)
        {
            if (_lobbyPlayers[i].ClientId == clientId)
            {
                _lobbyPlayers.RemoveAt(i);
                break;
            }
        }
    }

    private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState)
    {
        for (int i = 0; i < _lobbyPlayers.Count; i++)
        {
            PlayerController player = NetworkSpawnManager.GetPlayerNetworkObject(_lobbyPlayers[i].ClientId).gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetLobbyPlayerState(_lobbyPlayers[i]);
            }
            else
            {
                Debug.LogError("player empty");
            }
        }
    }
}
