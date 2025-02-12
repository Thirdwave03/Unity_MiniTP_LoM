using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public static readonly string[] SalesItem =
    {
        "SalesItemTable",
    };

    public static readonly string[] InventoryCost =
    {
        "InventoryTable",
    };

    public static readonly string[] GameMode =
    {
        "GameModeTable",
    };

    public static readonly string[] Upgrade =
    {
        "UpgradeTable",
    };
}

public enum Languages
{
    English,
    Korean,
    Chinese,
    Japanese,
}

public static class Variables
{
    public static Languages currentLanguage = Languages.English;
}

public enum GameModes // GameModes Table ±¸¼º
{
    Default,
    ShortGame,
    Endless,
    Poverty,
    ShowMeTheMoney,
    ProdigalSon,
    ProdigalSons,
    BigInventory,
    SmallInventory,
    IsAnyoneThere,
    Count,
}

public enum UpgradeItems
{
    WholesalesPriceAdvantage,
    SpecialPriceAdvantage,
    EnhancedInnProfitRatio,
    LargerLoanableAmount,
    HigherPaybackInterest,
    ReducedInventoryFee,
    InitialCoinAdvantage,
    Count,
}

public enum MerchantRanks
{
    NoviceMerchant,
    PromisingMerchant,
    SeasonedMerchant,
    TradeMaestro,
    MerchantGod,
    Count,
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

public static class SalesItemDataIndex
{
    public static int minPrimary = 40001;
    public static int maxPrimary = 40020;

    public static int minSecondary = 40021;
    public static int maxSecondary = 40040;

    public static int minLuxury = 40041;
    public static int maxLuxury = 40050;
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

public static class GameInfos
{
    public static readonly int minInventoryLevel = 1;
    public static readonly int maxInventoryLevel = 11;

    public static readonly int minLentAmount = 5000;
    public static readonly int maxLentAmount = 20000;

    public static readonly int minPaybackDate = 5;
    public static readonly int maxPaybackDate = 8;

    public static readonly float minLentAmountMultiplier = 1.15f;
    public static readonly float maxLentAmountMultiplier = 1.3f;

    public static readonly int minTipsIndex = 998001;
    public static readonly int maxTipsIndex = 998011;

    public static readonly int priceInfoCost = 1000;

    public static readonly int minSaveSlot = 1;
    public static readonly int maxSaveSlot = 3;

    public static readonly int tutorialStringIdBegin = 999991;
    public static readonly int tutorialStringIdEnd = 999997;

    public static int GetBulletinInfoStringId(ItemTypes itemType, PriceTrends trend, bool random = true)
    {
        int stringIdTempSuffix = (((int)itemType) - 1) * 4 + 1;
        stringIdTempSuffix += 2 - (((int)trend) * 2);
        int bulletinBoardIdDefault = 950000;
        if (random)
        {
            stringIdTempSuffix += UnityEngine.Random.Range(0, 2);
            stringIdTempSuffix += bulletinBoardIdDefault;
            return stringIdTempSuffix;
        }
        else
        {
            stringIdTempSuffix += bulletinBoardIdDefault;
            return stringIdTempSuffix;
        }
    }

    public static int RequiredCountToReveal(ItemTypes itemType)
    {
        switch (itemType)
        {
            case ItemTypes.Default:
                return 9999;

            case ItemTypes.Veges:
                return 50;

            case ItemTypes.Fruits:
            case ItemTypes.Foods:
            case ItemTypes.Tools:
                return 30;

            case ItemTypes.Books:
                return 15;

            case ItemTypes.Luxuries:
                return 1;
        }
        return 9999;
    }

    public static int RequiredCoinToControl(ItemTypes itemType)
    {
        switch (itemType)
        {
            case ItemTypes.Default:
                return 0;
            case ItemTypes.Veges:
                return 5000;
            case ItemTypes.Fruits:
            case ItemTypes.Foods:
            case ItemTypes.Tools:
                return 15000;
            case ItemTypes.Books:
                return 30000;
            case ItemTypes.Luxuries:
                return 10000;
        }
        return 0;
    }
}
    public static class LocalizerContents
{
    public static void AddAction(TextLocalizer localizer, UnityAction action)
    {
        //localizer.customizedFormat.AddListener(action);
    }
}

public enum TitleSceneCenterMsgType
{
    BestRecord,
    DevInfo,
    SelectLoadSlot,
    SelectNewGameSlot,
    SelectOverwriteSlot,
    SelectDeleteSlot,
    InformDeleted,
    SelectGameMode,
    OpenUpgradeWindow,
    IfReallyUpgrade,
}

public enum MainMenuCenterMsgType
{
    InventoryLevelMax,
    InventoryLevelMin,
    InventoryUpgrade,
    InventoryDowngrade,
    InsufficientCoin,
    LackOfCapacity,
    CannotProceed,
    CanProceed,
    LastDay,
}

public enum PurchaseSceneCenterMsgType
{
    InsufficientCoin,
    LackOfCapacity,
}

public enum SalesSceneMsgType
{
    InsufficientCoin,
    LoanPickUp,
    LoanPickedUp,
    AlreadyLent,
}

public enum InnSceneMsgType
{
    InsufficientCoin,
    LackOfCapacity,
}

public enum BulletinBoardMsgType
{
    InsufficientCoin,
    LackOfItems,
}
