using System;
using System.Collections.Generic;
using UnityEngine;

public enum SceneIds
{
    TitleScene,
    MainScene,
    PurchaseScene,
    SalesScene,
    InnScene,
    TutorialScene,
}

public static class DataTableIds
{
    public static readonly string[] String =
    {
        "StringTableEn",
        "StringTableKr",
        "StringTableCn",
        "StringTableJp",
    };

    public static readonly string[] Item =
    {
        "ItemTable",
    };

    public static readonly string[] Price =
    {
        "PriceTable",
    };
}

public enum Languages
{
    English,
    Korean,
    Chinese,
    Japanese,
}

public enum GameModes // GameModes Table ±¸¼º
{
    Default,
    ShowMeTheMoney,
    Endless,
}

public static class ItemDataIndex
{
    public static int minPrimary = 10001;
    public static int maxPrimary = 10020;

    public static int minSecondary = 10021;
    public static int maxSecondary = 10040;

    public static int minLuxury = 10041;
    public static int maxLuxury = 10050;
}
public static class PriceDataIndex
{
    public static int minPrimary = 20001;
    public static int maxPrimary = 20050;

    public static int minSecondary = 20051;
    public static int maxSecondary = 20085;

    public static int minLuxury = 20086;
    public static int maxLuxury = 20100;
}
