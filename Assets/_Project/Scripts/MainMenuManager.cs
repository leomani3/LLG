using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.SceneManagement;
using MLAPI.Transports.Tasks;
using MLAPI.Transports.UNET;
using System;
using System.Text;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainButtons;
    [SerializeField] private GameObject _infosSub;

    [SerializeField] private GameObject _ipText;
    [SerializeField] private GameObject _ipField;
    [SerializeField] private GameObject _pseudoField;
    [SerializeField] private GameObject _passwordField;
    [SerializeField] private GameObject _passwordText;
    [SerializeField] private GameObject _playButton;

    private bool _is_host = false;
    private string _pseudo;
    private string _ip;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ShowMainMenu()
    {
        _mainButtons.SetActive(true);
        _infosSub.SetActive(false);
    }

    private void ShowInfoMenu()
    {
        _mainButtons.SetActive(false);
        _infosSub.SetActive(true);

        if (_is_host)
        {
            _ipText.SetActive(false);
            _ipField.SetActive(false);
            _passwordText.SetActive(false);
            _passwordField.SetActive(false);
        }
        else
        {
            _ipText.SetActive(true);
            _ipField.SetActive(true);
            _passwordText.SetActive(true);
            _passwordField.SetActive(true);
        }
    }

    // Cette fonction vérifie l'état du formulaire
    // Si les infos sont correctes, rends le bouton jouer disponible
    public void CheckInfos()
    {
        if (!_pseudoField && !_playButton)
            return;

        if (!_is_host)
        {
            // TO-DO
            // Regex d'adresse IP si on garde la feature
            // La regex devrait ressembler à ça avec le port "^([0-9]{1,3}\.){3}[0-9]{1,3}(:[0-9]{1,5})?$"

            if (_pseudoField.GetComponent<InputField>().text.Length > 2 && _passwordField.GetComponent<InputField>().text.Length == 5)
            {
                _playButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                _playButton.GetComponent<Button>().interactable = false;
            }

        }
        else
        {
            if (_pseudoField.GetComponent<InputField>().text.Length > 2)
            {
                _playButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                _playButton.GetComponent<Button>().interactable = false;
            }
        }
    }

// Button
    public void HostButton_Click()
    {
        _is_host = true;
        ShowInfoMenu();
    }

    public void JoinButton_Click()
    {
        _is_host = false;
        ShowInfoMenu();
    }

    //TO DO
    public void SettingsButton_Click()
    {

    }

    public void BackButton_Click()
    {
        ShowMainMenu();
    }

    // Unity se charge de tout unload normalement non ?
    public void QuitButton_Click()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void _playButton_Click()
    {
        // TO-DO
        // A améliorer

        // Va être supprimé
        ServerInfos.IP = _ipField.GetComponent<InputField>().text;
        ServerInfos.Pseudo = _pseudoField.GetComponent<InputField>().text;

        // Hebergement de la partie
        if (_is_host)
        {
            StringBuilder strbuilder = new StringBuilder();
            System.Random random = new System.Random();

            for (int i = 0; i < 5; i++)
            {
                double myFloat = random.NextDouble();
                var myChar = Convert.ToChar(Convert.ToInt32(Math.Floor(25 * myFloat) + 65));
                strbuilder.Append(myChar);
            }

            GameManager.Instance.password = strbuilder.ToString().ToUpper();

            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartHost();
            NetworkSceneManager.SwitchScene("Lobby");
        }
        // Se connecter en tant que client
        else
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(_passwordField.GetComponent<InputField>().text);
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = _passwordField.GetComponent<InputField>().text.ToUpper();
            SocketTasks st = NetworkManager.Singleton.StartClient();
            // Je sais pas si cette boucle est necessaire ou pas
            while (!st.IsDone)
            {
            }
            if (st.Success)
            {
                // Envoyer le pseudo ici ou dans la connection data ?
                // Normalement le serveur va mettre à jour le client non ?
            }
            else
            {
                // Mettre un message d'erreur
            }
        }

        //SceneManager.LoadScene(1);
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkManager.ConnectionApprovedDelegate callback)
    {
        bool approve = false;
        bool createPlayerObject = true;

        if (GameManager.Instance.password == System.Text.Encoding.Default.GetString(connectionData))
        {
            approve = true;
        }

        ulong? prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("Player");

        // Dans le tuto
        // callback(createPlayerObject, prefabHash, approve, positionToSpawnAt, rotationToSpawnWith);
        callback(createPlayerObject, prefabHash, approve, null, null);
    }
}
