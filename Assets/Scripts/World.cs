using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class World : MonoBehaviour
{
    public CircleCollider2D worldCollider;

    [Range(1.0f, 20.0f)]
    public float gravityRadius = 5.0f;

    [Range(1.0f, 20.0f)]
    public float surfaceRadius = 2.56f;

    [Range(0.1f, 20.0f)]
    public float gravityStrength = 0.5f;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);

        Gizmos.color = new Color(1.0f, 0.5f, 0.5f, 1.0f);
        Gizmos.DrawWireSphere(transform.position, surfaceRadius);
    }
}
