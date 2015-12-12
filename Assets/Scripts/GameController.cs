using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private List<World> worlds;

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
            if (Vector3.Distance(position, worlds[i].transform.position) <= worlds[i].gravityRadius)
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
}
