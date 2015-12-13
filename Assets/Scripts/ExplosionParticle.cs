using UnityEngine;
using System.Collections.Generic;

public class ExplosionParticle : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    public ParticleSystem smokeParticle;

    private List<ParticleSystem> systems;

	void Awake()
    {
        systems = new List<ParticleSystem>();
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
	    for (int i = systems.Count - 1; i >= 0; --i)
        {
            if (systems[i].IsAlive() == false)
            {
                Destroy(systems[i].gameObject);
                systems.RemoveAt(i);
            }
        }
	}
}
