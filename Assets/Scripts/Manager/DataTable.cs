using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public abstract class DataTable
{
    public static readonly string FormatPath = "Tables/{0}";

    public abstract void Load(string fileName);

    public static List<T> LoadCSV<T>(string csvFile)
    {
        using (var reader = new StringReader(csvFile))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csvReader.GetRecords<T>().ToList();
        }
    }
}
