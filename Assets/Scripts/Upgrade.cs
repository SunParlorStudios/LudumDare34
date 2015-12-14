using UnityEngine;
using System.Collections;

public abstract class Upgrade : ScriptableObject
{
    public abstract void Execute();

    public void SetUnlocked(bool value)
    {
        unlocked = value;
    }

    private bool unlocked = false;
}

public class PlanetUnlock : Upgrade
{
    public void SetType(GameController.WorldUnlockTypes type);
}
