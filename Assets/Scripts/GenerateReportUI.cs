using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GenerateReportUI : UIHandler
{
    public GameObject content;
    public Text reportContent;
    public SalesHistoryUI UIHandler;

    public string DropBoxPath;

    public override void Activate()
    {
        reportContent.text = "";
        reportContent.text = reportContent.text + "From: " + UIHandler.FromDate.text;
        while (reportContent.text.Length < 20) { reportContent.text += " "; }
        reportContent.text = reportContent.text + "To: " + UIHandler.ToDate.text + "\n";
        reportContent.text = reportContent.text + "Count: " + content.transform.childCount.ToString() + "\n\n";


        for(int i = 0; i < content.transform.childCount; i++)
        {
            string line = "";
            reportContent.text = reportContent.text + "--------------------------------------------\n";
            Car car = content.transform.GetChild(i).GetComponent<Car>();

            line = "Make: " + car.GetMake();
            while (line.Length < 40) { line += " "; }
            line += "Model: " + car.GetMake() + "\n";
            reportContent.text += line;

            line = "VIN: " + car.GetVIN();
            while (line.Length < 40) { line += " "; }
            line += "Model: " + car.GetPrice() + "\n";
            reportContent.text += line;
        }
    }

    public void GenerateReport()
    {
        DateTime time = DateTime.Now;
        /*print(time.ToString().Replace("/", "_").Replace(":", "-"));*/
        StreamWriter reportfile = new StreamWriter(DropBoxPath +"/Report_" + time.ToString().Replace("/", "_").Replace(":","-")+".txt");
        reportfile.Write(reportContent.text);
        reportfile.Close();
    }
}
