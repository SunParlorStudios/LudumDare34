using UnityEngine;
using System.Collections.Generic;

public class BaseController : MonoBehaviour
{
    public World world;
    public HomeWorld homeWorld;
    public float growStrengthPerResource;

    public float smoothing;

    private float interpolateTime;
    private bool doInterpolate;

    private float beginSurfaceRadius;
    private float beginGravityRadius;
    private float endSurfaceRadius;
    private float endGravityRadius;

    public void Awake()
    {
        interpolateTime = 0.0f;
        doInterpolate = false;

        homeWorld.OnDeliverResources += OnDeliverResources;
    }

    private void OnDeliverResources()
    {
        doInterpolate = true;
        interpolateTime = 0.0f;

        beginSurfaceRadius = world.surfaceRadius;
        beginGravityRadius = world.gravityRadius;

        int numResources = 0;

        foreach (KeyValuePair<World.Resources, int> entry in world.resources)
        {
            numResources += entry.Value;
        }

        endSurfaceRadius = world.surfaceRadius + numResources * growStrengthPerResource;
        endGravityRadius = world.gravityRadius + numResources * growStrengthPerResource;
    }

    public void Update()
    {
        if (doInterpolate)
        {
            interpolateTime += Time.deltaTime * smoothing;

            world.surfaceRadius = Mathf.Lerp(beginSurfaceRadius, endSurfaceRadius, EaseOutElastic(interpolateTime, 0.0f, 1.0f, 1.0f));
            world.gravityRadius = Mathf.Lerp(beginGravityRadius, endGravityRadius, EaseOutElastic(interpolateTime, 0.0f, 1.0f, 1.0f));

            if (interpolateTime >= 1.0f)
            {
                doInterpolate = false;
                interpolateTime = 0.0f;
            }
        }
    }

    private float EaseOutElastic(float t, float b, float c, float d)
    {
        var ts = (t /= d) * t;
        var tc = ts * t;
        return b + c * (56 * tc * ts + -175 * ts * ts + 200 * tc + -100 * ts + 20 * t);
    }
}
