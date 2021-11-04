using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using DG.Tweening;

public class PressableButton : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Transform RedButton;
    private NetworkVariable<bool> isPressed = new NetworkVariable<bool>(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.Everyone}, false);
    
    private int _peopleNumber = 0;
    private Tween _moveTween;

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _moveTween.Complete();
        _peopleNumber++;
        if (_peopleNumber == 1)
        {       
            _moveTween = RedButton.DOMove(new Vector3(transform.position.x, transform.position.y - 0.17f, transform.position.z), 1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _moveTween.Kill();
        _peopleNumber--;
        if (_peopleNumber == 0)
        {
            _moveTween = RedButton.DOMove(new Vector3(transform.position.x, transform.position.y + 0.17f, transform.position.z), 0.5f);
        }
    }
}
