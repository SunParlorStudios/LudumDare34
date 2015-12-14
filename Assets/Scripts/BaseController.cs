﻿using UnityEngine;
using System.Collections.Generic;

public class BaseController : MonoBehaviour
{
    public World world;
    public ResourceDeliverer resourceDeliverer;
    public GameController gameController;
    public CameraController cameraController;
    public float growthSmoothing;
    public float growthPerLevel;

    public GameObject[] buildings;
    public Dictionary<World.Resources, int> resources;

    public Player player;

    private float interpolateTime;
    private bool doInterpolate;

    private float baseSurfaceRadius;
    private float baseGravityRadius;
    private float beginSurfaceRadius;
    private float beginGravityRadius;
    private float endSurfaceRadius;
    private float endGravityRadius;
    private List<World> worldsToBeSwallowed;

    public float resourceTimer = 60.0f;
    public float resourceTimerMax = 60.0f;

    public void Awake()
    {
        resources = new Dictionary<World.Resources, int>();
        resources.Add(World.Resources.Type1, 0);
        resources.Add(World.Resources.Type2, 0);
        resources.Add(World.Resources.Type3, 0);
        resources.Add(World.Resources.Type4, 0);
        resources.Add(World.Resources.Type5, 0);
        resources.Add(World.Resources.Type6, 0);

        resourceDeliverer.OnDeliverResources += OnDeliverResources;

        cameraController.onFocusedWorld += CameraFocusedWorld;

        baseSurfaceRadius = world.surfaceRadius;
        baseGravityRadius = world.gravityRadius;
    }

    private void CameraFocusedWorld()
    {
        // Interpolate the world
        doInterpolate = true;
        interpolateTime = 0.0f;
    }

    private void OnDeliverResources()
    {
        resourceTimer = resourceTimerMax;

        if (gameController.RequirementsMet(resources) && gameController.requirements[gameController.currentLevel].finalLevel == false)
        {
            UIHandler.instance.Show();
            DoUpgrade();
        }
    }

    public void DoUpgrade()
    {
        player.invincible = true;
        player.delivering = true;
        gameController.currentLevel++;
        gameController.DoNextMissionText();

        if (doInterpolate == false)
        {
            // Reset the resources
            Dictionary<World.Resources, int> newResources = new Dictionary<World.Resources, int>();
            foreach (KeyValuePair<World.Resources, int> entry in resources)
            {
                newResources.Add(entry.Key, resources[entry.Key] - GameController.GetValueFromRequirement(gameController.requirements[gameController.currentLevel], (int)entry.Key));
            }
            resources = newResources;

            beginSurfaceRadius = world.surfaceRadius;
            beginGravityRadius = world.gravityRadius;
            endSurfaceRadius = world.surfaceRadius * growthPerLevel;
            endGravityRadius = world.gravityRadius * growthPerLevel;

            worldsToBeSwallowed = gameController.FindWorldsInRadius(transform.position, endSurfaceRadius);

            cameraController.state = CameraController.State.FocusWorld;
            cameraController.offsetZ = -10 - world.surfaceRadius;

            for (int i = 0; i < worldsToBeSwallowed.Count; i++)
            {
                for (int j = 0; j < worldsToBeSwallowed[i].transform.childCount; j++)
                {
                    if (worldsToBeSwallowed[i].transform.GetChild(j).name.Contains("Pickup"))
                        Destroy(worldsToBeSwallowed[i].transform.GetChild(j).gameObject);
                }
            }
        }
    }

    public void EndUpgrade()
    {
        doInterpolate = false;
        interpolateTime = 0.0f;

        cameraController.state = CameraController.State.Default;
        cameraController.offsetZ = -10;

        foreach (World currentWorld in worldsToBeSwallowed)
        {
            if (currentWorld != world)
                gameController.DestroyWorld(currentWorld);
        }

        float angle = Mathf.Atan2(player.transform.position.y - world.transform.position.y, player.transform.position.x - world.transform.position.x);
        player.transform.position = new Vector3(Mathf.Cos(angle) * world.surfaceRadius + world.transform.position.x, Mathf.Sin(angle) * world.surfaceRadius + world.transform.position.y, player.transform.position.z);
        player.invincible = false;
        player.delivering = false;
    }

    public void Update()
    {
        if (doInterpolate == true)
        {
            float angle = Mathf.Atan2(player.transform.position.y - world.transform.position.y, player.transform.position.x - world.transform.position.x);
            player.transform.position = new Vector3(Mathf.Cos(angle) * world.surfaceRadius + player.playerRadius + world.transform.position.x, Mathf.Sin(angle) * world.surfaceRadius + player.playerRadius + world.transform.position.y, player.transform.position.z);
            
            interpolateTime += Mathf.Min(Time.deltaTime * growthSmoothing, 1.0f);

            world.surfaceRadius = Mathf.LerpUnclamped(beginSurfaceRadius, endSurfaceRadius, EaseOutElastic(interpolateTime, 0.0f, 1.0f, 1.0f));
            world.gravityRadius = Mathf.LerpUnclamped(beginGravityRadius, endGravityRadius, EaseOutElastic(interpolateTime, 0.0f, 1.0f, 1.0f));

            for (int i = 0; i < worldsToBeSwallowed.Count; i++)
            {
                if (worldsToBeSwallowed[i] != world)
                {
                    worldsToBeSwallowed[i].surfaceRadius = Mathf.Lerp(worldsToBeSwallowed[i].surfaceRadius, 0.0f, Time.deltaTime * 2.0f);
                    worldsToBeSwallowed[i].transform.position = Vector3.Lerp(worldsToBeSwallowed[i].transform.position, transform.position, Time.deltaTime * 2.0f);
                }
            }

            if (interpolateTime >= 1.0f)
            {
                EndUpgrade();
            }
        }
        else
        {
            resourceTimer -= Time.deltaTime;

            transform.FindChild("WorldHomeVisualsDestroyed").GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f - resourceTimer / resourceTimerMax);
        }
    }

    private float EaseOutElastic(float t, float b, float c, float d)
    {
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (56 * tc * ts + -175 * ts * ts + 200 * tc + -100 * ts + 20 * t);
    }
}
