using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class StringTable : DataTable
{
    private Dictionary<string, string> stringDictionary = new Dictionary<string, string>();
    public class StringData
    {
        public string Id { get; set; }
        public string String { get; set; }
    }

    public override void Load(string fileName)
    {
        string path = string.Format(FormatPath, fileName);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<StringData>(textAsset.text);

        stringDictionary.Clear();
        foreach ( var item in list )
        {
            if (!stringDictionary.ContainsKey(item.String))
            {
                stringDictionary.Add(item.Id, item.String);
            }
            else 
            {
                Debug.LogError($"Key \'{item.Id}\' exists already in String Table.");
            }
        }
    }

    public string Get(string key)
    {
        if( stringDictionary.ContainsKey(key) )
        {
            return stringDictionary[key];
        }
        else
        {
            Debug.LogError($"Key \'{key}\' does not exist in String Table.");
            return "";
        }
    }
}
