using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesItemData
{
    public int Id { get; set; }
    public int SalesItemId { get; set; }
    public int MinSupply { get; set; }
    public int MaxSupply { get; set; }
    public int OnSaleProbability { get; set; }
}

public class SavedSalesItemData
{
    [JsonConverter(typeof(SalesItemDataConverter))]
    public SalesItemData SalesItemData;
    public int stock;
    public bool isOnSale;
}


public class SalesItemTable : DataTable
{
    private Dictionary<int, SalesItemData> salesItemDictionary = new Dictionary<int, SalesItemData>();
    public int SalesItemDictionaryCount { get { return salesItemDictionary.Count; } }

    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<SalesItemData>(textAsset.text);

        salesItemDictionary.Clear();
        foreach (var item in list)
        {
            if (!salesItemDictionary.ContainsKey(item.Id))
            {
                salesItemDictionary.Add(item.Id, item);
            }
            else
            {
                Debug.LogError($"Key \'{item.Id}\' exists already in Sales Item Table.");
            }
        }
    }

    public SalesItemData Get(int key)
    {
        if (!salesItemDictionary.ContainsKey(key))
        {
            Debug.LogError($"Key \'{key}\' does not exist in Sales Item Table.");
            return default(SalesItemData);
        }
        return salesItemDictionary[key];
    }
}
