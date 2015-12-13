using UnityEngine;
using System.Collections.Generic;

public class ResourceDeliverer : MonoBehaviour
{
    public BaseController baseController;

    public delegate void OnDeliverResourcesDelegate();
    public event OnDeliverResourcesDelegate OnDeliverResources;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Player player = collider.gameObject.GetComponent<Player>();

            bool delivered = false;

            Dictionary<World.Resources, int> newDict = new Dictionary<World.Resources, int>();
            foreach (KeyValuePair<World.Resources, int> entry in player.inventory)
            {
                baseController.resources[entry.Key] += entry.Value;
                newDict[entry.Key] = 0;

                if (entry.Value > 0)
                    delivered = true;
            }
            player.inventory = newDict;

            if (OnDeliverResources != null && delivered)
            {
                OnDeliverResources();


                Debug.Log("hey faggits ayy " + Time.time);
            }
        }
    }
}
