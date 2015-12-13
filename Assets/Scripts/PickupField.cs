using UnityEngine;
using System.Collections;

public class PickupField : MonoBehaviour
{
    public float widthInPickups;
    public float heightInPickups;
    public float widthBetweenPickups;
    public float heightBetweenPickups;

    public World.Resources typeOfResource;

    public void Awake()
    {
        Object obj;

        switch (typeOfResource)
        {
            default:
            case World.Resources.Type1:
                obj = UnityEngine.Resources.Load("PickUps/Pickup1");
                break;
            case World.Resources.Type2:
                obj = UnityEngine.Resources.Load("PickUps/Pickup2");
                break;
            case World.Resources.Type3:
                obj = UnityEngine.Resources.Load("PickUps/Pickup3");
                break;
            case World.Resources.Type4:
                obj = UnityEngine.Resources.Load("PickUps/Pickup4");
                break;
            case World.Resources.Type5:
                obj = UnityEngine.Resources.Load("PickUps/Pickup5");
                break;
        }

        for (int row = 0; row < heightInPickups; row++)
        { 
            for (int col = 0; col < widthInPickups; col++)
            {
                GameObject pickupObject = (GameObject)Instantiate(obj, Vector3.zero, Quaternion.Euler(Vector3.zero));
                pickupObject.transform.parent = transform;
                pickupObject.transform.localPosition = new Vector3(col * widthBetweenPickups - (widthInPickups * widthBetweenPickups * 0.5f), row * heightBetweenPickups - (heightInPickups * heightBetweenPickups * 0.5f), 0.0f);
            }
        }
    }
}
