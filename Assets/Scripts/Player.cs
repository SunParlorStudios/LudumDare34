using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [HideInInspector]public List<World> currentWorlds;

    private GameController gameController;

    public float deceleration = 0.1f;
    public float maxSpeed = 0.2f;
    public float flySpeed = 0.2f;
    public float flyMovement = 1.0f;
    public float angularSpeed = 0.1f;
    public float playerRadius;

    public float pickUpRadius = 3.0f;
    public float pickUpStrength = 2.0f;

    private float speed;
    public bool grounded;

    private Vector3 normal;
    public Vector3 flyVelocity;
    private World lastWorld;
    private GameObject homeWorld;

    private float wobbleTimer;
    private float defaultFlySpeed;

    public Dictionary<World.Resources, int> inventory;
    public float ignoreGravityTimer = 0.0f;

    private Vector3 defaultScale;
    private float defaultZ;

    public void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        speed = 0.0f;
        flyVelocity = Vector3.zero;
        lastWorld = null;

        inventory = new Dictionary<World.Resources, int>();
        inventory.Add(World.Resources.Type1, 0);
        inventory.Add(World.Resources.Type2, 0);
        inventory.Add(World.Resources.Type3, 0);
        inventory.Add(World.Resources.Type4, 0);
        inventory.Add(World.Resources.Type5, 0);
        inventory.Add(World.Resources.Type6, 0);

        defaultScale = transform.localScale;
        defaultZ = transform.position.z;
        defaultFlySpeed = flySpeed;

        homeWorld = GameObject.Find("WorldHome");
    }

    public void Kill()
    {
        ExplosionParticle.Create(transform.position);
        transform.position = homeWorld.transform.position;
        ResetInventory();
    }

    public void ResetInventory()
    {
        for (int i = 0; i < 6; ++i)
        {
            inventory[(World.Resources)i] = 0;
        }
    }

    public void Update()
    {
        ignoreGravityTimer -= Time.deltaTime;
        ignoreGravityTimer = Mathf.Max(0.0f, ignoreGravityTimer);
        Move();
    }

    public void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.G) == true)
        {
            Kill();
            return;
        }

        wobbleTimer += Time.deltaTime;

        if (wobbleTimer > Mathf.PI * 2.0f)
        {
            wobbleTimer = 0.0f;
        }


        float a = (transform.rotation.z + 90.0f) * Mathf.Deg2Rad;
        float r = Mathf.Sin(wobbleTimer) * 0.5f;
        Vector3 p;
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).name != "AsteroidFocusMin" && transform.GetChild(i).name != "AsteroidFocusMax")
            {
                p = transform.GetChild(i).transform.localPosition;
                transform.GetChild(i).transform.localPosition = new Vector3(Mathf.Cos(a) * r, Mathf.Sin(a) * r, p.z);
            }
        }

        if (grounded == false)
        {
            Vector3 delta = flyVelocity.normalized;
            float angle = Mathf.Atan2(delta.y, delta.x);

            float speed = 0.0f;

            if (Input.GetKey(KeyCode.LeftArrow) == true)
            {
                speed = flyMovement;
            }
            else if (Input.GetKey(KeyCode.RightArrow) == true)
            {
                speed = -flyMovement;
            }

            float ax = Mathf.Cos(angle + speed * Time.deltaTime) * flySpeed;
            float ay = Mathf.Sin(angle + speed * Time.deltaTime) * flySpeed;

            flyVelocity = new Vector3(ax, ay, 0.0f);

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

        if (Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            Jump();
        }
    }

    public void Jump()
    {
        flyVelocity = normal * -1.0f * flySpeed;
        grounded = false;
    }

    public void Move()
    {
        currentWorlds = gameController.FindInGravityRadius(transform.position);

        if (currentWorlds.Count == 0 || ignoreGravityTimer <= 0.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(flyVelocity.y, flyVelocity.x) * Mathf.Rad2Deg), angularSpeed * Time.deltaTime);
        }

        Vector3 worldPosition;
        Vector3 position;

        if (grounded == true)
        {
            speed = Mathf.Clamp(Mathf.Lerp(speed, 0.0f, deceleration * Time.deltaTime), -maxSpeed, maxSpeed);
            flyVelocity = Vector3.zero;
        }

        transform.position += flyVelocity * Time.deltaTime;
        transform.localScale = new Vector3(speed >= 0.0f ? -defaultScale.x : defaultScale.x, transform.localScale.y, transform.localScale.z);

        float radius;

        if (ignoreGravityTimer > 0.0f)
        {
            return;
        }

        for (int i = 0; i < currentWorlds.Count; i++)
        {
            if (grounded == false && lastWorld == currentWorlds[i])
            {
                continue;
            }

            worldPosition = currentWorlds[i].transform.position;
            position = transform.position;

            float distance = Vector3.Distance(position, worldPosition);

            if (distance < currentWorlds[i].gravityRadius)
            {
                Vector3 newPosition = Vector3.MoveTowards(position, worldPosition, currentWorlds[i].gravityStrength * 100.0f * Time.deltaTime * (1.0f - distance / currentWorlds[i].gravityRadius));
                float newDistance = Vector3.Distance(newPosition, worldPosition);

                radius = currentWorlds[i].surfaceRadius + playerRadius;
                if (newDistance < radius)
                {
                    float angle = Mathf.Atan2(position.y - worldPosition.y, position.x - worldPosition.x) + (speed * Time.deltaTime) * (1.0f / radius);
                    transform.position = worldPosition + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle * Mathf.Rad2Deg - 90.0f)), angularSpeed * Time.deltaTime);

                    normal = (worldPosition - position).normalized;
                    
                    if (gameController.worldTypesUnlocked[(int)currentWorlds[i].type] == false)
                    {
                        Jump();
                    }

                    if (lastWorld != currentWorlds[i])
                    {
                        grounded = true;
                        speed *= -1.0f;
                        lastWorld = currentWorlds[i];
                    }

                    flySpeed = defaultFlySpeed;
                }
                else if (grounded == false)
                {
                    float angle = Mathf.Atan2(position.y - worldPosition.y, position.x - worldPosition.x) + (speed * Time.deltaTime) * (1.0f / radius) * flyVelocity.magnitude * Time.deltaTime;
                    transform.position = worldPosition + new Vector3(Mathf.Cos(angle) * newDistance, Mathf.Sin(angle) * newDistance);

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle * Mathf.Rad2Deg - 90.0f)), angularSpeed * Time.deltaTime);
                }
            }

            Debug.DrawLine(position, worldPosition, grounded == true ? Color.green : Color.white);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, defaultZ);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRadius);

        Gizmos.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
}