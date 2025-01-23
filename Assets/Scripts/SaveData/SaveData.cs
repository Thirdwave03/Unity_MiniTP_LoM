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
    public List<SavedItemData> savedItemList = new List<SavedItemData>();
    
    public GameModes currentGameMode;

    public int days;
    public int lastDay;

    public int coins;

    public int inventoryLevel;
    public int inventoryMinLevel;
    public int inventoryMaxLevel;

    public int inventoryCapacity;
    public int inventoryFee;

    public int lentAmount;
    public int paybackDateCnt;

    public int investedAmount;

    public int wholesaleItem1;
    public int wholesaleItem1Cnt;
    public int wholesaleItem1Cost;
    public bool isItem1Purchased;
    public int wholesaleItem2;
    public int wholesaleItem2Cnt;
    public int wholesaleItem2Cost;
    public bool isItem2Purchased;

    public bool isRandomBox1Purchased;
    public int randomBox1Item;
    public int randomBox1Cnt;
    public bool isRandomBox2Purchased;
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