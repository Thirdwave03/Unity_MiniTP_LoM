using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData
{
    public UpgradeItems Upgrade {  get; set; }
    public bool IsOneTimeUpgrade { get; set; }
    public int MaxLv { get; set; }
    public int UpgradeCost {  get; set; }    
    public float Variance { get; set; }
}

public class UpgradeTable : DataTable
{
    private Dictionary<UpgradeItems, UpgradeData> upgradesDictionary = new Dictionary<UpgradeItems, UpgradeData>();
    public int UpgradeDictionaryCount { get { return upgradesDictionary.Count; } }

    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<UpgradeData>(textAsset.text);

        upgradesDictionary.Clear();
        foreach (var item in list)
        {
            if (!upgradesDictionary.ContainsKey(item.Upgrade))
            {
                upgradesDictionary.Add(item.Upgrade, item);
            }
            else
            {
                Debug.LogError($"Key \'{item.Upgrade}\' exists already in Sales Item Table.");
            }
        }
    }

    public UpgradeData Get(UpgradeItems key)
    {
        if (!upgradesDictionary.ContainsKey(key))
        {
            Debug.LogError($"Key \'{key}\' does not exist in Sales Item Table.");
            return default(UpgradeData);
        }
        return upgradesDictionary[key];
    }

    //public Dictionary<int, UpgradeData> GetUpgradeTable()
    //{
    //    return upgradesDictionary.ToDictionary(k => k.Key, v => v.Value);
    //}
}