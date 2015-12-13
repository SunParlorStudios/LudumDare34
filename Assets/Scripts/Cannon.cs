﻿using UnityEngine;
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
            player.transform.position = transform.position;
        }
	}

    public void OnFire()
    {
        float angle = (transform.rotation.eulerAngles.z + 5.0f) * Mathf.Deg2Rad;

        ExplosionParticle.Create(transform.position);

        player.flyVelocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f);
        player.flySpeed = force;
        player.ignoreGravityTimer = 0.2f;
        player = null;
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
