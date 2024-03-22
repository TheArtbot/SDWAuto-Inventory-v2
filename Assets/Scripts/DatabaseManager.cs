using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    public string tablesPath;

    public List<Dictionary<string,string>> SearchTableSimilar(string table, string[] targets)
    {
        List<Dictionary<string, string>> similars = new List<Dictionary<string, string>>();

        string file_path = tablesPath + "\\" + table + ".txt";

        StreamReader sr = new StreamReader(file_path);
        string line = sr.ReadLine();
        while (line != null)
        {
            Dictionary<string, string> term = new Dictionary<string, string>();


            string[] segments = line.Split(',');
            foreach(string s in segments)
            {
                string[] feature = s.Split(':');
                term[feature[0]] = feature[1];
            }

            bool match = false;
            foreach (KeyValuePair<string, string> feature in term)
            {
                foreach(string t in targets)
                {
                    if (t == feature.Value || feature.Value.Contains(t))
                    {
                        similars.Add(term);
                        match = true;
                        break;
                    }
                }
                if (match) { break; }
            }

            line = sr.ReadLine();
        }

        sr.Close();
        return similars;
    }

    public void InsertTable(string table, List<string> entryInfo)
    {
        string file_path = tablesPath + "\\" + table + ".txt";

        // we need to combine the srting into the correct format.
        string entry = "";
        for(int i = 0; i < entryInfo.Count; i++)
        {
            entry += entryInfo[i];
            if (i < entryInfo.Count - 1) { entry += ","; }
        }
        entry += "";

        StreamWriter sw = new StreamWriter(file_path, true);
        sw.WriteLine(entry);
        sw.Close();
    }
}
