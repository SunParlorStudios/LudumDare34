using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelRequirements
{
    public int ResourcesType1;
    public int ResourcesType2;
    public int ResourcesType3;
    public int ResourcesType4;
    public int ResourcesType5;
    public int ResourcesType6;
    public bool finalLevel = false;
}

public class GameController : MonoBehaviour
{
    public List<World> worlds;

    public LevelRequirements[] requirements;
    public int currentLevel;

    public void Awake()
    {
        worlds = new List<World>();

        GameObject[] sceneWorlds = GameObject.FindGameObjectsWithTag("World");
        for (int i = 0; i < sceneWorlds.Length; i++)
        {
            worlds.Add(sceneWorlds[i].GetComponent<World>());
        }
    }

    public World FindClosestWorld(Vector3 position)
    {
        World closestWorld = null;
        float closestDistance = Mathf.Infinity;

        float distance;

        for (int i = 0; i < worlds.Count; i++)
        {
            distance = GetBorderDistance(worlds[i], position);

            if (closestWorld == null || distance < closestDistance)
            {
                closestWorld = worlds[i];
                closestDistance = distance;
            }
        }

        return closestWorld;
    }

    public List<World> FindInGravityRadius(Vector3 position)
    {
        List<World> worldsInRange = new List<World>();

        for (int i = 0; i < worlds.Count; i++)
        {
            if (worlds[i] != null && Vector3.Distance(position, worlds[i].transform.position) <= worlds[i].gravityRadius)
            {
                worldsInRange.Add(worlds[i]);
            }
        }

        return worldsInRange;
    }

    public List<World> FindWorldsInRadius(Vector3 position, float radius)
    {
        List<World> worldsInRange = new List<World>();

        for (int i = 0; i < worlds.Count; i++)
        {
            if (worlds[i] != null && Vector3.Distance(position, worlds[i].transform.position) <= radius + worlds[i].surfaceRadius)
            {
                worldsInRange.Add(worlds[i]);
            }
        }

        return worldsInRange;
    }

    private float GetBorderDistance(World world, Vector3 position)
    {
        return (world.gameObject.transform.position - position).magnitude - world.gravityRadius;
    }

    public bool RequirementsMet(Dictionary<World.Resources, int> resources)
    {
        if (resources[World.Resources.Type1] >= requirements[currentLevel].ResourcesType1 &&
            resources[World.Resources.Type2] >= requirements[currentLevel].ResourcesType2 &&
            resources[World.Resources.Type3] >= requirements[currentLevel].ResourcesType3 &&
            resources[World.Resources.Type4] >= requirements[currentLevel].ResourcesType4 &&
            resources[World.Resources.Type5] >= requirements[currentLevel].ResourcesType5 &&
            resources[World.Resources.Type6] >= requirements[currentLevel].ResourcesType6)
        {
            return true;
        }
        return false;
    }

    public void DestroyWorld(World world)
    {
        for (int i = 0; i < worlds.Count; i++)
        {
            if (world == worlds[i])
            {
                Destroy(worlds[i].gameObject);
                worlds.RemoveAt(i);
            }
        }
    }
}
