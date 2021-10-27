using System;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public Action onValid;
    public Action onUnvalid;

    public bool Valid => _valid;

    private bool _valid;

    public void Validate(bool b)
    {
        _valid = b;

        if (b)
            onValid?.Invoke();
        else
            onUnvalid?.Invoke();
    }
}