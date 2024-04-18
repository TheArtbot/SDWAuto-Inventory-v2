using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserSettingsUI : UIHandler
{
    public InputField searchBarText;
    public Button searchBarButton;
    public Button removeUserButton;
    public Button moteUserButton;
    public GameObject content;

    public UserController userCtrl;

    public GameObject userEntryPrefab;

    private GameObject selectedUser;

    public override void WhileActive()
    {
        moteUserButton.interactable = !(selectedUser == null);
        removeUserButton.interactable = !(selectedUser == null);
    }

    public void SearchFor()
    {
        // Read the searchBar entries
        string request = searchBarText.text.Replace(" ", "");
        // Request the cars from the car contorller
        (string, List<Dictionary<string, string>>) responce = userCtrl.RequestUsers(request);
        print(responce.Item1);
        List<Dictionary<string, string>> UserInfo = responce.Item2;
        // Load the carInfo into actuall UI elements.
        LoadUserEntries(UserInfo);
    }

    private void LoadUserEntries(List<Dictionary<string, string>> UserInfo)
    {
        // first i need to get rid of all the Children Curretnly linked to the content obj.
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }

        // Then add all the cars from the returned list.
        for (int i = 0; i < UserInfo.Count; i++)
        {
            GameObject entry = Instantiate(userEntryPrefab, content.transform);
            User user = entry.GetComponent<User>();

            user.SetUsername(UserInfo[i]["Username"]);
            user.SetPassword(UserInfo[i]["Password"]);
            user.SetManager(UserInfo[i]["Manager"] == "True");

            // Setting the display.
            Text UsernameText = entry.transform.GetChild(0).GetComponent<Text>();
            Text ManagerText = entry.transform.GetChild(1).GetComponent<Text>();

            UsernameText.text = user.GetUsername();
            if (user.isManager()) { ManagerText.text = "Manager"; } else { ManagerText.text = "Employee"; }

            UserEntryScript entityScript = entry.GetComponent<UserEntryScript>();
            entityScript.UiHandler = this;
        }
    }

    public void SelectUserEntry(GameObject userEntry)
    {
        if (userEntry.GetComponent<UserEntryScript>() == null) { return; }
        selectedUser = userEntry;
        if (userEntry.GetComponent<User>().isManager())
        {
            moteUserButton.transform.GetComponentInChildren<Text>().text = "Demote";
        }
        else
        {
            moteUserButton.transform.GetComponentInChildren<Text>().text = "Promote";
        }
    }

    public User GetSelected()
    {
        return selectedUser.transform.GetComponent<User>();
    }

    public void MoteButtonAction()
    {
        if (GetSelected().isManager())
        {
            print("demote");
            DemoteUser();
        }
        else
        {
            print("Promote");
            PromoteUser();
        }
        SearchFor();
    }

    public void DemoteUser()
    {
        User user = GetSelected();
        Dictionary<string, string> userInfo = new Dictionary<string, string>
        {
            ["Username"] = user.GetUsername(),
            ["Password"] = user.GetPassword(),
            ["Manager"] = "False"
        };

        userCtrl.EditUser(userInfo["Username"], userInfo);
    }

    public void PromoteUser()
    {
        User user = GetSelected();
        Dictionary<string, string> userInfo = new Dictionary<string, string>
        {
            ["Username"] = user.GetUsername(),
            ["Password"] = user.GetPassword(),
            ["Manager"] = "True"
        };

        userCtrl.EditUser(userInfo["Username"], userInfo);
    }
}
