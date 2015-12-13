﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public enum State
    {
        Default,
        FocusWorld
    }

    public Transform target;
    public float smoothing;
    public float angularSpeed;
    private Material postProcessing;
    public float imageDistortionShift = 0.00125f;
    public float imageDistortionFrequency = 0.05f;
    public bool doBloom = true;
    public float bloom = 0.5f;
    public float bloomBlurDistance = 0.1f;
    public float bloomPasses = 1.0f;

    public State state;
    public float offsetZ;

    public delegate void OnFocusedWorldDelegate();
    public event OnFocusedWorldDelegate onFocusedWorld;
    public bool onFocusedWorldCalled = false;

    private World worldBase;

    public void Awake()
    {
        postProcessing = new Material(Shader.Find("Hidden/StaticShader"));
        worldBase = GameObject.Find("WorldHome").GetComponent<World>();
        onFocusedWorldCalled = false;
    }

    public void Update()
    {
        Vector3 newPos;

        switch (state)
        {
            case State.Default:
                newPos = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
                newPos.z = Mathf.Lerp(transform.position.z, offsetZ, smoothing * Time.deltaTime);
                transform.position = newPos;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0.0f, 0.0f, target.transform.localEulerAngles.z)), angularSpeed * Time.deltaTime);
                onFocusedWorldCalled = false;
                break;
            case State.FocusWorld:
                newPos = Vector3.Lerp(transform.position, worldBase.transform.position, smoothing * Time.deltaTime);
                newPos.z = Mathf.Lerp(transform.position.z, offsetZ, Time.deltaTime);
                transform.position = newPos;

                if (Mathf.Abs(newPos.z) >= Mathf.Abs(offsetZ * 0.95f))
                {
                    if (!onFocusedWorldCalled)
                    {
                        onFocusedWorldCalled = true;
                        onFocusedWorld();
                    }
                }
                break;
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
