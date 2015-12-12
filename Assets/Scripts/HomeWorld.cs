using UnityEngine;
using System.Collections.Generic;

public class HomeWorld : MonoBehaviour
{
    public World baseWorld;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Player player = collider.gameObject.GetComponent<Player>();

            Dictionary<World.Resources, int> newDict = new Dictionary<World.Resources, int>();
            foreach(KeyValuePair<World.Resources, int> entry in player.inventory)
            {
                baseWorld.resources[entry.Key] += entry.Value;
                newDict[entry.Key] = 0;
            }
            player.inventory = newDict;
        }
    }
}
