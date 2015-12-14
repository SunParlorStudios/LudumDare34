using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    private Player player;
    private Animator animator;

    public float force = 1.0f;

    public enum State
    {
        Idle,
        Rotating,
        Firing
    };

    private State state;

	public void Awake ()
    {
        player = null;
        animator = GetComponent<Animator>();
        state = State.Idle;
	}
	
	public void Update ()
    {
        switch (state)
        {
            default:
            case State.Idle:

                break;
            case State.Firing:
                if (player != null)
                {
                    player.Hide();
                    player.invincible = true;
                    player.transform.position = transform.position;
                }
                break;
            case State.Rotating:
                if (player != null)
                {
                    player.Hide();
                    player.invincible = true;
                    player.transform.position = transform.position;

                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        transform.parent.Rotate(0, 0, 90.0f * Time.deltaTime);
                    }
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        transform.parent.Rotate(0, 0, -90.0f * Time.deltaTime);
                    }
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        state = State.Firing;
                        animator.SetTrigger("shoot");
                    }
                }
                break;
        }
    }

    public void OnFire()
    {
        if (player != null)
        {
            float angle = (transform.rotation.eulerAngles.z + 5.0f) * Mathf.Deg2Rad;

            SoundController.instance.Play(6);

            ExplosionParticle.Create(transform.position);

            player.grounded = false;
            player.invincible = false;
            player.flyVelocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f);
            player.flySpeed = force * (GameController.instance.cannonBoosterUnlocked == true ? 3.0f : 1.0f);
            player.ignoreGravityTimer = 0.2f;
            player.Show();
            player.gameCamera.state = CameraController.State.InCannon;

            if (GameController.instance.cannonShieldUnlocked == true)
            {
                player.transform.FindChild("CannonShield").gameObject.SetActive(true);
                player.invincible = true;
            }
            player = null;
        }
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
            if (GameController.instance.cannonRotationUnlocked == true)
            {
                state = State.Rotating;
                player = collider.gameObject.GetComponent<Player>();
            }
            else
            {
                state = State.Firing;
                player = collider.gameObject.GetComponent<Player>();
                animator.SetTrigger("shoot");
            }
        }
    }
}
