using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_InputField _pseudoInputField;
    [SerializeField] private TMP_InputField _adresseIpInputField;
    [SerializeField] private GameObject _playerInfoPanel;
    [SerializeField] private GameObject _ipAdressePanel;

    private void Start()
    {
        PlayerPrefs.GetString("PlayerName");
        PlayerPrefs.GetString("IpAdresse");

        ShowIpAdress(false);
    }

    public void OnHostClicked()
    {
        PlayerPrefs.SetString("PlayerName", _pseudoInputField.text);

        // Start du serveur
        ServerGameNetPortal.Instance.StartHost();
    }

    public void OnJoinClicked()
    {
        // Si on a déjà entré l'adresse ip
        if (_ipAdressePanel.activeSelf)
        {
            PlayerPrefs.SetString("PlayerName", _pseudoInputField.text);
            PlayerPrefs.SetString("IpAdress", _adresseIpInputField.text);

            // Start du client
            ClientGameNetPortal.Instance.StartClient();
        }
        else
        {
            ShowIpAdress(true);
        }
    }

    public void OnBackClicked()
    {
        ShowIpAdress(false);
    }

    private void ShowIpAdress(bool show)
    {
        _playerInfoPanel.SetActive(!show);
        _ipAdressePanel.SetActive(show);
    }
}
