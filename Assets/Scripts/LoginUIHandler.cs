using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIHandler : UIHandler
{
    public InputField password;

    public override void Activate()
    {
        if(password != null)
        {
            password.text = "";
        }
    }
}
