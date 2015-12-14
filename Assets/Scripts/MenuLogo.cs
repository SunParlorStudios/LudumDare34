using UnityEngine;
using System.Collections;

public class MenuLogo : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        float r = Mathf.Sin(Time.time * 0.5f);
        transform.localPosition = new Vector3(0.0f, 200.0f + r * 20.0f, 0.0f);
        transform.localScale = new Vector3(1.0f + r * 0.1f, 1.0f + r * 0.1f, 0.0f);
    }
}
