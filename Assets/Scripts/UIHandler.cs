using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHandler : MonoBehaviour
{
    public Player player;
    public GameController gameController;
    public BaseController baseController;

    private GameObject[] baseResources;
    private Text[] baseResourceTexts;

    private GameObject[] inventoryResources;
    private Text[] inventoryResourceTexts;

    private const int numResources = 6;

	void Awake()
    {
        int children = transform.childCount;

        baseResources = new GameObject[numResources];
        baseResourceTexts = new Text[numResources];

        inventoryResources = new GameObject[numResources];
        inventoryResourceTexts = new Text[numResources];

        GameObject child;
	    for (int i = 0; i < children; ++i)
        {
            child = transform.GetChild(i).gameObject;
            if (child.tag == "UIResource")
            {
                for (int j = 0; j < child.transform.childCount; ++j)
                {
                    if (child.name == "HomePlanetResources")
                    {
                        baseResources[j] = child.transform.GetChild(j).gameObject;
                        baseResourceTexts[j] = baseResources[j].transform.GetChild(1).GetComponent<Text>();
                    }
                    else
                    {
                        inventoryResources[j] = child.transform.GetChild(j).gameObject;
                        inventoryResourceTexts[j] = inventoryResources[j].transform.GetChild(0).GetComponent<Text>();
                    }
                }
            }
        }
	}

	void Update()
    {
        int val;
        int required;
        
        for (int i = 0; i < numResources; ++i)
        {
            val = baseController.resources[(World.Resources)i];
            required = GameController.GetValueFromRequirement(gameController.requirements[gameController.currentLevel], i);

            if (required == 0)
            {
                baseResources[i].transform.GetChild(0).gameObject.SetActive(true);
                baseResourceTexts[i].text = "";
                continue;
            }
            else
            {
                baseResources[i].transform.GetChild(0).gameObject.SetActive(false);
            }

            baseResourceTexts[i].color = val < required ? Color.red : Color.green;
            baseResourceTexts[i].text = val.ToString() + "/" + required.ToString();

            val = player.inventory[(World.Resources)i];

            inventoryResourceTexts[i].text = val.ToString();
        }
    }
}
