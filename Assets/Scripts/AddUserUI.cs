using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddUserUI : UIHandler
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Toggle isManager;

    public UserController userCtrl;

    public void AddUser()
    {
        Dictionary<string, string> UserFacts = new Dictionary<string, string>
        {
            ["Username"] = UsernameInput.text,
            ["Password"] = PasswordInput.text,
            ["Manager"] = (isManager.isOn).ToString()
        };

        string responce = userCtrl.AddUser(UserFacts);
    }
}
