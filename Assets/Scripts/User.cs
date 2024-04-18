using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    private string Username;
    private string Password;
    private bool Manager;

    public void SetUsername(string username) { Username = username; }
    public void SetPassword(string password) { Password = password; }
    public void SetManager(bool Manager) { this.Manager = Manager; }

    public string GetUsername() { return Username; }
    public string GetPassword() { return Password; }
    public bool isManager() { return Manager; }
}
