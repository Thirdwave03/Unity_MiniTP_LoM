using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using TMPro;

public class SalesItemDataConverter : JsonConverter<SalesItemData>
{
    public override SalesItemData ReadJson(JsonReader reader, Type objectType, SalesItemData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var Id = Convert.ToInt32(reader.Value);
        return DataTableManager.SalesItemTable.Get(Id);
    }

    public override void WriteJson(JsonWriter writer, SalesItemData value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Id);
    }
}

public class ItemDataConverter : JsonConverter<ItemData>
{
    public override ItemData ReadJson(JsonReader reader, Type objectType, ItemData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var Id = Convert.ToInt32(reader.Value);
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
