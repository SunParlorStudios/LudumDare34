using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHandler : MonoBehaviour
{
    public Player player;
    private GameObject[] resources;
    private Text[] resourceTexts;
    private const int numResources = 6;

	void Awake()
    {
        int children = transform.childCount;
        int current = -1;

        resources = new GameObject[numResources];
        resourceTexts = new Text[numResources];

        GameObject child;
	    for (int i = 0; i < children; ++i)
        {
            child = transform.GetChild(i).gameObject;
            if (child.tag == "UIResource")
            {
                resources[++current] = child;
                resourceTexts[current] = child.transform.GetChild(1).GetComponent<Text>();
            }
        }
	}
	
	void Update()
    {
        int val;
        bool found;
        
        for (int i = 0; i < numResources; ++i)
        {
            found = player.inventory.TryGetValue((World.Resources)i, out val);

            if (found == true)
            {
                resourceTexts[i].text = val.ToString() + "/8";
            }
        }
    }
}
