using System;
using System.Collections.Generic;
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
    [SerializeField] private Button startGameButton;
    [SerializeField] private GameObject playerPrefab;
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

        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not assigned");
            return;
        }

        SpawnPlayerObject(clientId);

        LobbyPlayerState lps = new LobbyPlayerState(
            clientId,
            playerData.Value.PlayerName,
            false,
            playerData.Value.NumberColor
        );

        _lobbyPlayers.Add(lps);
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
        PlayerController[] lpc = FindObjectsOfType<PlayerController>();

        for (int i = 0; i < _lobbyPlayers.Count; i++)
        {
            LobbyPlayerState lps = _lobbyPlayers[i];

            foreach (PlayerController pc in lpc)
            {
                NetworkObject no = pc.GetComponent<NetworkObject>();
                if (no.OwnerClientId == lps.ClientId)
                {
                    pc.SetLobbyPlayerState(lps);
                }
            }
        }
    }

    private void SpawnPlayerObject(ulong clientId)
    {
        PlayerController[] lpc = FindObjectsOfType<PlayerController>();

        for (int i = 0; i < lpc.Length; i++)
        {
            if (lpc[i].GetComponent<NetworkObject>().OwnerClientId == clientId) { return; }
        }

        GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        go.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, null, true);
    }
}
