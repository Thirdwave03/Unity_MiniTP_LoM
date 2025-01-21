using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum ItemTypes
{
    Primary,
    Secondary,
    Luxury,
}

public class ItemData
{
    // Independent variables
    public int Id { get; set; }
    public string ItemName { get; set; }
    public ItemTypes ItemType { get; set; }
    public string ImagefileName { get; set; }
    public int MaxCount { get; set; }
    public int InventoryOccupancy { get; set; }

    // Depending on game save data
    public int PriceNo { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public bool IsSoldOut { get; set; }

    public override string ToString()
    {
        return $"{ItemName} (No.{Id}, {ItemType})\n" +
            $"FilePath : Resources/Sprites/{ImagefileName}";

    }
}

public class ItemTable : DataTable
{
    private Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>();

    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<ItemData>(textAsset.text);

        itemDictionary.Clear();
        foreach( var item in list )
        {
            if(!itemDictionary.ContainsKey(item.Id))
            {
                itemDictionary.Add(item.Id, item);
            }
            else
            {
                Debug.LogError($"Key \'{item.Id}\' exists already in Item Table.");
            }
        }
    }

    public ItemData Get(int key)
    {
        if (!itemDictionary.ContainsKey(key))
        {
            Debug.LogError($"Key \'{key}\' does not exist in Item Table.");
            return default(ItemData);
        }
        return itemDictionary[key];
    }   
}
