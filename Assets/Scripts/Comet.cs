using UnityEngine;
using System.Collections;

public class Comet : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public float speed = 1.0f;
    public float timer = 0.0f;

    public void Awake()
    {
        timer = 0.0f;
    }

    public void Update()
    {
        timer += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(start, end, timer);

        if (timer >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}
