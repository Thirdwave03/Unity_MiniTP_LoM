using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();
    // there is very low probability to be reloaded

    static DataTableManager()
    {
        foreach (var id in DataTableIds.Item)
        {
            var table = new ItemTable();
            table.Load(id);
            tables.Add(id, table);
        }
        foreach (var id in DataTableIds.SalesItem)
        {
            var table = new SalesItemTable();
            table.Load(id);
            tables.Add(id, table);
        }
        foreach (var id in DataTableIds.Price)
        {
            var table = new PriceTable();
            table.Load(id);
            tables.Add(id, table);
        }

//#if UNITY_EDITOR
//        foreach (var id in DataTableIds.String)
//        {
//            var table = new StringTable();
//            table.Load(id);
//            tables.Add(id, table);
//        }
//#else
//        var table = new StringTable();
//        var stringTableId = DataTableIds.String[(int)Variables.currentLang];
//        table.Load(stringTableId);
//        tables.Add(stringTableId, table);
//#endif
    }
    //public static StringTable StringTable
    //{
    //    //get
    //    //{
    //    //   // return Get<StringTable>(DataTableIds.String[(int)Variables.currentLang]);
    //    //}
    //}

    public static ItemTable ItemTable
    {
        get
        {
            return Get<ItemTable>(DataTableIds.Item[0]);
        }
    }

    public static SalesItemTable SalesItemTable
    {
        get
        {
            return Get<SalesItemTable>(DataTableIds.SalesItem[0]);
        }
    }

    public static PriceTable PriceTable
    {
        get
        {
            return Get<PriceTable>(DataTableIds.Price[0]);
        }
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("No table");
            return null;
        }
        return tables[id] as T;
    }
}
