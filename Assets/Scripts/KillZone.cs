using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour
{
	void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag != "Player")
        {
            return;
        }

        other.GetComponent<Player>().invincible = false;
        other.GetComponent<Player>().delivering = false;
        other.transform.FindChild("CannonShield").gameObject.SetActive(false);
        other.GetComponent<Player>().Kill();
    }
}
