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
    private NetworkList<AvailableLevelState> _availableLevelsState;

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

    private void DestroyChildrenPanel()
    {
        while (levelsContainerPanel.transform.childCount > 0)
        {
            Destroy(levelsContainerPanel.transform.GetChild(0).gameObject);
        }
    }

    private void InstantiateAvailableLevelState()
    {
        _availableLevelsState.Clear();

        for (int i = _pageNumber * 8 + 1; i <= (_pageNumber + 1) * 8; i++)
        {
            AvailableLevelState als = new AvailableLevelState(i, i + 1 <= _highestLevelCleared);
            _availableLevelsState.Add(als);
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
        DestroyChildrenPanel();

        //Instantie les boutons de selection de niveau quand création de niveaux
        foreach (AvailableLevelState als in _availableLevelsState)
        {
            // Je me suis un peu emballé, ça va dans ALSC
            GameObject go = Instantiate(levelSelectionPrefab);
            LevelSelectButton lsb = go.GetComponent<LevelSelectButton>();

            if (lsb == null)
            {
                Debug.LogError("Button Level Selector prefab not references, probably");
            }
            else
            {
                lsb.SetText(als.LevelNumber.ToString());
                lsb.SetAvailable(als.IsAvailable);
                lsb.SetDone(als.IsDone);

                Button b = lsb.GetComponent<Button>();
                if (b == null)
                {
                    Debug.LogError("Pas de button oops");
                }
                else
                {
                    b.onClick.AddListener(delegate {ChangeSceneOnButtonPressedServerRpc(als.LevelNumber); });
                }
            }
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
