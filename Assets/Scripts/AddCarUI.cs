using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCarUI : UIHandler
{
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

    //This will be connected to a Button in the Unity Editor
    public void AddCar()
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

        carCtrl.AddCar(CarFacts);
    }
}
