using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public float angularSpeed;
    public Vector3 offset;
    
	public void Update ()
    {
	    if (target != null)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
            newPos.z = offset.z;
            transform.position = newPos;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0.0f, 0.0f, target.transform.localEulerAngles.z)), angularSpeed * Time.deltaTime);
        }
	}
}
