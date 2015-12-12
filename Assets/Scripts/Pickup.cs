using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour
{
    public World.Resources type;

    public void Awake()
    {

    }

    public void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<Player>().inventory[type]++;

            Destroy(gameObject);
        }
    }
}
