using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalesHistoryUI : UIHandler
{
    public InputField searchBarText;
    public Button searchBarButton;
    public Button undoSaleButton;
    public GameObject content;
    public CarDisplayScript display;
    public InputField FromDate;
    public InputField ToDate;

    public SaleController saleCtrl;

    public GameObject saleEntryPrefab;

    private GameObject selectedSale;

    public override void WhileActive()
    {
        undoSaleButton.interactable = !(selectedSale == null);
    }

    private int DateValue(string Date)
    {
        string[] parts = Date.Split('/');

        int Day = 0;
        int Month = 0;
        int Year = 0;

        bool success = parts.Length == 3;
        success = success && (parts[0].Length == 2 || parts[0].Length == 1);
        success = success && (parts[1].Length == 2 || parts[1].Length == 1);
        success = success && parts[2].Length == 4;
        success = success && int.TryParse(parts[0], out Day);
        success = success && int.TryParse(parts[1], out Month);
        success = success && int.TryParse(parts[2], out Year);

        if (success) { return Day + (30 * Month) + (Year); } else { return 0; }
    }

    public void SearchFor()
    {
        // Read the searchBar entries
        string request = searchBarText.text.Replace(" ", "");
        // Request the cars from the car contorller
        (string, List<Dictionary<string, string>>) responce = saleCtrl.FindSales(request);
        print(responce.Item1);
        List<Dictionary<string, string>> SaleInfo = responce.Item2;
        // Load the carInfo into actuall UI elements.
        LoadSaleEntries(SaleInfo);
    }

    private void LoadSaleEntries(List<Dictionary<string, string>> SaleInfo)
    {
        // first i need to get rid of all the Children Curretnly linked to the content obj.
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }

        int fromDateValue = DateValue(FromDate.text);
        int toDateValue = DateValue(ToDate.text);

        // Then add all the cars from the returned list.
        for (int i = 0; i < SaleInfo.Count; i++)
        {
            // first check the date period: 
            int dateValue = DateValue(SaleInfo[i]["Day"] + "/" + SaleInfo[i]["Month"] + "/" + SaleInfo[i]["Year"]);
            // if (given a valid time period) and (failed to meet the time period) then don't include
            if ((fromDateValue != 0 && toDateValue != 0 && (fromDateValue <= toDateValue)) && (dateValue < fromDateValue && toDateValue > dateValue)) 
            { continue; }

            GameObject entry = Instantiate(saleEntryPrefab, content.transform);
            Sale sale = entry.GetComponent<Sale>();


            sale.SetSaleNo(SaleInfo[i]["SaleNo"]);
            sale.SetDate(SaleInfo[i]["Day"], SaleInfo[i]["Month"], SaleInfo[i]["Year"]);
            sale.SetCarID(SaleInfo[i]["CarId"]);

            // Setting the display.
            Text SaleNoText = entry.transform.GetChild(0).GetComponent<Text>();
            Text DateText = entry.transform.GetChild(1).GetComponent<Text>();
            Text CarIDText = entry.transform.GetChild(2).GetComponent<Text>();

            SaleNoText.text = sale.GetSaleNo();
            DateText.text = sale.GetDate().Item1 + "/" + sale.GetDate().Item2 + "/" + sale.GetDate().Item3;
            CarIDText.text = sale.GetCarID();


            SaleEntryScript entityScript = entry.GetComponent<SaleEntryScript>();
            entityScript.UiHandler = this;

            // Setting Up the Car component now
            List<Dictionary<string, string>> CarInfo = saleCtrl.carCtrl.RequestCars("Id=" + sale.GetCarID()).Item2;
            Car car = entry.GetComponent<Car>();

            car.SetID(CarInfo[0]["Id"]);
            car.SetMake(CarInfo[0]["Make"]);
            car.SetModel(CarInfo[0]["Model"]);
            car.SetVIN(CarInfo[0]["VIN"]);
            car.SetPrice(int.Parse(CarInfo[0]["Price"]));
            car.SetLoan(CarInfo[0]["onLoan"] == "true");
            car.SetLocationState(CarInfo[0]["inHouse"] == "True");
            car.SetSale(CarInfo[0]["Sold"] == "true");
            car.SetColour(CarInfo[0]["Colour"]);
            car.SetDescription(CarInfo[0]["Description"]);
            car.SetDate(CarInfo[0]["Day"], CarInfo[0]["Month"], CarInfo[0]["Year"]);
            car.SetClientAlias(CarInfo[0]["Client"]);
        }
    }

    public void SelectSaleEntry(GameObject saleEntry)
    {
        if (saleEntry.GetComponent<SaleEntryScript>() == null) { return; }
        selectedSale = saleEntry;
        display.Display(saleEntry.transform.GetComponent<Car>());
    }

    public Car GetSelectedCar()
    {
        return selectedSale.transform.GetComponent<Car>();
    }

    public Sale GetSelectedSale()
    {
        return selectedSale.transform.GetComponent<Sale>();
    }

    public void UndoSale()
    {
        Car selectedCar = GetSelectedCar();
        Sale selectedSale = GetSelectedSale();

        Dictionary<string, string> CarDetails = new Dictionary<string, string>
        {
            ["Id"] = selectedCar.GetID(),
            ["Day"] = selectedCar.GetDate().Item1,
            ["Month"] = selectedCar.GetDate().Item2,
            ["Year"] = selectedCar.GetDate().Item3,
            ["Client"] = selectedCar.GetClientAlias(),
            ["Make"] = selectedCar.GetMake(),
            ["Model"] = selectedCar.GetModel(),
            ["VIN"] = selectedCar.GetVIN(),
            ["Price"] = selectedCar.GetPrice().ToString(),
            ["Description"] = selectedCar.GetDescription(),
            ["Colour"] = selectedCar.GetColour(),
            ["inHouse"] = selectedCar.isHere().ToString(),
            ["onLoan"] = "False",
            ["Sold"] = "False"
        };

        saleCtrl.UndoSale(selectedSale.GetSaleNo(), selectedCar.GetID(), CarDetails);
        SearchFor();
    }
}
