using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _instance = this;
    }

// Data
    private static string _ip_adress;
    public static string ip_adress
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

    // Apparement cette valeur peut être obtenu dans le singleton du NetworkManager
    private static bool _is_host;
    public static bool is_host
    {
        get
        {
            if (_is_host is null)
            {
                Debug.LogError("Host not set");
                return false;
            }
            return _is_host;
        }
        set
        {
            _is_host = value;
        }
    }

    private static Dictionary<int, Player> _players;
    public static Dictionary<int, Player> players
    {
        get
        {
            if (_players is null)
            {
                _players = new Dictionary<int, Player>();
            }

            return _players;
        }
        set
        {
            _players = value;
        }
    }
}
