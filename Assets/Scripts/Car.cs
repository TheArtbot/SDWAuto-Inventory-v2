using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private string ID;
    private string Make;
    private string Model;
    private string VIN;
    private int Price;
    private bool Sold;
    private bool onLoan;
    private bool inHouse;
    private string Description;
    private string colour;

    public void SetID(string id) { ID = id; }
    public void SetMake(string Make) { this.Make = Make; }
    public void SetModel(string Model) { this.Model = Model; }
    public void SetVIN(string VIN) { this.VIN = VIN; }
    public void SetPrice(int Price) { this.Price = Price; }
    public void SetLoan(bool loan) { this.onLoan = loan; }
    public void SetLocationState(bool state) { this.inHouse = state; }
    public void SetDescription(string des) { this.Description = des; }
    public void SetColour(string colour) { this.colour = colour; }

    public string GetID() { return ID; }
    public string GetMake() { return Make; }
    public string GetModel() { return Model; }
    public string GetVIN() { return VIN; }
    public int GetPrice() { return Price; }
    public bool isLoaned() { return onLoan; }
    public bool isSold() { return Sold; }
    public bool isHere() { return inHouse; }
    public string GetDescription() { return Description; }
    public string GetColour() { return colour; }
}
