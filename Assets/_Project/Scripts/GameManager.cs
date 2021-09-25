using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogError("Game Manager null");

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance is null)
            _instance = this;

        DontDestroyOnLoad(this);
    }

// Data
    private string _ip_adress;
    public string ip_adress
    {
        get
        {
            if (_ip_adress is null)
            {
                Debug.LogError("No IP Adress");
                return "";
            }

            return _ip_adress;
        }
        set
        {
            _ip_adress = value;
        }
    }

    private string _password;
    public string password
    {
      get
      {
        if (_password is null)
        {
          Debug.LogError("No password");
          return "";
        }

        return _password;
      }
      set
      {
        _password = value;
      }
    }

    // Apparement cette valeur peut être obtenu dans le singleton du NetworkManager
    private bool _is_host;
    public bool is_host
    {
        get
        {
            return _is_host;
        }
        set
        {
            _is_host = value;
        }
    }

    private Dictionary<ulong, Player> _players;
    public Dictionary<ulong, Player> players
    {
        get
        {
            if (_players is null)
            {
                _players = new Dictionary<ulong, Player>();
            }

            return _players;
        }
        set
        {
            _players = value;
        }
    }
}
