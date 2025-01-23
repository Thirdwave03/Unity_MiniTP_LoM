using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using TMPro;
using Unity.VisualScripting;

public class ItemDataConverter : JsonConverter<ItemData>
{
    public override ItemData ReadJson(JsonReader reader, Type objectType, ItemData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var Id = Convert.ToInt32(reader.Value);
        Debug.Log(Id);
        return DataTableManager.ItemTable.Get(Id);
    }

    public override void WriteJson(JsonWriter writer, ItemData value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Id);
    }
}





public class JsonConverters
{
   

}
