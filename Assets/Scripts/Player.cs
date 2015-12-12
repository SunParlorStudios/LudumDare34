using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [HideInInspector]public List<World> currentWorlds;

    private GameController gameController;

    public void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void FixedUpdate()
    {
        currentWorlds = gameController.FindInGravityRadius(transform.position);

        Vector3 worldPosition;
        Vector3 position;

        for (int i = 0; i < currentWorlds.Count; i++)
        {
            worldPosition = currentWorlds[i].transform.position;
            position = transform.position;

            Debug.DrawLine(position, worldPosition);

            float distance = Vector3.Distance(position, worldPosition);

            if (distance < currentWorlds[i].gravityRadius)
            {
                Vector3 newPosition = Vector3.MoveTowards(position, worldPosition, currentWorlds[i].gravityStrength * (1.0f - distance / currentWorlds[i].gravityRadius));
                float newDistance = Vector3.Distance(newPosition, worldPosition);

                if (newDistance < currentWorlds[i].surfaceRadius)
                {
                    float angle = Mathf.Atan2(position.y - worldPosition.y, position.x - worldPosition.x);
                    transform.position = worldPosition + new Vector3(Mathf.Cos(angle) * currentWorlds[i].surfaceRadius, Mathf.Sin(angle) * currentWorlds[i].surfaceRadius);
                }
                else
                {
                    transform.position = newPosition;
                }
            }
        }
    }
}