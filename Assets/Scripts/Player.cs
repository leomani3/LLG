using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _id;
    public int id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    private string _pseudo;
    public string pseudo
    {
        get
        {
            if (_pseudo is null)
            {
                Debug.LogError("Player pseudo is null");
                return "";
            }

            return _pseudo;
        }
        set
        {
            _pseudo = value;
        }
    }
}
