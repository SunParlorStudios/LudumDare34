using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [HideInInspector]public List<World> currentWorlds;

    private GameController gameController;

    public float deceleration = 0.1f;
    public float maxSpeed = 0.2f;
    public float maxJumpHeight = 2.0f;
    public float jumpGravity = 0.1f;
    public float playerRadius;

    private float speed;
    private bool grounded;

    private Vector3 normal;
    private Vector3 jumpVelocity;
    private World lastWorld;

    public Dictionary<World.Resources, int> inventory;

    public void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        speed = 0.0f;
        jumpVelocity = Vector3.zero;
        lastWorld = null;

        inventory = new Dictionary<World.Resources, int>();
        inventory.Add(World.Resources.Type1, 0);
        inventory.Add(World.Resources.Type2, 0);
        inventory.Add(World.Resources.Type3, 0);
    }

    public void Update()
    {
        if (grounded == false)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow) == true)
        {
            speed = maxSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow) == true)
        {
            speed = -maxSpeed;
        }

        if (Input.GetKey(KeyCode.UpArrow) == true)
        {
            jumpVelocity = normal * -1.0f * maxJumpHeight;
            grounded = false;
        }
    }

    public void FixedUpdate()
    {
        currentWorlds = gameController.FindInGravityRadius(transform.position);

        Vector3 worldPosition;
        Vector3 position;
        
        if (grounded == true)
        {
            speed = Mathf.Clamp(Mathf.Lerp(speed, 0.0f, deceleration), -maxSpeed, maxSpeed);
        }

        jumpVelocity = Vector3.Lerp(jumpVelocity, Vector3.zero, jumpGravity);

        transform.position += jumpVelocity;

        float radius;

        for (int i = 0; i < currentWorlds.Count; i++)
        {
            if (currentWorlds[i] == lastWorld && grounded == false && lastWorld != null && currentWorlds.Count > 1)
            {
                continue;
            }

            worldPosition = currentWorlds[i].transform.position;
            position = transform.position;

            float distance = Vector3.Distance(position, worldPosition);

            if (distance < currentWorlds[i].gravityRadius)
            {
                Vector3 newPosition = Vector3.MoveTowards(position, worldPosition, currentWorlds[i].gravityStrength * (1.0f - distance / currentWorlds[i].gravityRadius));
                float newDistance = Vector3.Distance(newPosition, worldPosition);

                radius = currentWorlds[i].surfaceRadius + playerRadius;
                if (newDistance < radius)
                {
                    float angle = Mathf.Atan2(position.y - worldPosition.y, position.x - worldPosition.x) + speed * (1.0f / radius);
                    transform.position = worldPosition + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle * Mathf.Rad2Deg)), 0.1f);

                    normal = (worldPosition - position).normalized;

                    if (lastWorld != currentWorlds[i])
                    {
                        speed *= -1.0f;
                    }

                    lastWorld = currentWorlds[i];
                    grounded = true;
                }
                else if (grounded == false)
                {
                    float angle = Mathf.Atan2(position.y - worldPosition.y, position.x - worldPosition.x) + speed * (1.0f / radius) * jumpVelocity.magnitude;
                    transform.position = worldPosition + new Vector3(Mathf.Cos(angle) * newDistance, Mathf.Sin(angle) * newDistance);

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle * Mathf.Rad2Deg)), 0.1f);
                }
            }

            Debug.DrawLine(position, worldPosition, grounded == true ? Color.green : Color.white);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRadius);
    }
}