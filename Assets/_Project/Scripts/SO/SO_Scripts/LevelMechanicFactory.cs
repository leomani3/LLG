using UnityEngine;

public enum LevelMechanicType{Bump}
public static class LevelMechanicFactory
{
    public static ILevelMechanic Create(LevelMechanicType levelMechanicType, GameObject assignedGameobject)
    {
        switch (levelMechanicType)
        {
            case LevelMechanicType.Bump :
                BumpLevelMechanic bumpLevelMechanic = new BumpLevelMechanic();
                bumpLevelMechanic.AssigneObject = assignedGameobject;
                
                return bumpLevelMechanic;
                break;
            default:
                return new BumpLevelMechanic();
                break;
        }
    }
}