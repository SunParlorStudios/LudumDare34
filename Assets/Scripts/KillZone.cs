using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour
{
	void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("WHAT");
        if (other.transform.tag != "Player")
        {
            return;
        }

        other.GetComponent<Player>().Kill();
    }
}
