using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemTypes
{   
    // v1
    //Primary,
    //Secondary,
    //Luxury,

    // v2
    //Veges,
    //Fruits,
    //Foods,
    //Tools,
    //FineTools,
    //Books,
    //HighClassBooks,
    //Luxuries,

    // v3
    Veges,
    Fruits,
    Foods,
    Tools,
    Books,
    Luxuries,
}

public enum PriceTrends
{
    Raising,
    Descending,
    Stationary,
    Count,
}

public class ItemData
{
    // Independent variables
    public int Id {  get; set; }
    public string ItemName { get; set; }
    public ItemTypes ItemType { get; set; }
    public string ItemDescription {  get; set; }
    public string ImageFileName {  get; set; }
    public int MaxCount {  get; set; }
    public int InventoryOccupancy {  get; set; }
    public int EncounterProbability {  get; set; }
    public int MaxOnSaleCount { get; set; }

    public override string ToString()
    {
        return $"{ItemName} (No.{Id}, {ItemType})\n" +
            $"FilePath : Resources/Sprites/Icon/{ImageFileName}";
    }

    public Sprite IconSprite
    {
        get
        {
            return Resources.Load<Sprite>($"Sprites/Icon/{ImageFileName}");
        }
    }
}

public class SavedItemData
{
    [JsonConverter(typeof(ItemDataConverter))]
    public ItemData ItemData;
    // Depending on game save data
    public int priceID;
    public int count;
    public int price;
    public bool isSoldOut;
    public int avgCost;
    public PriceTrends priceTrend;
    public int trendRemainingDate;
}

public class ItemTable : DataTable
{
    private Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>();
    public int ItemDictionaryCount { get { return itemDictionary.Count; } }

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

    public Dictionary<int, ItemData> GetItemTable()
    {
        return itemDictionary.ToDictionary(k => k.Key, v => v.Value);
    }
}
