using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReturnButton : MonoBehaviour
{
    private Image image;
    private Sprite hover;

	void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        hover = Resources.Load<Sprite>("ReturnButtonHover");

        image.enabled = false;
    }

    public void OnEnter()
    {
        image.enabled = true;
        image.overrideSprite = null;
    }

    public void OnExit()
    {
        image.enabled = false;
        image.overrideSprite = null;
    }

    public void OnDown()
    {
        image.enabled = true;
        image.overrideSprite = hover;
    }

    public void OnUp()
    {
        image.enabled = true;
        image.overrideSprite = null;

        if (GameController.instance.player.delivering == true)
        {
            return;
        }

        UIHandler.instance.Hide();
    }
}
