using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public Vector3 offset;
    
	public void Update ()
    {
	    if (target != null)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
            newPos.z = offset.z;
            transform.position = newPos;
        }
	}
}
