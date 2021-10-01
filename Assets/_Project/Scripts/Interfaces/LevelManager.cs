using System;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.Spawning;
using MLAPI.NetworkVariable.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class LevelManager : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private int levelNumber;
    [SerializeField] private bool isInPause = false;
    //[SerializeField] private Door door;

    private NetworkList<LevelStateObject> _levelStateObjects = new NetworkList<LevelStateObject>();


    // Fonction qui va vérifier si les conditions sont bonnes pour valider le niveau
    private abstract bool ConditionsAreMet();


    // Cette fonction initie un NetworkObject sur le GameObject s'il n'en possède pas
    // Sans le NetworkObject, le NetworkManager ne sait pas que le NetworkBehaviour existe
    // et ne trigger pas NetworkStart.
    // DISCLAIMER : Je ne sais pas si ça fonctionne
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
            _levelStateObjects.OnListChanged += HandleLevelStateObjectsChanged;
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
        _levelStateObjects.OnListChanged -= HandleLobbyPlayersStateChanged;

        if (NetworkManager.Singleton)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }
    }

    // L'utilité première de cette fonction sert à vérifier que tous les joueurs sont là après une reconnexion
    // Si c'est le cas le niveau ne sera plus en pause
    // Elle peut être aussi utilisé pour instancier les PlayerNetworkObject dans le cas où en changeant de niveaux
    // ils seraient destroy
    private void HandleClientConnected(ulong clientId)
    {

    }

    // Si un joueur se déconnecte pendant la partie, déclenche la pause
    // SERVER
    // Ajouter une condition qui permettent au client de se reconnecter
    // De base ça devrait spawn un PlayerNetworkObject mais on veut éviter ça, où alors le spawn à l'endroit où il a été déconnecté
    // Dans le cas où on le respawn, on perdra les valeurs de déplacement et d'action en cours.
    // Dans l'autre, il faudrait faire en sorte que le PlayerNetworkObject n'est pas Destroy et lui reattribuer à la reconnection
    // UI
    // Afficher un Panel avec le nom de la personne déconnecté et des boutons qui permettent de retourner au lobby
    private void HandleClientDisconnect(ulong clientId)
    {
        isInPause = true;
    }

    //
    private void HandleLevelStateObjectsChanged(NetworkListEvent<LevelStateObject> lso)
    {
        if (ConditionsAreMet())
        {
            // Ouvre la porte
            // door.Open()
        }
        else
        {
            // Ferme la porte
            // door.Close()
        }
    }


}
