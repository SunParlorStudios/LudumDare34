using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public float angularSpeed;
    public Vector3 offset;
    private Material postProcessing;
    public float imageDistortionShift = 0.00125f;
    public float imageDistortionFrequency = 0.05f;

    public void Awake()
    {
        postProcessing = new Material(Shader.Find("Hidden/StaticShader"));
    }

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

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        postProcessing.SetFloat("_Shift", imageDistortionShift);
        postProcessing.SetFloat("_Frequency", imageDistortionFrequency);
        Graphics.Blit(source, destination, postProcessing);
    }
}
