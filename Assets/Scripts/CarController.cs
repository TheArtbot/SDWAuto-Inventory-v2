using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public DatabaseManager dbm;

    public List<Dictionary<string,string>> RequestCars(string searchReq)
    {
        string[] requests = searchReq.Split(';');
        // more could be add for sorting the order.
        return dbm.SearchTableSimilar("Cars", requests); 
    }

    public void AddCar(List<string> carinfo)
    {
        List<string> request = new List<string>();

        request.Add("ID:" + carinfo[0]);
        request.Add("Make:" + carinfo[1]);
        request.Add("Model:" + carinfo[2]);
        request.Add("VIN:" + carinfo[3]);
        request.Add("Price:" + carinfo[4]);
        request.Add("Sold:" + "false");
        request.Add("onLoan:" + "false");
        request.Add("inHouse:" + carinfo[5]);
        request.Add("Colour:" + carinfo[6]);
        request.Add("Description:" + carinfo[7]);

        dbm.InsertTable("Cars", request);
    }
}
