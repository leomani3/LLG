using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<PlayerController> players;
    [SerializeField] private LevelMechanicType levelMechanicType;
    [SerializeField] private LevelMechanicData mechanicData;

    private ILevelMechanic _levelMechanic;

    private void Awake()
    {
        foreach (PlayerController playerController in players)
        {
            LevelMechanicFactory.Create(levelMechanicType, playerController, mechanicData);
        }
    }
}