using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveCarUI : UIHandler
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
    

    public CarController carCtrl;
    public LookUpUI LookUI;

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
        if(selectedInfo.isSold())
        {
            SoldText.text = "Sold";
            inHouseText.text = "N/A";
            if (selectedInfo.isLoaned()) { SoldText.text = "Loaned"; }
        }
        else { SoldText.text = "Unsold"; }
    }

    public void RemoveCar()
    {
        Car selectedInfo = LookUI.GetSelected();
        carCtrl.RemoveCar(selectedInfo.GetID());
    }
}
