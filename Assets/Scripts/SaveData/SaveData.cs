using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveData
{
    public int Version { get; protected set; }
    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    // Item List
    public List<SavedItemData> savedItemList = new List<SavedItemData>();
    // List of items on sale
    public List<SavedSalesItemData> savedSalesItemList = new List<SavedSalesItemData>();
    
    // GameMode and datas dependant to the GameMode
    public GameModes currentGameMode;
    public int lastDay;
    public int inventoryMinLevel;
    public int inventoryMaxLevel;

    // Game Core Data
    public int days;
    public int coins;

        // Availabilities
    public int tipIndex;
    public int infoItemIndex;
    public bool isDisplayingMinPriceInfo;
    public bool isInfoOpened;

        // Inventory
    public int inventoryLevel;
    public int inventoryCapacity;
    public int inventoryFee;

        // Loan
    public int lentAmount;
    public int paybackDateCnt;
    public int lentPaybackAmount;
    public bool ifLent;
    public bool isBusinessmanAvailable;

    // Inn
    public int investedAmount;
    public int innProfit;

    // Wholesale
    public bool isItem1Purchased;
    public bool isItem1PickedUp;
    public bool isItem1Pickupable;
    public int wholesaleItem1;
    public int wholesaleItem1Cnt;
    public int wholesaleItem1Cost;

    public bool isItem2Purchased;
    public bool isItem2PickedUp;
    public bool isItem2Pickupable;
    public int wholesaleItem2;
    public int wholesaleItem2Cnt;
    public int wholesaleItem2Cost;

    // RandomBox
    public bool isRandomBox1Purchased;
    public bool isRandomBox1PickedUp;
    public int randomBox1Item;
    public int randomBox1Cnt;

    public bool isRandomBox2Purchased;
    public bool isRandomBox2PickedUp;
    public int randomBox2Item;
    public int randomBox2Cnt;

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        throw new System.NotImplementedException();
    }
}