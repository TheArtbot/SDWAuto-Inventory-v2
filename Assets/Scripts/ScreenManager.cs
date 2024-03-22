using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public GameObject currentScreen;

    public void SwitchScreens(GameObject Screen)
    {
        if(currentScreen != null) { currentScreen.SetActive(false); }
        currentScreen = Screen;
        currentScreen.SetActive(true);
    }

    public void CloseProgram()
    {
        Application.Quit();
    }
}
