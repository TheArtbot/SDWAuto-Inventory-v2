using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    //<Dev Note: Daniel Wright> Add a repsonce code if an error oucurs.
    
    public string tablesPath;
    public string tableMetaPath;

    /// <summary>
    /// Returns the value stores in the meta file for a give feature
    /// </summary>
    /// <param name="table"></param>
    /// <param name="feature"></param>
    /// <returns></returns>
    private string RetriveMetaInfo(string table, string feature)
    {
        string meta_path = tableMetaPath + "\\" + table + "Data.txt";
        string value = "";
        bool found = false;

        StreamReader meta_sr = new StreamReader(meta_path);
        string table_info = meta_sr.ReadLine();
        foreach (string segment in table_info.Split(','))
        {
            string[] pair = segment.Split(':');
            if (pair[0] == feature)
            {
                value = pair[1];
                found = true;
            }
            if (found) { break; }
        }
        meta_sr.Close();
        if (!found) { Debug.LogError("\"" + feature + "\" was not found in the \""+ table + "\" table."); }

        return value;
    }

    private (string,bool) InTable(string table, List<string> entryInfo)
    {
        string[] empty_strArray = { "" };
        string primaryKey = RetriveMetaInfo(table, "primaryKey");
        Dictionary<string, Dictionary<string, string>>[] Table = FindInTable(table, empty_strArray).Item2;

        foreach (string feature in entryInfo)
        {
            string[] parts = feature.Split(':');
            if(parts[0] == primaryKey) 
            {
                return ("200 OK",(Table[0].ContainsKey(parts[1]) || Table[1].ContainsKey(parts[1])));
            }   
        }
        return ("404 Error",false);
    }

    //<Dev Note: Daniel Wright> Add the cases where the database transaction can fail.
    /// <summary>
    /// This function takes a string as the name of a table and a Dictionary of all the terms int the table
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public (bool, Dictionary<string,Dictionary<string,string>>[]) FindInTable(string table, string[] search_terms)
    {
        string primaryKey = RetriveMetaInfo(table, "primaryKey");

        Dictionary<string, Dictionary<string, string>> similars = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> matches = new Dictionary<string, Dictionary<string, string>>();

        string file_path = tablesPath + "\\" + table + ".txt";

        StreamReader sr = new StreamReader(file_path);
        string line = sr.ReadLine();
        while (line != null)
        {
            bool match_found = false;
            Dictionary<string, string> entry = new Dictionary<string, string>();

            string[] segments = line.Split('~');
            foreach (string s in segments)
            {
                string[] feature = s.Split(':');
                string combined = feature[1];
                if (feature.Length > 2)
                {
                    for (int i = 2; i < feature.Length; i++) { combined = combined + ":" + feature[i]; }
                }
                entry[feature[0]] = combined;
            }

            foreach (string term in search_terms)
            {
                foreach (KeyValuePair<string, string> feature in entry)
                {
                    if (feature.Value.ToLower().Contains(term.ToLower()))
                    {
                        similars[entry[primaryKey]] = entry;
                        match_found = true;
                        break;
                    }

                    if (term.ToLower() == feature.Value.ToLower())
                    {
                        matches[entry[primaryKey]] = entry;
                        match_found = true;
                        break;
                    }
                }
                if (match_found) { break; }
            }

            line = sr.ReadLine();
        }
        sr.Close();

        Dictionary<string, Dictionary<string, string>>[] entries = { matches, similars };
        return (true,entries);
    }

    //<Dev Note: Daniel Wright> Add the cases where the database transaction can fail.
    public string InsertIntoTable(string table, List<string> entryInfo)
    {
        var (error_code, found) = InTable(table, entryInfo);
        if(error_code == "200 OK" && found) { return "405 Error: No duplicate allowed"; }

        string file_path = tablesPath + "\\" + table + ".txt";
        // we need to combine the srting into the correct format.
        string entry = "";
        for(int i = 0; i < entryInfo.Count; i++)
        {
            entry += entryInfo[i];
            if (i < entryInfo.Count - 1) { entry += "~"; }
        }

        StreamWriter sw = new StreamWriter(file_path, true);
        sw.WriteLine(entry);
        sw.Close();

        return "200 OK";
    }

    //<Dev Note: Daniel Wright> Add a check for the temp table at the start of the database manager.
    //                          This will also require the database knowing which tables it has.
    public void RemoveFromTable(string table, string unique_id)
    {
        string primaryKey = RetriveMetaInfo(table, "primaryKey");
        string file_path = tablesPath + "\\" + table + ".txt";

        // first we need to make the backup in case something goes wrong.
        string temp_path = tablesPath + "\\" + table + "Temp.txt";
        StreamReader sr = new StreamReader(file_path);
        StreamWriter tempw = new StreamWriter(temp_path);

        // while makeing the backup, we can store all the entries except the one we want to delete.
        List<string> saved_lines = new List<string>();

        string line = sr.ReadLine();
        while(line != null)
        {
            bool match_found = false;
            tempw.WriteLine(line);
            string[] features = line.Split('~');
            foreach(string f in features)
            {
                string[] segements = f.Split(':');
                /*print(segements[1] + ":" + unique_id);*/
                if(segements[0] == primaryKey && segements[1] == unique_id) { match_found = true; break;}
            }
            if (match_found) { line = sr.ReadLine(); continue; }

            saved_lines.Add(line);
            line = sr.ReadLine();
        }

        sr.Close();
        tempw.Close();

        StreamWriter sw = new StreamWriter(file_path);
        foreach(string l in saved_lines)
        {
            sw.WriteLine(l);
        }
        sw.Close();

        File.Delete(temp_path);
    }

    //<Dev Note: Daniel Wright> Add the cases where the database transaction can fail.
    public string UpdateInTable(string table, string unique_id, List<string> entryInfo)
    {
        // this function will be a merge between removing and inserting.
        // I did it this way instead of call both functions becuase i wanted to preserve the temporary back up
        // until i was finished with the edit.

        string primaryKey = RetriveMetaInfo(table, "primaryKey");
        string file_path = tablesPath + "\\" + table + ".txt";

        // first we need to make the backup in case something goes wrong.
        string temp_path = tablesPath + "\\" + table + "Temp.txt";
        StreamReader sr = new StreamReader(file_path);
        StreamWriter tempw = new StreamWriter(temp_path);

        // while makeing the backup, we can store all the entries except the one we want to delete.
        List<string> saved_lines = new List<string>();

        string line = sr.ReadLine();
        while (line != null)
        {
            tempw.WriteLine(line);
            bool match_found = false;
            string[] features = line.Split('~');

            foreach (string f in features)
            {
                string[] segements = f.Split(':');
                string key = segements[0];
                string value = segements[1];
                if (segements.Length > 2)
                {
                    for(int i = 2; i < segements.Length; i++) { value = value + segements[i]; }
                }
                if (key == primaryKey && value == unique_id) { match_found = true; break; }
            }

            if (!match_found) { saved_lines.Add(line); }
            line = sr.ReadLine();
        }
        sr.Close();
        tempw.Close();

        StreamWriter sw = new StreamWriter(file_path);
        foreach (string l in saved_lines)
        {
            sw.WriteLine(l);
        }
        string entry = "";
        for (int i = 0; i < entryInfo.Count; i++)
        {
            entry += entryInfo[i];
            if (i < entryInfo.Count - 1) { entry += "~"; }
        }
        sw.WriteLine(entry);
        sw.Close();

        File.Delete(temp_path);
        return "200 OK";
    }

    public string GetUniqueKey(string table, string inital)
    {
        string pKey = RetriveMetaInfo(table, "primaryKey");
        string newKey = inital;

        string file_path = tablesPath + "\\" + table + ".txt";
        StreamReader sr = new StreamReader(file_path);
        string line = sr.ReadLine();

        while(line != null)
        {
            string[] feature = line.Split('~');
            foreach(string f in feature)
            {
                string[] segements = f.Split(':');
                string key = segements[0];
            }
        }
        sr.Close();
        
        return newKey;
    }
}
