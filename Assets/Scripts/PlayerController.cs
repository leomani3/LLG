using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MLAPI;
using MyBox;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += transform.right;
        }
    }
}