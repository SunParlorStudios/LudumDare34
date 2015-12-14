using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour
{
    public World.Resources type;
    public Player player;

    public World world;

    private bool followPlayer;

    public void Awake()
    {
        followPlayer = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void Update()
    {
        if (followPlayer == true || Vector3.Distance(player.transform.position, transform.position) < player.pickUpRadius && GameController.instance.worldTypesUnlocked[(int)world.type] == true)
        {
            followPlayer = true;

            transform.position = Vector3.Lerp(
                transform.position, 
                player.transform.position,
                Mathf.Max((1.0f - (Vector3.Distance(player.transform.position, transform.position) / player.pickUpRadius)) * player.pickUpStrength, 0.022f * Time.deltaTime * 100.0f)
            );
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && GameController.instance.worldTypesUnlocked[(int)world.type] == true)
        {
            collider.gameObject.GetComponent<Player>().inventory[type]++;
            Destroy(gameObject);
            SoundController.instance.Play(Random.Range(2, 3), false);
        }
    }
}
