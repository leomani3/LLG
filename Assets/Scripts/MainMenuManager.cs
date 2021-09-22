using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainButtons;
    public GameObject InfosSub;

    private GameObject IPText;
    private GameObject IPInputField;
    private GameObject PseudoInputField;
    private GameObject PlayButton;

    private bool IsHost = false;
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

        if (IsHost)
        {
            IPText.SetActive(false);
            IPInputField.SetActive(false);
        }
        else
        {
            IPText.SetActive(true);
            IPInputField.SetActive(true);
        }
    }

    // Cette fonction vérifie l'état du formulaire
    // Si les infos sont correctes, rends le bouton jouer disponible
    public void CheckInfos()
    {
        if (!PseudoInputField && !PlayButton)
            return;

        if (!IsHost)
        {
            // TO-DO 
            // Regex d'adresse IP si on garde la feature
            // La regex devrait ressembler à ça avec le port "^([0-9]{1,3}\.){3}[0-9]{1,3}(:[0-9]{1,5})?$"
        }
        if (PseudoInputField.GetComponent<InputField>() && PseudoInputField.GetComponent<InputField>().text.Length > 2)
        {
            PlayButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            PlayButton.GetComponent<Button>().interactable = false;
        }
    }

// Button
    public void HostButton_Click()
    {
        IsHost = true;
        ShowInfoMenu();
    }

    public void JoinButton_Click()
    {
        IsHost = false;
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
        ServerInfos.IP = IPInputField.GetComponent<InputField>().text;
        ServerInfos.Pseudo = PseudoInputField.GetComponent<InputField>().text;
        SceneManager.LoadScene(1);
    }
}
