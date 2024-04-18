using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public GameObject currentScreen;

    public void Update()
    {
        UIHandler currentUI = currentScreen.GetComponent<UIHandler>();
        currentUI.WhileActive();
    }

    public void SwitchScreens(UIHandler Screen)
    {
        if(currentScreen != null) { currentScreen.SetActive(false); }
        currentScreen = Screen.gameObject;
        currentScreen.SetActive(true);
        Screen.Activate();
    }

    public void CloseProgram()
    {
        Application.Quit();
    }
}
