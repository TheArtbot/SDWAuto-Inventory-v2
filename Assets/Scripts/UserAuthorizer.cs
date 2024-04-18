using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserAuthorizer : MonoBehaviour
{
    public ScreenManager scrnm;
    public DatabaseManager dbm;

    public InputField UsernameInput;
    public InputField PasswordInput;

    public UIHandler MainScreen;
    public UIHandler LoginFailScreen;

    private string CurrentUsername;
    private bool CurrentUserManager;

    public void Login()
    {
        Dictionary<string,Dictionary<string,string>>[] respnce = dbm.FindInTable("Users", new string[] {""}).Item2;
        print(respnce[0].Count + respnce[1].Count);
        if(respnce[0].Count + respnce[1].Count <= 0) { LoginFail();return; }
        foreach(KeyValuePair<string, Dictionary<string, string>> users in respnce[0])
        {
            if(users.Value["Username"].ToLower() == UsernameInput.text.ToLower() && users.Value["Password"] == PasswordInput.text)
            {
                CurrentUserManager = users.Value["Manager"] == "True";
                CurrentUsername = users.Value["Username"];
                LoginSuccess();
                return;
            }
        }
        foreach (KeyValuePair<string, Dictionary<string, string>> users in respnce[1])
        {
            if (users.Value["Username"].ToLower() == UsernameInput.text.ToLower() && users.Value["Password"] == PasswordInput.text)
            {
                CurrentUserManager = users.Value["Manager"] == "True";
                CurrentUsername = users.Value["Username"];
                LoginSuccess();
                return;
            }
        }
        LoginFail();
        return;
    }

    private void LoginFail()
    {
        scrnm.SwitchScreens(LoginFailScreen);
    }

    private void LoginSuccess()
    {
        scrnm.SwitchScreens(MainScreen);
    }

    public bool isManager() { return CurrentUserManager; }
    public string GetCurrentUser() { return CurrentUsername; }
}
