﻿using UnityEngine;
using System.Collections.Generic;

public enum WorldTypes : int
{
    Default,
    Toxic,
    Electric,
    Fire
}

public class World : MonoBehaviour
{
    public enum Resources : int
    {
        Type1,
        Type2,
        Type3,
        Type4,
        Type5,
        Type6
    }

    public enum RotationDirection
    {
        Left,
        Right
    }

    public CircleCollider2D worldCollider;

    [Range(1.0f, 40.0f)]
    public float gravityRadius = 5.0f;

    [Range(1.0f, 20.0f)]
    public float surfaceRadius = 2.56f;

    [Range(0.1f, 5.0f)]
    public float gravityStrength = 0.5f;

    public Dictionary<Resources, int> resources;

    public RotationDirection rotationDirection;
    public float rotationSpeed;

    [Range(0, 20)]public int numberOfPickups = 10;
    public World.Resources typeOfResource;
    public float pickUpOffset = 1.0f;

    public WorldTypes type;

    private GameObject lockIcon;

    public void Awake()
    {
        resources = new Dictionary<Resources, int>();
        resources.Add(Resources.Type1, 0);
        resources.Add(Resources.Type2, 0);
        resources.Add(Resources.Type3, 0);
        resources.Add(Resources.Type4, 0);
        resources.Add(Resources.Type5, 0);

        Object obj;

        switch (typeOfResource)
        {
            default:
            case Resources.Type1:
                obj = UnityEngine.Resources.Load("PickUps/Pickup1");
                break;
            case Resources.Type2:
                obj = UnityEngine.Resources.Load("PickUps/Pickup2");
                break;
            case Resources.Type3:
                obj = UnityEngine.Resources.Load("PickUps/Pickup3");
                break;
            case Resources.Type4:
                obj = UnityEngine.Resources.Load("PickUps/Pickup4");
                break;
            case Resources.Type5:
                obj = UnityEngine.Resources.Load("PickUps/Pickup5");
                break;
            case Resources.Type6:
                obj = UnityEngine.Resources.Load("PickUps/Pickup6");
                break;
        }

        for (int i = 0; i < numberOfPickups; i++)
        {
            float angle = (360 / numberOfPickups) * i;
            GameObject newPickup = (GameObject)Instantiate(obj, Vector3.zero, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle - 120.0f)));
            newPickup.transform.parent = transform;
            newPickup.transform.localScale = new Vector3(1.0f / transform.localScale.x * 0.6f, 1.0f / transform.localScale.y * 0.6f, 0.0f);
            newPickup.transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * (surfaceRadius + pickUpOffset), Mathf.Sin(angle * Mathf.Deg2Rad) * (surfaceRadius + pickUpOffset)) + transform.position;

            newPickup.GetComponent<Pickup>().world = this;
        }

        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0, 360)));

        lockIcon = (GameObject)Instantiate(UnityEngine.Resources.Load("WorldIconLock", typeof (GameObject)));
        lockIcon.transform.SetParent(transform, false);
        lockIcon.transform.localPosition = Vector3.zero;
    }

    public Vector3 GetWorldScale(Transform transform)
    {
        Vector3 worldScale = transform.localScale;
        Transform parent = transform.parent;

        while (parent != null)
        {
            worldScale = Vector3.Scale(worldScale, parent.localScale);
            parent = parent.parent;
        }

        return worldScale;
    }

    public void Update()
    {
        if (lockIcon != null)
        {
            lockIcon.transform.rotation = Quaternion.Euler(Vector3.zero);

            if (GameController.instance.worldTypesUnlocked[(int)type] == true)
            {
                Destroy(lockIcon);
                lockIcon = null;
            }
        }

        transform.Rotate(new Vector3(0, 0, (rotationDirection == RotationDirection.Left ? rotationSpeed : -rotationSpeed) * Time.deltaTime));

        UpdateScale();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);

        Gizmos.color = new Color(1.0f, 0.5f, 0.5f, 1.0f);
        Gizmos.DrawWireSphere(transform.position, surfaceRadius);

        UpdateScale();
    }

    public void UpdateScale()
    {
        transform.localScale = new Vector3((1.0f / 5.12f) * surfaceRadius, (1.0f / 5.12f) * surfaceRadius);
    }
}
