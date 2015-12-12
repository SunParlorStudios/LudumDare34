using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    public Transform target;

    public void Start()
    {

    }

    public void Update()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle * Mathf.Rad2Deg));
    }
}
