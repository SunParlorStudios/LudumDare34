using UnityEngine;
using System.Collections;
using System;

public abstract class Upgrade : ScriptableObject
{
    public abstract void Execute();
}

public class PlanetUnlock : Upgrade
{
    public void SetType(WorldTypes type)
    {
        worldType = type;
    }

    public override void Execute()
    {
        GameController.instance.worldTypesUnlocked[(int)worldType] = true;
    }

    private WorldTypes worldType;

    public static PlanetUnlock Create(WorldTypes type)
    {
        PlanetUnlock unlock = CreateInstance<PlanetUnlock>();
        unlock.SetType(type);

        return unlock;
    }
}
