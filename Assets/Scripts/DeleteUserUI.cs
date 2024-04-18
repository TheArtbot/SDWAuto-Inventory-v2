using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteUserUI : UIHandler
{
    public Text Username;
    public Text Rank;

    public UserController userCtrl;
    public UserSettingsUI usUI;

    public override void Activate()
    {
        User selectedInfo = usUI.GetSelected();

        Username.text = selectedInfo.GetUsername();
        if (selectedInfo.isManager()) { Rank.text = "Manager"; } else { Rank.text = "Employee"; } 
    }

    public void RemoveUser()
    {
        User selctedInfo = usUI.GetSelected();
        userCtrl.RemoveUser(selctedInfo.GetUsername());
    }
}
