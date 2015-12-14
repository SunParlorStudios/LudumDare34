using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHandler : MonoBehaviour
{
    public Player player;
    public GameController gameController;
    public BaseController baseController;

    public GameObject dangerIndicatorPrefab;

    private GameObject[] baseResources;
    private Text[] baseResourceTexts;

    private GameObject[] inventoryResources;
    private Text[] inventoryResourceTexts;

    private GameObject homeIndicatorArrow;
    private GameObject upgradeWindow;

    public Camera camera;

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
            else if (child.name == "HomeWorldIndicator")
            {
                homeIndicatorArrow = child;
            }
            else if (child.name == "UpgradeCanvas")
            {
                upgradeWindow = child.transform.GetChild(0).gameObject;
            }
        }

        gameController.OnCometCreated += OnCometCreated;
        upgradeWindow.SetActive(false);
	}

    private void OnCometCreated(Comet comet)
    {
        GameObject dangerIndicator = Instantiate(dangerIndicatorPrefab);
        dangerIndicator.transform.SetParent(transform, false);

        DangerIndicator indicator = dangerIndicator.GetComponent<DangerIndicator>();

        indicator.danger = comet.transform;
        indicator.danger.position = Vector3.Scale(indicator.danger.position, new Vector3(1.0f, 1.0f, 0.0f));
        indicator.player = player;
        indicator.camera = camera;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.O) == true)
        {
            upgradeWindow.SetActive(!upgradeWindow.activeSelf);
        }

        int val;
        int required;

        Vector3 playerScreenPos = camera.WorldToScreenPoint(player.transform.position);

        Vector3 p1 = playerScreenPos;
        Vector3 p2 = camera.WorldToScreenPoint(baseController.gameObject.transform.position);

        float a = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x);
        homeIndicatorArrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, a * Mathf.Rad2Deg + 90.0f);
        homeIndicatorArrow.transform.position = new Vector2(playerScreenPos.x + Mathf.Cos(a) * 64.0f, playerScreenPos.y + Mathf.Sin(a) * 64.0f);

        for (int i = 0; i < numResources; ++i)
        {
            val = player.inventory[(World.Resources)i];
            inventoryResourceTexts[i].text = val.ToString();

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
        }
    }
}
