using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarDisplayScript : MonoBehaviour
{
    public Image image;
    public Text InfoText;
    public Text DescriptionText;

    public void Display(Car car)
    {
        InfoText.text = "Price: " + car.GetPrice() + "\n";
        InfoText.text += "ID: " + car.GetID() + "\n";
        InfoText.text += "Make: " + car.GetMake() + "\n";
        InfoText.text += "Model: " + car.GetModel() + "\n";
        InfoText.text += "Colour: " + car.GetColour() + "\n";
        InfoText.text += "VIN: " + car.GetVIN();

        DescriptionText.text = car.GetDescription();
    }
}
