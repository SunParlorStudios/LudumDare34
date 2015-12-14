using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    private Player player;
    private Animator animator;

    public float force = 1.0f;

	public void Awake ()
    {
        player = null;
        animator = GetComponent<Animator>();
	}
	
	public void Update ()
    {
        if (player != null)
        {
            player.Hide();
            player.dead = true;
            player.transform.position = transform.position;
        }
	}

    public void OnFire()
    {
        float angle = (transform.rotation.eulerAngles.z + 5.0f) * Mathf.Deg2Rad;

        ExplosionParticle.Create(transform.position);

        player.grounded = false;
        player.dead = false;
        player.flyVelocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f);
        player.flySpeed = force;
        player.ignoreGravityTimer = 0.2f;
        player.Show();
        player.gameCamera.state = CameraController.State.InCannon;
        player = null;
    }

    public void OnDrawGizmos()
    {
        float angle = (transform.rotation.eulerAngles.z + 5.0f) * Mathf.Deg2Rad;

        Gizmos.DrawRay(transform.position, new Vector3(Mathf.Cos(angle) * 500.0f, Mathf.Sin(angle) * 500.0f, 0.0f));
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            player = collider.gameObject.GetComponent<Player>();
            animator.SetTrigger("shoot");
        }
    }
}
