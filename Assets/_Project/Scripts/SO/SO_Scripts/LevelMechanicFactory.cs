using UnityEngine;

public enum LevelMechanicType{Bump}
public static class LevelMechanicFactory
{
    public static void Create(LevelMechanicType levelMechanicType, PlayerController playerToAssignTo, LevelMechanicData mechanicData)
    {
        switch (levelMechanicType)
        {
            case LevelMechanicType.Bump :
                BumpLevelMechanic bumpLevelMechanic = playerToAssignTo.gameObject.AddComponent<BumpLevelMechanic>();
                bumpLevelMechanic.AssigneObject = playerToAssignTo.gameObject;

                bumpLevelMechanic.bumpForce = mechanicData.bumpForce;
                bumpLevelMechanic.bumpLayers = mechanicData.bumpLayers;
                bumpLevelMechanic.bumpRadius = mechanicData.bumpRadius;
                bumpLevelMechanic.bumpFXPoolRef = mechanicData.bumpFXPoolRef;
                
                playerToAssignTo.SetLevelMechanic(bumpLevelMechanic);
                break;
            default:
                break;
        }
    }
}