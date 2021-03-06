﻿using UnityEngine;
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
        if (world != null)
        {
            if (followPlayer == true || Vector3.Distance(player.transform.position, transform.position) < player.GetPickupRadius() && GameController.instance.worldTypesUnlocked[(int)world.type] == true)
            {
                followPlayer = true;

                transform.position = Vector3.Lerp(
                    transform.position,
                    player.transform.position,
                    Mathf.Max((1.0f - (Vector3.Distance(player.transform.position, transform.position) / player.GetPickupRadius())) * player.pickUpStrength, 0.022f * Time.deltaTime * 100.0f)
                );
            }
        }
        else
        {
            if (followPlayer == true || Vector3.Distance(player.transform.position, transform.position) < player.GetPickupRadius())
            {
                followPlayer = true;

                transform.position = Vector3.Lerp(
                    transform.position,
                    player.transform.position,
                    Mathf.Max((1.0f - (Vector3.Distance(player.transform.position, transform.position) / player.GetPickupRadius())) * player.pickUpStrength, 0.022f * Time.deltaTime * 100.0f)
                );
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (world != null)
        {
            if (collider.gameObject.tag == "Player" && GameController.instance.worldTypesUnlocked[(int)world.type] == true)
            {
                collider.gameObject.GetComponent<Player>().inventory[type]++;

                if (GameController.instance.pickupDoubleUnlocked == true)
                {
                    collider.gameObject.GetComponent<Player>().inventory[type]++;
                }

                Destroy(gameObject);
                SoundController.instance.Play(Random.Range(2, 3), false);
            }
        }
        else
        {
            if (collider.gameObject.tag == "Player")
            {
                collider.gameObject.GetComponent<Player>().inventory[type]++;

                if (GameController.instance.pickupDoubleUnlocked == true)
                {
                    collider.gameObject.GetComponent<Player>().inventory[type]++;
                }

                Destroy(gameObject);
                SoundController.instance.Play(Random.Range(2, 3), false);
            }
        }
    }
}
