using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelMechanic levelMechanic;
    [SerializeField] private List<PlayerController> players;

    private void Awake()
    {
        foreach (PlayerController playerController in players)
        {
            playerController.SetLevelMechanic(levelMechanic);
        }
    }
}