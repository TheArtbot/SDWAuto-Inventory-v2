using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookUpUIHandler : MonoBehaviour
{

    public InputField searchBarText;
    public Button searchBarButton;
    public GameObject content;
    public CarDisplayScript display;

    public CarController carCtrl;

    public GameObject carEntryPrefab;

    private GameObject selectedCar;

    public void SearchFor()
    {
        // Read the searchBar entries
        string request = searchBarText.text;
        // Request the cars from the car contorller
        List<Dictionary<string, string>> CarInfo = carCtrl.RequestCars(request); 
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
            if (CarInfo[i]["Sold"] == "true") { continue; }

            GameObject entry = Instantiate(carEntryPrefab, content.transform);
            Car car = entry.GetComponent<Car>();


            car.SetID(CarInfo[i]["ID"]);
            car.SetMake(CarInfo[i]["Make"]);
            car.SetModel(CarInfo[i]["Model"]);
            car.SetVIN(CarInfo[i]["VIN"]);
            car.SetPrice(int.Parse(CarInfo[i]["Price"]));
            car.SetLoan(CarInfo[i]["onLoan"] == "true");
            car.SetLocationState(CarInfo[i]["inHouse"] == "true");
            car.SetColour(CarInfo[i]["Colour"]);
            car.SetDescription(CarInfo[i]["Description"]);

            // Setting the display.
            Text idText = entry.transform.GetChild(0).GetComponent<Text>();
            Text makeText = entry.transform.GetChild(1).GetComponent<Text>();
            Text modelText = entry.transform.GetChild(2).GetComponent<Text>();
            Text colourText = entry.transform.GetChild(3).GetComponent<Text>();

            idText.text = car.GetID();
            makeText.text = car.GetMake();
            modelText.text = car.GetModel();
            colourText.text = car.GetColour();

            CarEntityScript entityScript = entry.GetComponent<CarEntityScript>();
            entityScript.UiHandler = this;
        }
    }

    public void SelectCarEntry(GameObject carEntry)
    {
        print(carEntry);
        if(carEntry.GetComponent<CarEntityScript>() == null) { return; }
        selectedCar = carEntry;

        display.Display(carEntry.GetComponent<Car>());
    }

}
