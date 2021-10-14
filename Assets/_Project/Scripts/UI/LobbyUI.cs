using System;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.Spawning;
using MLAPI.NetworkVariable.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUI : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private TMP_Text playerReadyText;
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

            startGameButton.gameObject.SetActive(true);

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

        playerReadyText.text = NumberPlayerReady().ToString() + "/" + _lobbyPlayers.Count.ToString();

        if (IsHost)
        {
            startGameButton.interactable = IsEveryoneReady();
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

    [ServerRpc(RequireOwnership = false)]
    private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < _lobbyPlayers.Count; i++)
        {
            if (_lobbyPlayers[i].ClientId == serverRpcParams.Receive.SenderClientId)
            {
                _lobbyPlayers[i] = new LobbyPlayerState(
                    _lobbyPlayers[i].ClientId,
                    _lobbyPlayers[i].PlayerName,
                    !_lobbyPlayers[i].IsReady,
                    _lobbyPlayers[i].NumberColor
                );
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        if (serverRpcParams.Receive.SenderClientId != NetworkManager.Singleton.LocalClientId) { return; }

        if (!IsEveryoneReady()) { return; }

        ServerGameNetPortal.Instance.StartGame();
    }

    public void OnReadySelected()
    {
        ToggleReadyServerRpc();
    }

    public void OnStartGameClicked()
    {
        StartGameServerRpc();
    }

    private bool IsEveryoneReady()
    {
        if (_lobbyPlayers.Count < 2) { return false; }

        if (NumberPlayerReady() != _lobbyPlayers.Count) { return false; }

        return true;
    }

    private int NumberPlayerReady()
    {
        int i = 0;

        foreach (LobbyPlayerState lps in _lobbyPlayers)
        {
            if (lps.IsReady) { i++; }
        }

        return i;
    }
}
