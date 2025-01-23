using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using TMPro;

public class ItemDataConverter : JsonConverter<ItemData>
{
    public override ItemData ReadJson(JsonReader reader, Type objectType, ItemData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var IdStr = reader.Value as string;
        var Id = Convert.ToInt32(IdStr);
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
