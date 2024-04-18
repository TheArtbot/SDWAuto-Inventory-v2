using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserEntryScript : MonoBehaviour
{
    public UserSettingsUI UiHandler;

    public void ExecuteSelection()
    {
        UiHandler.SelectUserEntry(this.gameObject);
    }
}
