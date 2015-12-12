using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [HideInInspector]public List<World> currentWorlds;

    private GameController gameController;

    public float deceleration = 0.1f;
    public float maxSpeed = 0.2f;
    public float maxJumpHeight = 2.0f;
    public float playerRadius;

    private float speed;
    private float jump;
    private bool grounded;

    private Vector3 normal;
    private Vector3 jumpVelocity;

    public void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        speed = 0.0f;
        jump = 0.0f;
        jumpVelocity = Vector3.zero;
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

        jumpVelocity = Vector3.Lerp(jumpVelocity, Vector3.zero, 0.1f);

        transform.position += jumpVelocity;

        float radius;

        for (int i = 0; i < currentWorlds.Count; i++)
        {
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
                    normal = (worldPosition - position).normalized;
                    grounded = true;
                    break;
                }
                else if (grounded == false)
                {
                    float angle = Mathf.Atan2(position.y - worldPosition.y, position.x - worldPosition.x) + speed * (1.0f / radius);
                    transform.position = worldPosition + new Vector3(Mathf.Cos(angle) * newDistance, Mathf.Sin(angle) * newDistance);
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