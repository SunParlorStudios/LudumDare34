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

            GameController gameController = baseController.gameController;

            foreach (KeyValuePair<World.Resources, int> entry in player.inventory)
            {
                baseController.resources[entry.Key] += entry.Value;

                if (entry.Value > 0)
                    delivered = true;
            }

            player.ResetInventory();

            if (OnDeliverResources != null && delivered)
            {
                OnDeliverResources();
            }
        }
    }
}
