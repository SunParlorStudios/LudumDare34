using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIUpgrade : MonoBehaviour
{
    private GameObject checkMark;
    private GameObject cross;
    public GameObject selectionBox;

    bool hasUpgrade;

	void Awake()
    {
        checkMark = gameObject.transform.GetChild(0).gameObject;
        cross = gameObject.transform.GetChild(1).gameObject;

        hasUpgrade = false;
    }

    public void OnPointerEnter(BaseEventData evt)
    {
        selectionBox.SetActive(true);
        selectionBox.transform.position = transform.position;
    }

    public void OnPointerExit(BaseEventData evt)
    {
        selectionBox.SetActive(false);
    }

        void Update()
    {
        checkMark.SetActive(hasUpgrade);
        cross.SetActive(!hasUpgrade);
	}
}
