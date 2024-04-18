using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookUpUI : UIHandler
{

    public InputField searchBarText;
    public Button searchBarButton;
    public Button editCarButton;
    public Button removeCarButton;
    public Button UserSettingButton;
    public Button SaleHistoryButton;
    public GameObject content;
    public CarDisplayScript display;

    public CarController carCtrl;
    public UserAuthorizer UA;

    public GameObject carEntryPrefab;

    private GameObject selectedCar;

    public override void Activate()
    {
        UserSettingButton.interactable = UA.isManager();
        SaleHistoryButton.interactable = UA.isManager();
    }

    public override void WhileActive()
    {
        editCarButton.interactable = !(selectedCar == null);
        removeCarButton.interactable = !(selectedCar == null);
    }

    public void SearchFor()
    {
        // Read the searchBar entries
        string request = searchBarText.text.Replace(" ","");
        // Request the cars from the car contorller
        (string, List<Dictionary<string, string>>) responce = carCtrl.RequestCars(request);
        print(responce.Item1);
        List<Dictionary<string, string>> CarInfo = responce.Item2;
        // Load the carInfo into actuall UI elements.
        LoadCarEntries(CarInfo);
    }

    private void LoadCarEntries(List<Dictionary<string,string>> CarInfo)
    {
        // first i need to get rid of all the Children Curretnly linked to the content obj.
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }

        // Then add all the cars from the returned list.
        for (int i = 0; i < CarInfo.Count; i++)
        {
            if (CarInfo[i]["Sold"] == "True") { continue; }

            GameObject entry = Instantiate(carEntryPrefab, content.transform);
            Car car = entry.GetComponent<Car>();


            car.SetID(CarInfo[i]["Id"]);
            car.SetMake(CarInfo[i]["Make"]);
            car.SetModel(CarInfo[i]["Model"]);
            car.SetVIN(CarInfo[i]["VIN"]);
            car.SetPrice(int.Parse(CarInfo[i]["Price"]));
            car.SetLoan(CarInfo[i]["onLoan"] == "true");
            car.SetLocationState(CarInfo[i]["inHouse"] == "True");
            car.SetSale(CarInfo[i]["Sold"] == "true");
            car.SetColour(CarInfo[i]["Colour"]);
            car.SetDescription(CarInfo[i]["Description"]);
            car.SetDate(CarInfo[i]["Day"], CarInfo[i]["Month"], CarInfo[i]["Year"]);
            car.SetClientAlias(CarInfo[i]["Client"]);

            // Setting the display.
            Text idText = entry.transform.GetChild(0).GetComponent<Text>();
            Text makeText = entry.transform.GetChild(1).GetComponent<Text>();
            Text modelText = entry.transform.GetChild(2).GetComponent<Text>();
            Text colourText = entry.transform.GetChild(3).GetComponent<Text>();

            idText.text = car.GetID();
            makeText.text = car.GetMake();
            modelText.text = car.GetModel();
            colourText.text = car.GetColour();

            CarEntryScript entityScript = entry.GetComponent<CarEntryScript>();
            entityScript.UiHandler = this;
        }
    }

    public void SelectCarEntry(GameObject carEntry)
    {
        if(carEntry.GetComponent<CarEntryScript>() == null) { return; }
        selectedCar = carEntry;
        display.Display(carEntry.GetComponent<Car>());
    }

    public Car GetSelected()
    {
        return selectedCar.transform.GetComponent<Car>();
    }
}
