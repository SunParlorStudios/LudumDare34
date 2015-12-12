using UnityEngine;
using System.Collections;

public class AlphaWave : MonoBehaviour
{
    public float pulseSpeed;

    [Range(0.0f, 1.0f)]
    public float pulseMinimum;

    [Range(0.0f, 1.0f)]
    public float pulseMaximum;

    private float offset;
    private Material mat;
    
	void Start ()
    {
        offset = Random.Range(0.0f, 1.0f);
        mat = GetComponent<SpriteRenderer>().material;
	}
	
	void Update ()
    {
        float sine = Mathf.Abs(Mathf.Sin(pulseSpeed * (Time.time + offset)));
        float range = pulseMaximum - pulseMinimum;
        float alpha = pulseMinimum + range * sine;
        mat.color = new Color(1.0f, 1.0f, 1.0f, alpha);
	}
}
