using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<PlayerController> players;
    [SerializeField] private LevelMechanicType levelMechanicType;
    [SerializeField] private LevelMechanicData mechanicData;

    private List<WinCondition> _winConditions;
    private int _nbWinConditionsValid;

    private void Awake()
    {
        _winConditions = GetComponentsInChildren<WinCondition>().ToList();
        foreach (WinCondition winCondition in _winConditions)
        {
            winCondition.onUnvalid += OnWinConditionUnvalidate;
            winCondition.onValid += OnWinConditionValidate;
        }
        
        foreach (PlayerController playerController in players)
        {
            LevelMechanicFactory.Create(levelMechanicType, playerController, mechanicData);
        }
    }

    public void OnWinConditionUnvalidate()
    {
        _nbWinConditionsValid--;
    }

    public void OnWinConditionValidate()
    {
        _nbWinConditionsValid++;
        if (_nbWinConditionsValid == _winConditions.Count)
        {
            //Trigger la win
            print("win");
        }
    }
}