using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<PlayerController> players;
    [SerializeField] private LevelMechanicType levelMechanicType;

    [SerializeField] private ILevelMechanic _levelMechanic;

    private void Awake()
    {
        foreach (PlayerController playerController in players)
        {
            _levelMechanic = LevelMechanicFactory.Create(LevelMechanicType.Bump, playerController.gameObject);
            playerController.SetLevelMechanic(_levelMechanic);
        }
    }
}