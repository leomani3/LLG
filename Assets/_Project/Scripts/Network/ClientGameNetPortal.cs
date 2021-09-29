using System;
using System.Text;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameNetPortal))]
public class ClientGameNetPortal : MonoBehaviour
{
    public static ClientGameNetPortal Instance => _instance;
    private static ClientGameNetPortal _instance;

    public DisconnectReason DisconnectReason { get; private set; } = new DisconnectReason();

    public event Action<ConnectStatus> OnConnectionFinished;

    public event Action OnNetworkTimedOut;

    private GameNetPortal _gameNetPortal;

    private void Awake()
    {
        if (_instance != null & _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _gameNetPortal = GetComponent<GameNetPortal>();

        _gameNetPortal.OnNetworkReadied += HandleNetworkReadied;
        _gameNetPortal.OnConnectionFinished -= HandleConnectionFinished;
        _gameNetPortal.OnDisconnectReasonReceived += HandleDisconnectReasonReceived;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void OnDestroy()
    {
        if (_gameNetPortal == null) { return; }

        _gameNetPortal.OnNetworkReadied -= HandleNetworkReadied;
        _gameNetPortal.OnConnectionFinished -= HandleConnectionFinished;
        _gameNetPortal.OnDisconnectReasonReceived -= HandleDisconnectReasonReceived;

        if (NetworkManager.Singleton == null) { return; }

        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }

    public void StartClient()
    {
        var payload = JsonUtility.ToJson(new ConnectionPayload()
        {
            clientGUID = Guid.NewGuid().ToString(),
            clientScene = SceneManager.GetActiveScene().buildIndex,
            playerName = PlayerPrefs.GetString("PlayerName", "Missing Name")
        });

        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = PlayerPrefs.GetString("IpAdress", "127.0.0.1");
        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

        NetworkManager.Singleton.StartClient();
    }

    private void HandleNetworkReadied()
    {
        if (!NetworkManager.Singleton.IsClient) { return; }

        if (!NetworkManager.Singleton.IsHost) {
            _gameNetPortal.OnUserDisconnectRequested += HandleUserDisconnectRequested;
        }

        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _gameNetPortal.ClientToServerSceneChanged(SceneManager.GetActiveScene().buildIndex);
    }

    private void HandleUserDisconnectRequested()
    {
        DisconnectReason.SetDisconnectReason(ConnectStatus.UserRequestedDisconnect);
        NetworkManager.Singleton.StopClient();

        HandleClientDisconnect(NetworkManager.Singleton.LocalClientId);

        SceneManager.LoadScene("MainMenu");
    }

    private void HandleConnectionFinished(ConnectStatus status)
    {
        if (status != ConnectStatus.Success)
        {
            DisconnectReason.SetDisconnectReason(status);
        }

        Debug.Log("Je suis connecté");
        OnConnectionFinished?.Invoke(status);
    }

    private void HandleDisconnectReasonReceived(ConnectStatus status)
    {
        DisconnectReason.SetDisconnectReason(status);
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsConnectedClient && !NetworkManager.Singleton.IsHost)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            _gameNetPortal.OnUserDisconnectRequested -= HandleUserDisconnectRequested;

            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                if (!DisconnectReason.HasTransitionReason)
                {
                    DisconnectReason.SetDisconnectReason(ConnectStatus.GenericDisconnect);
                }

                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                OnNetworkTimedOut?.Invoke();
            }
        }
    }
}
