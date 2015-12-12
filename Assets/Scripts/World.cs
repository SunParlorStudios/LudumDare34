using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
public class World : MonoBehaviour
{
    public enum Resources
    {
        Type1,
        Type2,
        Type3
    }

    public enum RotationDirection
    {
        Left,
        Right
    }

    public CircleCollider2D worldCollider;

    [Range(1.0f, 20.0f)]
    public float gravityRadius = 5.0f;

    [Range(1.0f, 20.0f)]
    public float surfaceRadius = 2.56f;

    [Range(0.1f, 5.0f)]
    public float gravityStrength = 0.5f;

    public Dictionary<Resources, int> resources;

    public RotationDirection rotationDirection;
    public float rotationSpeed;

    public void Awake()
    {
        resources = new Dictionary<Resources, int>();
        resources.Add(Resources.Type1, 0);
        resources.Add(Resources.Type2, 0);
        resources.Add(Resources.Type3, 0);
    }

    public void Update()
    {
        transform.Rotate(new Vector3(0, 0, (rotationDirection == RotationDirection.Left ? -rotationSpeed : rotationSpeed) * Time.deltaTime));
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);

        Gizmos.color = new Color(1.0f, 0.5f, 0.5f, 1.0f);
        Gizmos.DrawWireSphere(transform.position, surfaceRadius);

        //transform.localScale = new Vector3((1.0f / 2.56f) * surfaceRadius, (1.0f / 2.56f) * surfaceRadius);
    }
}
