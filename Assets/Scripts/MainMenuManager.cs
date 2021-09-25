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
    public GameObject MainButtons;
    public GameObject InfosSub;

    private GameObject IPText;
    private GameObject IPInputField;
    private GameObject PseudoInputField;
    private GameObject PasswordField;
    private GameObject PasswordText;
    private GameObject PlayButton;

    private bool _is_host = false;
    private string _Pseudo;
    private string _IP;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
        ReferenceComponent();

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ReferenceComponent()
    {
        Transform[] childs = InfosSub.transform.GetComponentsInChildren<Transform>();
        foreach (Transform tr in childs)
        {
            GameObject go = tr.gameObject;

            if (go.name == "IPText")
            {
                IPText = go;
            }
            if (go.name == "IPInputField")
            {
                IPInputField = go;
            }
            if (go.name == "PseudoInputField")
            {
                PseudoInputField = go;
            }
            if (go.name == "PlayButton")
            {
                PlayButton = go;
            }
            if (go.name == "PasswordField")
            {
                PasswordField = go;
            }
            if (go.name == "PasswordText")
            {
                PasswordText = go;
            }
        }
    }

    private void ShowMainMenu()
    {
        MainButtons.SetActive(true);
        InfosSub.SetActive(false);
    }

    private void ShowInfoMenu()
    {
        MainButtons.SetActive(false);
        InfosSub.SetActive(true);

        if (_is_host)
        {
            IPText.SetActive(false);
            IPInputField.SetActive(false);
            PasswordText.SetActive(false);
            PasswordField.SetActive(false);
        }
        else
        {
            IPText.SetActive(true);
            IPInputField.SetActive(true);
            PasswordText.SetActive(true);
            PasswordField.SetActive(true);
        }
    }

    // Cette fonction vérifie l'état du formulaire
    // Si les infos sont correctes, rends le bouton jouer disponible
    public void CheckInfos()
    {
        if (!PseudoInputField && !PlayButton)
            return;

        if (!_is_host)
        {
            // TO-DO
            // Regex d'adresse IP si on garde la feature
            // La regex devrait ressembler à ça avec le port "^([0-9]{1,3}\.){3}[0-9]{1,3}(:[0-9]{1,5})?$"

            if (PseudoInputField.GetComponent<InputField>().text.Length > 2 && PasswordField.GetComponent<InputField>().text.Length == 5)
            {
                PlayButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                PlayButton.GetComponent<Button>().interactable = false;
            }

        }
        else
        {
            if (PseudoInputField.GetComponent<InputField>().text.Length > 2)
            {
                PlayButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                PlayButton.GetComponent<Button>().interactable = false;
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

    public void PlayButton_Click()
    {
        // TO-DO
        // A améliorer

        // Va être supprimé
        ServerInfos.IP = IPInputField.GetComponent<InputField>().text;
        ServerInfos.Pseudo = PseudoInputField.GetComponent<InputField>().text;

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
            NetworkSceneManager.SwitchScene("Léo");
        }
        // Se connecter en tant que client
        else
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(PasswordField.GetComponent<InputField>().text);
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = PasswordField.GetComponent<InputField>().text.ToUpper();
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
