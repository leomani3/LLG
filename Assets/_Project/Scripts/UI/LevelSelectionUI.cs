using System;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.Spawning;
using MLAPI.NetworkVariable.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectionUI : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject levelSelectionPrefab;
    [SerializeField] private GameObject levelsContainerPanel;

    private int _pageNumber = 0;
    private int _highestLevelCleared = 0;
    private NetworkList<AvailableLevelState> _availableLevelsState = new NetworkList<AvailableLevelState>();

    public void Awake()
    {
        NetworkObject no = gameObject.GetComponent<NetworkObject>();

        if (no == null)
        {
            gameObject.AddComponent<NetworkObject>();
        }

        previousButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    public override void NetworkStart()
    {
        if (IsClient)
        {
            _availableLevelsState.OnListChanged += AvailableLevelStateChanged;
        }
        if (IsServer)
        {
            previousButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);

            InstantiateAvailableLevelState();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void NextPanelServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _pageNumber++;
        UpdateButton();
        InstantiateAvailableLevelState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void PreviousPanelServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _pageNumber--;
        UpdateButton();
        InstantiateAvailableLevelState();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeSceneOnButtonPressedServerRpc(int sceneNumber, ServerRpcParams serverRpcParams = default)
    {
        ServerGameNetPortal.Instance.LoadLevel(sceneNumber);
    }

    private void InstantiateAvailableLevelState()
    {
        PopulateAvailableLevelState();
        int lvNbr = _pageNumber * 8;

        for (int i = 0; i < _availableLevelsState.Count; i++)
        {
            _availableLevelsState[i] = new AvailableLevelState(i + lvNbr + 1, i + lvNbr + 1 <= _highestLevelCleared + 1);
        }
    }

    private void PopulateAvailableLevelState()
    {
        if (_availableLevelsState.Count > 1) { return; }

        for (int i = 0; i < 8; i++)
        {
            _availableLevelsState.Add(new AvailableLevelState(i + 1, false));
        }
    }

    private void UpdateButton()
    {
        if (_pageNumber == 0)
        {
            previousButton.gameObject.SetActive(false);
        }
        else
        {
            previousButton.gameObject.SetActive(true);
        }

        // A faire avec le nombre de niveau disponible
        if (_pageNumber == 3)
        {
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
        }
    }

    private void AvailableLevelStateChanged(NetworkListEvent<AvailableLevelState> alState)
    {
        LevelSelectButton[] lsbs = levelsContainerPanel.GetComponentsInChildren<LevelSelectButton>();

        for (int i = 0; i < 8; i++)
        {
            if (_availableLevelsState.Count <= i || lsbs.Length <= i) { break; }
            AvailableLevelState als = _availableLevelsState[i];

            lsbs[i].SetText(als.LevelNumber.ToString());
            lsbs[i].SetAvailable(als.IsAvailable);
            lsbs[i].SetDone(als.IsDone);
            lsbs[i].SetUI(this);
        }
    }

    public void PreviousButtonClicked()
    {
        PreviousPanelServerRpc();
    }

    public void NextButtonClicked()
    {
        NextPanelServerRpc();
    }
}
