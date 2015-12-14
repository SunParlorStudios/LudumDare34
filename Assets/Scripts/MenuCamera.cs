using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour
{
    void Awake()
    {

    }

    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime);
    }
}
