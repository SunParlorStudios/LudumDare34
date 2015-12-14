using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UIUpgrade : MonoBehaviour
{
    private GameObject checkMark;
    private GameObject cross;
    public GameObject selectionBox;
    public GameObject tooltip;
    public int requiredLevel;
    private Sprite tex;
    private Sprite hoverTex;

    [Range(0, 8)]
    public int upgradeID;

	void Awake()
    {
        checkMark = gameObject.transform.GetChild(0).gameObject;
        cross = gameObject.transform.GetChild(1).gameObject;
        tex = Resources.Load<Sprite>("UpgradeToolTip" + (upgradeID + 1).ToString());
        hoverTex = Resources.Load<Sprite>("UpgradeScreenIconClick");
    }

    public void OnPointerEnter(BaseEventData evt)
    {
        selectionBox.SetActive(true);
        selectionBox.transform.position = transform.position;

        tooltip.GetComponent<Image>().overrideSprite = tex;
        tooltip.GetComponent<Image>().enabled = true;
    }

    public void OnPointerExit(BaseEventData evt)
    {
        selectionBox.SetActive(false);
        tooltip.GetComponent<Image>().enabled = false;

        selectionBox.GetComponent<Image>().overrideSprite = null;
    }

    public void OnPointerDown(BaseEventData evt)
    {
        selectionBox.GetComponent<Image>().overrideSprite = hoverTex;
    }

    public void OnPointerUp(BaseEventData evt)
    {
        if (GameController.instance.player.HasUpgrade(upgradeID) == true || GameController.instance.currentLevel <= requiredLevel || GameController.instance.player.delivering == false)
        {
            selectionBox.GetComponent<Image>().overrideSprite = null;
            return;
        }

        selectionBox.GetComponent<Image>().overrideSprite = null;
        GameController.instance.player.AddUpgrade(upgradeID);
        UIHandler.instance.Hide();
    }

        void Update()
    {
        checkMark.SetActive(GameController.instance.player.HasUpgrade(upgradeID));
        cross.SetActive(GameController.instance.currentLevel <= requiredLevel);
	}
}
