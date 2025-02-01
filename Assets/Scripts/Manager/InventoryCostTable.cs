using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryData
{
    public int Lv { get; set; }
    public int Capacity { get; set; }
    public int DailyCost { get; set; }
    public int UpgradeCost { get; set; }
}

public class InventoryCostTable : DataTable
{
    private Dictionary<int, InventoryData> inventoryDictionary = new Dictionary<int, InventoryData>();
    public int SalesItemDictionaryCount { get { return inventoryDictionary.Count; } }

    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<InventoryData>(textAsset.text);

        inventoryDictionary.Clear();
        foreach (var item in list)
        {
            if (!inventoryDictionary.ContainsKey(item.Lv))
            {
                inventoryDictionary.Add(item.Lv, item);
            }
            else
            {
                Debug.LogError($"Key \'{item.Lv}\' exists already in Inventory Cost Table.");
            }
        }
    }

    public InventoryData Get(int key)
    {
        if (!inventoryDictionary.ContainsKey(key))
        {
            Debug.LogError($"Key \'{key}\' does not exist in Sales Item Table.");
            return default(InventoryData);
        }
        return inventoryDictionary[key];
    }

    public Dictionary<int, InventoryData> GetSalesItemTable()
    {
        return inventoryDictionary.ToDictionary(k => k.Key, v => v.Value);
    }
}
