using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCarUIHandler : MonoBehaviour
{
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
        List<string> CarFacts = new List<string>();
        CarFacts.Add(IdInput.text);
        CarFacts.Add(MakeInput.text);
        CarFacts.Add(ModelInput.text);
        CarFacts.Add(VINInput.text);
        CarFacts.Add(PriceInput.text);
        CarFacts.Add(inHouse.isOn.ToString());
        CarFacts.Add(Colour.text);
        CarFacts.Add(Description.text);

        carCtrl.AddCar(CarFacts);
    }
}
