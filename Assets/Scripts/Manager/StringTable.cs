using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringTable : DataTable
{
    public class StringData
    {
        public int Id { get; set; }
        public string String { get; set; }
    }

    private Dictionary<int, string> stringDictionary = new Dictionary<int, string>();

    public override void Load(string fileName)
    {
        string path = string.Format(FormatPath, fileName);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<StringData>(textAsset.text);

        stringDictionary.Clear();
        foreach ( var item in list )
        {
            if (!stringDictionary.ContainsKey(item.Id))
            {
                stringDictionary.Add(item.Id, item.String);
            }
            else 
            {
                Debug.LogError($"Key \'{item.Id}\' exists already in String Table.");
            }
        }
    }

    public string Get(int key)
    {
        if(stringDictionary.ContainsKey(key))
        {
            return stringDictionary[key];
        }
        else
        {
            Debug.LogError($"Key \'{key}\' does not exist in String Table.");
            return "No key(StringTable)";
        }
    }
}
