using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour
{
    private Image button;
    private Image hover;

    void Awake()
    {
        button = transform.GetChild(0).GetComponent<Image>();
        hover = transform.GetChild(1).GetComponent<Image>();

        hover.enabled = false;
    }

    public void OnEnter()
    {
        button.enabled = false;
        hover.enabled = true;
        hover.color = Color.white;
    }

    public void OnExit()
    {
        button.enabled = true;
        hover.enabled = false;
        hover.color = Color.white;
    }

    public void OnDown()
    {
        button.enabled = false;
        hover.enabled = true;
        hover.color = Color.gray;
    }

    public void OnUp()
    {
        button.enabled = true;
        hover.enabled = false;
        hover.color = Color.white;

        if (name == "Start")
        {
            Application.LoadLevel(1);
        }
        else
        {
            Application.Quit();
        }
    }

    void Update()
    {

    }
}
