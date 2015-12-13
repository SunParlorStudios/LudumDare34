using UnityEngine;
using System.Collections.Generic;

public class BaseController : MonoBehaviour
{
    public World world;
    public HomeWorld homeWorld;
    public float growStrengthPerResource;
    public float smoothing;

    public GameObject[] buildings;
    [Tooltip("The threshold size (size of growth of the planet) of a delivery, if met spawns a homebase object")]
    public float thresholdSize;

    public float surfaceOffset;

    private float interpolateTime;
    private bool doInterpolate;

    private GameObject buildingsObj;

    private float baseSurfaceRadius;
    private float baseGravityRadius;
    private float beginSurfaceRadius;
    private float beginGravityRadius;
    private float endSurfaceRadius;
    private float endGravityRadius;

    public void Awake()
    {
        interpolateTime = 0.0f;
        doInterpolate = false;

        baseSurfaceRadius = world.surfaceRadius;
        baseGravityRadius = world.gravityRadius;

        homeWorld.OnDeliverResources += OnDeliverResources;

        buildingsObj = GameObject.Find("WorldHomeVisuals");
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

        endSurfaceRadius = baseSurfaceRadius + numResources * growStrengthPerResource;
        endGravityRadius = baseGravityRadius + numResources * growStrengthPerResource;

        if (endSurfaceRadius - beginSurfaceRadius > thresholdSize)
        {
            GameObject obj = Instantiate(buildings[(int)(Random.value * buildings.Length)]);
            obj.transform.parent = buildingsObj.transform;

            float angle = Random.Range(0, 360);

            obj.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
            obj.transform.localPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * ((1.0f / 5.12f) * endSurfaceRadius + surfaceOffset), Mathf.Sin(angle * Mathf.Deg2Rad) * ((1.0f / 5.12f) * endSurfaceRadius + surfaceOffset), transform.position.z + 10.0f);
            obj.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle - 90.0f));
        }

        for (int i = 0; i < buildingsObj.transform.childCount; i++)
        {
            Transform trans = buildingsObj.transform.GetChild(i);
            
            float angle = Mathf.Atan2(trans.localPosition.y, trans.localPosition.x) * Mathf.Rad2Deg;

            trans.localScale = new Vector3(3.0f, 3.0f, 1.0f);
            trans.localPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * ((1.0f / 5.12f) * endSurfaceRadius + surfaceOffset), Mathf.Sin(angle * Mathf.Deg2Rad) * ((1.0f / 5.12f) * endSurfaceRadius + surfaceOffset), -5.0f);
            trans.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle - 90.0f));
        }
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
