using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeSaleUI : UIHandler
{
    public Text DateText;
    public Text ClientText;
    public Text IdText;
    public Text MakeText;
    public Text ModelText;
    public Text VINText;
    public Text PriceText;
    public Text Description;
    public Text ColourText;
    public Text inHouseText;
    public Text SoldText;


    public SaleController saleCtrl;
    public CarController carCtrl;
    public LookUpUI LookUI;

    public InputField DayText;
    public InputField MonthText;
    public InputField YearText;
    public InputField SaleNoText;
    public Toggle Loaned;

    public override void Activate()
    {
        Car selectedInfo = LookUI.GetSelected();

        IdText.text = selectedInfo.GetID();
        var (Day, Month, Year) = selectedInfo.GetDate();
        DateText.text = Day + "/" + Month + "/" + Year;
        ClientText.text = selectedInfo.GetClientAlias();
        MakeText.text = selectedInfo.GetMake();
        ModelText.text = selectedInfo.GetModel();
        VINText.text = selectedInfo.GetVIN();
        PriceText.text = selectedInfo.GetPrice().ToString();
        Description.text = selectedInfo.GetDescription();
        ColourText.text = selectedInfo.GetColour();
        if (selectedInfo.isHere()) { inHouseText.text = "On Site"; }
        else { inHouseText.text = "Away"; }
        if (selectedInfo.isSold())
        {
            SoldText.text = "Sold";
            inHouseText.text = "N/A";
            if (selectedInfo.isLoaned()) { SoldText.text = "Loaned"; }
        }
        else { SoldText.text = "Unsold"; }
    }

    public void SellCar()
    {
        Dictionary<string, string> SalesInfo = new Dictionary<string, string>
        {
            ["SaleNo"] = SaleNoText.text,
            ["Day"] = DayText.text,
            ["Month"] = MonthText.text,
            ["Year"] = YearText.text
        };

        Car selected = LookUI.GetSelected();
        Dictionary<string, string> CarInfo = new Dictionary<string, string>
        {
            ["Id"] = selected.GetID(),
            ["Day"] = selected.GetDate().Item1,
            ["Month"] = selected.GetDate().Item2,
            ["Year"] = selected.GetDate().Item3,
            ["Client"] = selected.GetClientAlias(),
            ["Make"] = selected.GetMake(),
            ["Model"] = selected.GetModel(),
            ["VIN"] = selected.GetVIN(),
            ["Price"] = selected.GetPrice().ToString(),
            ["Description"] = selected.GetDescription(),
            ["Colour"] = selected.GetColour(),
            ["inHouse"] = selected.isHere().ToString(),
            ["onLoan"] = (Loaned.isOn).ToString(),
            ["Sold"] = "True"
        };

        saleCtrl.MakeSale(selected.GetID(),SalesInfo,CarInfo);
    }
}
