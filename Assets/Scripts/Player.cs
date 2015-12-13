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
    private bool grounded;

    private Vector3 normal;
    private Vector3 flyVelocity;
    private World lastWorld;
    private GameObject homeWorld;

    private float wobbleTimer;

    public Dictionary<World.Resources, int> inventory;

    private ExplosionParticle explosion;

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

        defaultScale = transform.localScale;
        defaultZ = transform.position.z;

        homeWorld = GameObject.Find("WorldHome");
        explosion = GetComponent<ExplosionParticle>();
    }

    public void Kill()
    {
        explosion.Spawn(transform.position);
        transform.position = homeWorld.transform.position;
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
            flyVelocity = normal * -1.0f * flySpeed;
            grounded = false;
        }
    }

    public void FixedUpdate()
    {
        currentWorlds = gameController.FindInGravityRadius(transform.position);

        if (currentWorlds.Count == 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(flyVelocity.y, flyVelocity.x) * Mathf.Rad2Deg), angularSpeed);
        }

        Vector3 worldPosition;
        Vector3 position;
        
        if (grounded == true)
        {
            speed = Mathf.Clamp(Mathf.Lerp(speed, 0.0f, deceleration), -maxSpeed, maxSpeed);
            flyVelocity = Vector3.zero;
        }

        transform.position += flyVelocity;
        transform.localScale = new Vector3(speed >= 0.0f ? -defaultScale.x : defaultScale.x, transform.localScale.y, transform.localScale.z);

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
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle * Mathf.Rad2Deg - 90.0f)), angularSpeed);

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
                    float angle = Mathf.Atan2(position.y - worldPosition.y, position.x - worldPosition.x) + speed * (1.0f / radius) * flyVelocity.magnitude;
                    transform.position = worldPosition + new Vector3(Mathf.Cos(angle) * newDistance, Mathf.Sin(angle) * newDistance);

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle * Mathf.Rad2Deg - 90.0f)), angularSpeed);
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