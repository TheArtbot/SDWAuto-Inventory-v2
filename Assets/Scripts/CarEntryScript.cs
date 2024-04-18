using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarEntryScript : MonoBehaviour
{
    public LookUpUI UiHandler;

    public void ExecuteSelection()
    {
        UiHandler.SelectCarEntry(this.gameObject);
    }
}
