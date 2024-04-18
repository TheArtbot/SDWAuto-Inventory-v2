using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public DatabaseManager dbm;

    public (string, List<Dictionary<string,string>>) RequestCars(string searchReq)
    {
        string[] requests = searchReq.Split(';');//get each search term
        var tempFilter = from r in requests where r.Contains('=') select r; // this will catch filters.
           
        // the user may want to specificaly search for some thing, in which case they can type "<feature> = <value>"
        // this is called a "filter" (or "specific filter" in the case a regualar search is refered as a filter) 
 
        // now we will check if the <feature> in filter on that actuall is in car."
        List<string> tempResuest = requests.ToList<string>();
        Dictionary<string, string> filters = new Dictionary<string, string>();
        List<string> features = new List<string>{ "id", "day", "month", "year", "client", "make", "model", "vin", "price", "description", "colour", "inhouse" };
        for(int i = 0; i < tempFilter.Count<string>(); i++)
        {
            string[] terms = tempFilter.ElementAt<string>(i).Split('=');
            if (features.Contains(terms[0].ToLower())) // if it is a filter, then get remove it from search terms
            {
                filters[terms[0]] = terms[1];
                if(terms.Length > 2)
                {
                    for(int j = 2; j < terms.Length; j++) { filters[terms[0]] = filters[terms[0]] + "=" + terms[j]; }
                    print(filters[terms[0]]);
                }
                tempResuest.Remove(tempFilter.ElementAt<string>(i)); 
                tempResuest.Add(filters[terms[0]]);
            }
        }
        requests = tempResuest.ToArray<string>();
        
        // now to retrive values from the database
        (bool, Dictionary<string, Dictionary<string, string>>[]) results = dbm.FindInTable("Cars", requests);
        if (!results.Item1) { return ("400 Error", new List<Dictionary<string, string>>()); }//<Dev Note: Daniel Wright> Add some kind of error responce to the UI if the databse fails.
        
        List<Dictionary<string, string>> complied_results = new List<Dictionary<string, string>>();
        for(int i = 0; i <= 1; i++)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> entry in results.Item2[i]) 
            {
                complied_results.Add(entry.Value);

                bool insideFilter = true;
                foreach(KeyValuePair<string,string> filter in filters)
                {
                    if (!entry.Value.ContainsKey(filter.Key)) { print("Invalid String"); continue; }
                    if(entry.Value[filter.Key].ToLower() != filter.Value.ToLower()) { insideFilter = false; }
                }
                if (!insideFilter) { complied_results.Remove(entry.Value); } 
            }
        }

        return ("200 OK",complied_results);
    }

    public string AddCar(Dictionary<string,string> carinfo)
    {
        List<string> request = new List<string>();
        foreach(KeyValuePair<string,string> carfact in carinfo)
        {
            request.Add(carfact.Key + ":" + carfact.Value);
        }
        
        string responce = dbm.InsertIntoTable("Cars", request);
        return responce;
    }

    public void RemoveCar(string carID)
    {
        dbm.RemoveFromTable("Cars", carID);
    }

    public string EditCar(string carID, Dictionary<string,string> carinfo)
    {
        List<string> request = new List<string>();
        foreach(KeyValuePair<string, string> carfact in carinfo)
        {
            request.Add(carfact.Key + ":" + carfact.Value);
        }

        string responce = dbm.UpdateInTable("Cars", carID ,request);
        return responce;
    }
}
