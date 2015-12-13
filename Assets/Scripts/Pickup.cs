using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour
{
    public World.Resources type;
    public Player player;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < player.pickUpRadius)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, (1.0f - (Vector3.Distance(player.transform.position, transform.position) / player.pickUpRadius)) * player.pickUpStrength);
        }
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
