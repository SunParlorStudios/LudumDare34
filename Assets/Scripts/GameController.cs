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
    public GameObject asteroidPrefab;
    public Player player;
    public Transform asteroidFocusMin;
    public Transform asteroidFocusMax;
    public float cometSpawnRate = 300.0f;

    public bool[] worldTypesUnlocked;
    public bool cannonRotationUnlocked;
    public bool cannonBoosterUnlocked;
    public bool pickupRangeIncreaseUnlocked;
    public bool pickupDoubleUnlocked;
    public bool orbitSpeedIncreased;

    public delegate void OnCometCreatedDelegate(Comet comet);
    public event OnCometCreatedDelegate OnCometCreated;

    public static GameController instance;

    public void Awake()
    {
        worlds = new List<World>();

        GameObject[] sceneWorlds = GameObject.FindGameObjectsWithTag("World");
        for (int i = 0; i < sceneWorlds.Length; i++)
        {
            worlds.Add(sceneWorlds[i].GetComponent<World>());
        }

        asteroidFocusMin = GameObject.Find("AsteroidFocusMin").transform;
        asteroidFocusMax = GameObject.Find("AsteroidFocusMax").transform;

        worldTypesUnlocked = new bool[4];
        worldTypesUnlocked[(int)WorldTypes.Default] = true;
        worldTypesUnlocked[(int)WorldTypes.Electric] = false;
        worldTypesUnlocked[(int)WorldTypes.Toxic] = false;
        worldTypesUnlocked[(int)WorldTypes.Fire] = false;

        cannonRotationUnlocked = false;
        cannonBoosterUnlocked = false;
        pickupRangeIncreaseUnlocked = false;
        pickupDoubleUnlocked = false;
        orbitSpeedIncreased = false;

        instance = this;
    }

    public void FixedUpdate()
    {
        if (Random.Range(0, (int)cometSpawnRate) == 1)
        {
            Comet comet = ((GameObject)Instantiate(asteroidPrefab)).GetComponent<Comet>();

            var hitPoint = Vector3.Lerp(asteroidFocusMin.transform.position, asteroidFocusMax.transform.position, Random.Range(0.0f, 1.0f));

            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            Vector3 start = new Vector3(Mathf.Cos(angle) * 50.0f, Mathf.Sin(angle) * 50.0f, 0.0f);

            float angle2 = Mathf.Atan2(start.y - hitPoint.y, start.x - hitPoint.x) + 180 * Mathf.Deg2Rad;
            Vector3 end = new Vector3(Mathf.Cos(angle2) * 50.0f, Mathf.Sin(angle2) * 50.0f, 0.0f);

            comet.speed = Random.Range(0.3f, 0.4f);
            comet.transform.position = start + player.transform.position;
            comet.start = start + player.transform.position;
            comet.end = end + player.transform.position;

            float angle3 = (Mathf.Atan2(comet.start.y - comet.end.y, comet.start.x - comet.end.x) + Mathf.PI) * Mathf.Rad2Deg;
            comet.transform.rotation = Quaternion.Euler(0, 0, angle3);

            if (OnCometCreated != null)
                OnCometCreated(comet);
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

    public static int GetValueFromRequirement(LevelRequirements requirement, int index)
    {
        switch (index)
        {
            case 0:
                return requirement.ResourcesType1;
            case 1:
                return requirement.ResourcesType2;
            case 2:
                return requirement.ResourcesType3;
            case 3:
                return requirement.ResourcesType4;
            case 4:
                return requirement.ResourcesType5;
            case 5:
                return requirement.ResourcesType6;
            default:
                return 0;
        }
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
