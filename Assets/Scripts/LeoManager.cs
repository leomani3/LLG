using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class LeoManager : MonoBehaviour
{
    [SerializeField]private Text PasswordText;

    private void Awake()
    {
        PasswordText.text = GameManager.Instance.password;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
