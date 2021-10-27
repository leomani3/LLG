using System;
using DG.Tweening;
using UnityEngine;

public class PressurePlate : WinCondition
{
    private int _nbDetectedEntities;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Validate(true);
        transform.parent.DOScaleY(0.1f, 0.5f);
        _nbDetectedEntities++;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        _nbDetectedEntities--;
        if (_nbDetectedEntities == 0)
        {
            Validate(false);
            transform.parent.DOScaleY(1, 0.5f);
        }
    }
}