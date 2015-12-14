using UnityEngine;
using System.Collections;

public class DangerIndicator : MonoBehaviour
{
    public Transform danger;

    public Camera camera;
    public Player player;

    public void Update()
    {
        if (danger == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 playerScreenPos = camera.WorldToScreenPoint(player.transform.position);

        Vector3 p1 = playerScreenPos;
        Vector3 p2 = camera.WorldToScreenPoint(danger.position);

        float a = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, a * Mathf.Rad2Deg + 90.0f);
        transform.position = new Vector2(playerScreenPos.x + Mathf.Cos(a) * 64.0f, playerScreenPos.y + Mathf.Sin(a) * 64.0f);
    }
}
