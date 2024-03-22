using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarEntityScript : MonoBehaviour
{
    public LookUpUIHandler UiHandler;

    public void ExicuteSelection()
    {
        UiHandler.SelectCarEntry(this.gameObject);
    }
}
