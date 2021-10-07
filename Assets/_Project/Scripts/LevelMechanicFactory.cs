using UnityEngine;

public enum LevelMechanicType{Bump, Grapple}
public static class LevelMechanicFactory
{
    public static void Create(LevelMechanicType levelMechanicType, PlayerController playerToAssignTo, LevelMechanicData mechanicData)
    {
        switch (levelMechanicType)
        {
            case LevelMechanicType.Bump :
                BumpLevelMechanic bumpLevelMechanic = playerToAssignTo.gameObject.AddComponent<BumpLevelMechanic>();

                bumpLevelMechanic.bumpForce = mechanicData.bumpForce;
                bumpLevelMechanic.bumpLayers = mechanicData.bumpLayers;
                bumpLevelMechanic.bumpRadius = mechanicData.bumpRadius;
                bumpLevelMechanic.bumpFXPoolRef = mechanicData.bumpFXPoolRef;
                
                playerToAssignTo.SetLevelMechanic(bumpLevelMechanic);
                break;
            case LevelMechanicType.Grapple :
                GrappleLevelMechanic grappleLevelMechanic = playerToAssignTo.gameObject.AddComponent<GrappleLevelMechanic>();

                grappleLevelMechanic.layerMask = mechanicData.grappleLayerMask;
                
                playerToAssignTo.SetLevelMechanic(grappleLevelMechanic);
                break;
            default:
                break;
        }
    }
}