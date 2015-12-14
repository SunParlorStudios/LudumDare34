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
            ExplosionParticle.Create(transform.position);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Player player = collider.gameObject.GetComponent<Player>();

            if (player.dead == true || player.invincible == true)
            {
                return;
            }

            ExplosionParticle.Create(transform.position);
            player.Kill();
            Destroy(gameObject);
        }
    }
}
