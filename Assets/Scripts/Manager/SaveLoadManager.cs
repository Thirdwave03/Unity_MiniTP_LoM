using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SaveDataVC = SaveDataV1;


public class SaveLoadManager
{ 
    public static int SaveDataVersion { get; private set; } = 1;
    public static SaveDataVC Data {  get; set; }

    private static readonly string[] SaveFileName =
    {
        "LoM_Save1.json",
        "LoM_Save2.json",
        "LoM_Save3.json",
    };

    private static JsonSerializerSettings jsonSettings;

    static SaveLoadManager()
    {
        if(!Load())
        {
            Data = new SaveDataVC();
            Save();
        }
    }

    // readonly 넣어도 되나..?
    private static string SaveDirectory = $"{Application.persistentDataPath}/Save";
    
    public static bool Save(int slot = 0)
    {
        if (Data == null || slot < 0 || slot >= SaveFileName.Length)
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
        var json = JsonConvert.SerializeObject(Data, jsonSettings);
        File.WriteAllText(path, json);

        Debug.Log($"File Save to slotIndex ({slot}) successful");
        return true;
    }

    public static bool Load(int slot = 0)
    {
        if (slot < 0 || slot >= SaveFileName.Length)
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
        Debug.Log($"Loaded JSON: {json}");
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, jsonSettings);

        while(saveData.Version < SaveDataVersion)
        {
            saveData = saveData.VersionUp();
        }
        Data = saveData as SaveDataVC;

        return true;
    }

    public static int GetAvailableSaveSlot()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
            return -1;
        }
        if (!File.Exists(Path.Combine(SaveDirectory, SaveFileName[0])))
        {
            return 0;
        }
        if (!File.Exists(Path.Combine(SaveDirectory, SaveFileName[1])))
        {
            return 1;
        }
        if (!File.Exists(Path.Combine(SaveDirectory, SaveFileName[2])))
        {
            return 2;
        }
        return -1;
    }
}
