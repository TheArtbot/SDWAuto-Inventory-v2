using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditCarUI : UIHandler
{
    //TODO: makes is so that we can edit sale state

    public InputField DayInput;
    public InputField MonthInput;
    public InputField YearInput;
    public InputField ClientInput;
    public InputField IdInput;
    public InputField MakeInput;
    public InputField ModelInput;
    public InputField VINInput;
    public InputField PriceInput;
    public InputField Description;
    public InputField Colour;
    public Toggle inHouse;

    public CarController carCtrl;
    public LookUpUI LookUI;

    private string oldId;

    public override void Activate()
    {
        Car selectedInfo = LookUI.GetSelected();

        IdInput.text = selectedInfo.GetID();
        oldId = selectedInfo.GetID();
        (DayInput.text,MonthInput.text,YearInput.text) = selectedInfo.GetDate();
        ClientInput.text = selectedInfo.GetClientAlias();
        MakeInput.text = selectedInfo.GetMake();
        ModelInput.text = selectedInfo.GetModel();
        VINInput.text = selectedInfo.GetVIN();
        PriceInput.text = selectedInfo.GetPrice().ToString();
        Description.text = selectedInfo.GetDescription();
        Colour.text = selectedInfo.GetColour();
        inHouse.isOn = selectedInfo.isHere();
    }

    //This will be connected to a Button in the Unity Editor
    public void EditCar()
    {
        Dictionary<string, string> CarFacts = new Dictionary<string, string>
        {
            ["Id"] = IdInput.text,
            ["Day"] = DayInput.text,
            ["Month"] = MonthInput.text,
            ["Year"] = YearInput.text,
            ["Client"] = ClientInput.text,
            ["Make"] = MakeInput.text,
            ["Model"] = ModelInput.text,
            ["VIN"] = VINInput.text,
            ["Price"] = PriceInput.text,
            ["Description"] = Description.text,
            ["Colour"] = Colour.text,
            ["inHouse"] = (inHouse.isOn == true).ToString(),
            ["onLoan"] = "False",
            ["Sold"] = "False"
        };

        carCtrl.EditCar(oldId,CarFacts);
    }
}
