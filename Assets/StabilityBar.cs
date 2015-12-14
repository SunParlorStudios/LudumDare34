using UnityEngine;
using System.Collections;

public class StabilityBar : MonoBehaviour
{
    public void Update()
    {
        transform.localScale = new Vector3(Mathf.Clamp(BaseController.instance.resourceTimer / BaseController.instance.resourceTimerMax, 0.0f, 1.0f), 1.0f, 1.0f);
    }
}
