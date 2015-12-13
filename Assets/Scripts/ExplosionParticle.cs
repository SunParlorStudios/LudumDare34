using UnityEngine;
using System.Collections.Generic;

public class ExplosionParticle : MonoBehaviour
{
    private ParticleSystem explosionParticle;
    private ParticleSystem smokeParticle;

    private List<ParticleSystem> systems;
    private static GameObject prefab = null;

    public static void Create(Vector3 p)
    {
        if (prefab == null)
        {
            prefab = (GameObject)Resources.Load("ExplosionPrefab");
        }
        Instantiate(prefab, p, Quaternion.Euler(0.0f, 0.0f, 0.0f));
    }

	void Awake()
    {
        systems = new List<ParticleSystem>();
        explosionParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        smokeParticle = transform.GetChild(1).GetComponent<ParticleSystem>();

        Spawn(transform.position);
    }

    private void Add(ParticleSystem system, Vector3 point)
    {
        ParticleSystem explosion = Instantiate(system);
        explosion.transform.position = point;
        explosion.Play();

        systems.Add(explosion);
    }

    public void Spawn(Vector3 point)
    {
        Add(explosionParticle, point);
        Add(smokeParticle, point);
    }
	
	void Update()
    {
        bool done = true;
	    for (int i = systems.Count - 1; i >= 0; --i)
        {
            if (systems[i].IsAlive() == false)
            {
                Destroy(systems[i].gameObject);
                systems.RemoveAt(i);
            }
            else
            {
                done = false;
            }
        }

        if (done == true)
        {
            Destroy(gameObject);
        }
	}
}
