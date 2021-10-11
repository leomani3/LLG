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

    public void Awake()
    {
        NetworkObject no = gameObject.GetComponent<NetworkObject>();

        if (no == null)
        {
            gameObject.AddComponent<NetworkObject>();
        }
    }

    public override void NetworkStart()
    {
        if (IsClient)
        {
        }
        if (IsServer)
        {
            previousButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
    }

    [ServerRpc(RequireOwnership = true)]
    private void NextPanelServerRpc(ServerRpcParams serverRpcParams = default)
    {
        DestroyChildrenPanel();
        _pageNumber++;

        UpdateButton();
    }
    
    [ServerRpc(RequireOwnership = true)]
    private void PreviousPanelServerRpc(ServerRpcParams serverRpcParams = default)
    {
        DestroyChildrenPanel();
        _pageNumber--;

        UpdateButton();
    }

    private void DestroyChildrenPanel()
    {
        while (levelsContainerPanel.transform.childCount > 0)
        {
            Destroy(levelsContainerPanel.transform.GetChild(0).gameObject);
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

}
