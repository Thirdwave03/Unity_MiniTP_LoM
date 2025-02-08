using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using SaveDataVC = SaveDataV1;
using BaseSaveDataVC = BaseSaveDataV1;
using System.Collections.Generic;


public class SaveLoadManager
{ 
    public static int SaveDataVersion { get; private set; } = 1;
    public static SaveDataVC GameData {  get; set; }
    public static BaseSaveDataVC BaseData {  get; set; }

    private static readonly string[] SaveFileName =
    {
        "LoM_Save_Base.json",
        "LoM_Save1.json",
        "LoM_Save2.json",
        "LoM_Save3.json",
    };

    private static JsonSerializerSettings jsonSettings;

    static SaveLoadManager()
    {
        //if(!Load())
        //{
        //    GameData = new SaveDataVC();
        //    Save();
        //}
        if(!LoadBase())
        {
            BaseData = new BaseSaveDataVC();
            BaseData.gameModes = new GameModes[3];
            BaseData.coins = new int[3];
            BaseData.days = new int[3];
            BaseData.dateTimes = new System.DateTime[3];
            BaseData.bgmVolume = 0.2f;
            BaseData.sfxVolume = 0.2f;
            BaseData.bestScore = new int[(int)GameModes.Count];
            BaseData.diamonds = 0;
            SaveBase();
        }
    }

    public void Init()
    {

    }

    // readonly 넣어도 되나..?
    private static string SaveDirectory = $"{Application.persistentDataPath}/Save";
    
    public static bool Save(int slot)
    {
        if (GameData == null || slot < 1 || slot > SaveFileName.Length)
        {
            Debug.Log($"File Save to slotIndex ({slot}) failed");
            return false;
        }

        if(!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
        };

        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        var json = JsonConvert.SerializeObject(GameData, jsonSettings);
        File.WriteAllText(path, json);

        Debug.Log($"File Save to slotIndex ({slot}) successful");

        return true;
    }

    public static bool SaveBase()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
        jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
        };

        var path = Path.Combine(SaveDirectory, SaveFileName[0]);
        var json = JsonConvert.SerializeObject(BaseData, jsonSettings);
        File.WriteAllText(path, json);

        Debug.Log($"Base file data save successful");

        return true;
    }

    public static bool Load(int slot)
    {
        if (slot < 1 || slot > SaveFileName.Length)
            return false;

        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        if (!File.Exists(path))
            return false;

        jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
        };

        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, jsonSettings);

        while(saveData.Version < SaveDataVersion)
        {
            saveData = saveData.VersionUp();
        }
        GameData = saveData as SaveDataVC;

        return true;
    }

    public static bool LoadBase()
    {
        var path = Path.Combine(SaveDirectory, SaveFileName[0]);
        if (!File.Exists(path))
            return false;

        jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
        };

        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<BaseSaveData>(json, jsonSettings);

        while (saveData.Version < SaveDataVersion)
        {
            saveData = saveData.VersionUp();
        }
        BaseData = saveData as BaseSaveDataVC;

        return true;
    }

    public static bool DeleteSlot(int slotIndex)
    {
        var path = Path.Combine(SaveDirectory, SaveFileName[slotIndex]);
        if(!File.Exists(path))
        {
            Debug.Log($"No file exists at path: {path}");
            return false;
        }
        else
        {
            File.Delete(path);
            Debug.Log($"File delete successful at path: {path}");
        }

        BaseData.days[slotIndex - 1] = default;
        BaseData.coins[slotIndex - 1] = default;
        BaseData.dateTimes[slotIndex - 1] = default;
        BaseData.gameModes[slotIndex - 1] = default;
        SaveBase();        
        return true;
    }

    public static int GetAvailableSaveSlot()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
            return -1;
        }
        if (!File.Exists(Path.Combine(SaveDirectory, SaveFileName[1])))
        {
            return 0;
        }
        if (!File.Exists(Path.Combine(SaveDirectory, SaveFileName[2])))
        {
            return 1;
        }
        if (!File.Exists(Path.Combine(SaveDirectory, SaveFileName[3])))
        {
            return 2;
        }
        return -1;
    }

    public static void AvoidNull()
    {
        if(GameData == null)
        {
            GameData = new SaveDataVC();
        }
        if(GameData.savedItemList == null)
        {
            GameData.savedItemList = new List<SavedItemData>();
        }
        if(GameData.savedSalesItemList == null)
        {
            GameData.savedSalesItemList = new List<SavedSalesItemData>();
        }
        if (GameData.notOnSaleItemsIds == null)
        {
            GameData.notOnSaleItemsIds = new List<int>();
        }
        if (GameData.specialPriceItemIndexes == null)
        {
            GameData.specialPriceItemIndexes = new List<int>();
        }
        if (GameData.bulletinBoardContentsId == null)
        {
            GameData.bulletinBoardContentsId = new List<int>();
        }
    }
}
