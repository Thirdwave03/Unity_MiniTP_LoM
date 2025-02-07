using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameModeData
{
    public GameModes GameMode { get; set; }
    public int StringId { get; set; }
    public int LastDay { get; set; }
    public int InventoryMaxLv { get; set; }
    public int InventoryMinLv { get; set; }
    public int InventoryInitialLv { get; set; }
    public int InitialCoin {  get; set; }
    public int DiamondGoal { get; set; }
    public float DiamondPaybackRate { get; set; }
}

public class GameModeTable : DataTable
{
    private Dictionary<GameModes, GameModeData> gameModesDictionary = new Dictionary<GameModes, GameModeData>();
    public int SalesItemDictionaryCount { get { return gameModesDictionary.Count; } }

    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<GameModeData>(textAsset.text);

        gameModesDictionary.Clear();
        foreach (var item in list)
        {
            if (!gameModesDictionary.ContainsKey(item.GameMode))
            {
                gameModesDictionary.Add(item.GameMode, item);
            }
            else
            {
                Debug.LogError($"Key \'{item.GameMode}\' exists already in Sales Item Table.");
            }
        }
    }

    public GameModeData Get(GameModes key)
    {
        if (!gameModesDictionary.ContainsKey(key))
        {
            Debug.LogError($"Key \'{key}\' does not exist in Sales Item Table.");
            return default(GameModeData);
        }
        return gameModesDictionary[key];
    }

    public Dictionary<GameModes, GameModeData> GetGameModeTable()
    {
        return gameModesDictionary.ToDictionary(k => k.Key, v => v.Value);
    }
}
