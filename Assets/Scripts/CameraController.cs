using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public float angularSpeed;
    public Vector3 offset;
    private Material postProcessing;
    public float imageDistortionShift = 0.00125f;
    public float imageDistortionFrequency = 0.05f;
    public bool doBloom = true;
    public float bloom = 0.5f;
    public float bloomBlurDistance = 0.1f;
    public float bloomPasses = 1.0f;

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
        postProcessing.SetFloat("_Bloom", 1.0f - bloom);
        postProcessing.SetFloat("_BloomBlur", bloomBlurDistance);
        postProcessing.SetFloat("_BloomPasses", bloomPasses);
        postProcessing.SetFloat("_DoBloom", doBloom == true ? 1.0f : 0.0f);

        Graphics.Blit(source, destination, postProcessing);
    }
}
