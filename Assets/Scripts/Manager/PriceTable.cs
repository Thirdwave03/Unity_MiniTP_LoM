using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PriceTypes
{
    PrimaryA,
    PrimaryB,
    SecondaryA,
    SecondaryB,
    LuxuryA,
    LuxuryB,
}

public class PriceData
{
    public int Id {  get; set; }
    public PriceTypes PriceType { get; set; }
    public int MaxPrice { get; set; }
    public int MinPrice { get; set; }
    public int MaxChangable {  get; set; }
    public int MinChangable { get; set; } // Could consider disposing min Changable val    
}

public class PriceTable : DataTable
{
    private Dictionary<int, PriceData> priceDictionary = new Dictionary<int, PriceData>();



    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<PriceData>(textAsset.text);

        priceDictionary.Clear();
        foreach( var item in list )
        {
            if(!priceDictionary.ContainsKey(item.Id))
            {
                priceDictionary.Add(item.Id, item);
            }
            else
            {
                Debug.LogError($"Key \'{item.Id}\' exists already in Price Table.");
            }
        }
    }

    public PriceData Get(int key)
    {
        if (!priceDictionary.ContainsKey(key))
        {
            Debug.LogError($"Key \'{key}\' does not exist in Price Table.");
            return default(PriceData);
        }
        else
        {
            return priceDictionary[key];           
        }
    }
}
