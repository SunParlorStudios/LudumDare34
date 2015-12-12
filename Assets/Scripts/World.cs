using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public enum Resources
    {
        Type1,
        Type2,
        Type3,
        Type4,
        Type5
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
        }

        for (int i = 0; i < numberOfPickups; i++)
        {
            float angle = (360 / numberOfPickups) * i;
            GameObject newPickup = (GameObject)Instantiate(obj, Vector3.zero, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle)));
            newPickup.transform.parent = transform;
            newPickup.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            newPickup.transform.localPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * (surfaceRadius + pickUpOffset), Mathf.Sin(angle * Mathf.Deg2Rad) * (surfaceRadius + pickUpOffset));
        }
    }

    public void Update()
    {
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
