using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sale : MonoBehaviour
{
    private string Day;
    private string Month;
    private string Year;
    private string SaleNo;
    private string CarID;

    public void SetDate(string day, string month, string year) { Day = day; Month = month; Year = year; }
    public void SetSaleNo(string saleNo) { SaleNo = saleNo; }
    public void SetCarID(string carId) { CarID = carId; }

    public (string, string, string) GetDate() { return (Day, Month, Year); }
    public string GetSaleNo() { return SaleNo; }
    public string GetCarID() { return CarID; }
}
