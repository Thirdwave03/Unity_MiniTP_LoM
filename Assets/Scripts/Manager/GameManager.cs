using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class GameManager
{    
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                InitialCall();
            }
            return instance;
        }
    }

    public Dictionary<int, SavedItemData> entireItemDict;
    private bool isItemInitializingNeeded = false;

    public GameModes CurrentGameMode { get; private set; }
    public int currentSavedSlotIndex;

    private int days;
    public int Days {  get { return days; } }
    public int lastDay;

    private int coins;
    public int Coins { get { return coins; } }
    public int inventoryLevel;
    public int inventoryMinLevel;
    public int inventoryMaxLevel;

    public int inventoryCapacity;
    public int InventoryCapacity { get { return inventoryCapacity; } }

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

    private static void InitialCall()
    {
        instance = new GameManager();
        instance.SetupEntireItemData();
    }

    private void SetupEntireItemData()
    {
        entireItemDict = new Dictionary<int, SavedItemData>();
        entireItemDict.Clear();
       
        for(int i = ItemDataIndex.minPrimary; i <= ItemDataIndex.maxLuxury; ++i)
        {
            var tempSavedData = new SavedItemData();
            tempSavedData.ItemData = DataTableManager.Get<ItemTable>(DataTableIds.Item[0]).Get(i);
            tempSavedData.priceID = -1;
            tempSavedData.price = -1;
            tempSavedData.priceTrend = PriceTrends.Stationary;
            tempSavedData.trendRemainingDate = 0;
            tempSavedData.avgCost = 0;
            tempSavedData.count = 0;

            entireItemDict.Add(tempSavedData.ItemData.Id, tempSavedData);                
        }
        isItemInitializingNeeded = true;
        
        if (isItemInitializingNeeded)
        {
            InitializeItemDictData();
        }
        SynchronizeWithSaveData();
    }

    private void InitializeItemDictData()
    {
        var priceTable = DataTableManager.PriceTable;
        var priceList = priceTable.GetPriceKeyList();

        for(int i = ItemDataIndex.minPrimary; i <= ItemDataIndex.maxPrimary; ++i)
        {
            int key = Random.Range(PriceDataIndex.minPrimary, PriceDataIndex.maxPrimary);
            while (!priceList.Contains(key))
            {
                key = Random.Range(PriceDataIndex.minPrimary, PriceDataIndex.maxPrimary);
            }

            entireItemDict[i].priceID = key;
            entireItemDict[i].price = Random.Range(DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MinPrice, DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MaxPrice + 1);
            entireItemDict[i].priceTrend = (PriceTrends)Random.Range(0,(int)PriceTrends.Count);
            if(entireItemDict[i].priceTrend != PriceTrends.Stationary)
            {
                entireItemDict[i].trendRemainingDate = Random.Range(0, 3);
            }
            priceList.Remove(key);
        }

        for (int i = ItemDataIndex.minSecondary; i <= ItemDataIndex.maxSecondary; ++i)
        {
            int key = Random.Range(PriceDataIndex.minSecondary, PriceDataIndex.maxSecondary);
            while (!priceList.Contains(key))
            {
                key = Random.Range(PriceDataIndex.minSecondary, PriceDataIndex.maxSecondary);
            }

            entireItemDict[i].priceID = key;
            entireItemDict[i].price = Random.Range(DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MinPrice, DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MaxPrice + 1);
            entireItemDict[i].priceTrend = (PriceTrends)Random.Range(0, (int)PriceTrends.Count);
            if (entireItemDict[i].priceTrend != PriceTrends.Stationary)
            {
                entireItemDict[i].trendRemainingDate = Random.Range(0, 3);
            }
            priceList.Remove(key);
        }

        for (int i = ItemDataIndex.minLuxury; i <= ItemDataIndex.maxLuxury; ++i)
        {
            int key = Random.Range(PriceDataIndex.minLuxury, PriceDataIndex.maxLuxury);
            while (!priceList.Contains(key))
            {
                key = Random.Range(PriceDataIndex.minLuxury, PriceDataIndex.maxLuxury);
            }

            entireItemDict[i].priceID = key;
            entireItemDict[i].price = Random.Range(DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MinPrice, DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MaxPrice + 1);
            entireItemDict[i].priceTrend = (PriceTrends)Random.Range(0, (int)PriceTrends.Count);
            if (entireItemDict[i].priceTrend != PriceTrends.Stationary)
            {
                entireItemDict[i].trendRemainingDate = Random.Range(0, 3);
            }
            priceList.Remove(key);
        }
    }

    private void SynchronizeWithSaveData()
    {
        SaveLoadManager.Data.savedItemList.Clear();
        foreach(var saveData in entireItemDict.Values.ToList())
        {
            SaveLoadManager.Data.savedItemList.Add(saveData);
        }

        SaveLoadManager.Data.currentGameMode = CurrentGameMode;
        SaveLoadManager.Data.days = days;
        SaveLoadManager.Data.lastDay = lastDay;
        SaveLoadManager.Data.coins = coins;
        SaveLoadManager.Data.inventoryLevel = inventoryLevel;
        SaveLoadManager.Data.inventoryMinLevel = inventoryMinLevel;
        SaveLoadManager.Data.inventoryMaxLevel = inventoryMaxLevel;
        SaveLoadManager.Data.inventoryCapacity = inventoryCapacity;
        SaveLoadManager.Data.inventoryFee = inventoryFee;
        SaveLoadManager.Data.lentAmount = lentAmount;
        SaveLoadManager.Data.paybackDateCnt = paybackDateCnt;
        SaveLoadManager.Data.investedAmount = investedAmount;
        SaveLoadManager.Data.wholesaleItem1 = wholesaleItem1;
        SaveLoadManager.Data.wholesaleItem1Cnt = wholesaleItem1Cnt;
        SaveLoadManager.Data.wholesaleItem1Cost = wholesaleItem1Cost;
        SaveLoadManager.Data.isItem1Purchased = isItem1Purchased;
        SaveLoadManager.Data.wholesaleItem2 = wholesaleItem2;
        SaveLoadManager.Data.wholesaleItem2Cnt = wholesaleItem2Cnt;
        SaveLoadManager.Data.wholesaleItem2Cost = wholesaleItem2Cost;
        SaveLoadManager.Data.isItem2Purchased = isItem2Purchased;
        SaveLoadManager.Data.isRandomBox1Purchased = isRandomBox1Purchased;
        SaveLoadManager.Data.randomBox1Item = randomBox1Item;
        SaveLoadManager.Data.randomBox1Cnt = randomBox1Cnt;
        SaveLoadManager.Data.isRandomBox2Purchased = isRandomBox2Purchased;
        SaveLoadManager.Data.randomBox2Item = randomBox2Item;
        SaveLoadManager.Data.randomBox2Cnt = randomBox2Cnt;
    }

    public void SetupNewGame(GameModes gameMode = GameModes.Default)
    {
        if(gameMode == GameModes.Default)
        {
            SetUpNewDefault();
        }
        Debug.Log($"Save Result: { SaveLoadManager.Save(currentSavedSlotIndex)}");
        
    }        

    private void SetUpNewDefault()
    {
        CurrentGameMode = GameModes.Default;
        days = 1;
        lastDay = 100;
        coins = 10000;
        inventoryLevel = 1;
        inventoryMinLevel = 1;
        inventoryMaxLevel = 7;
        inventoryCapacity = 50;
        inventoryFee = 100;
        lentAmount = 0;
        paybackDateCnt = -1;
        investedAmount = 0;
        wholesaleItem1 = Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxPrimary+1);
        wholesaleItem1Cnt = Random.Range(1,3)*50;
        wholesaleItem1Cost = entireItemDict[wholesaleItem1].price;
        isItem1Purchased = false;
        wholesaleItem2 = Random.Range(ItemDataIndex.minSecondary, ItemDataIndex.maxSecondary + 1);
        wholesaleItem2Cnt = Random.Range(1, 3) * 50;
        wholesaleItem2Cost = entireItemDict[wholesaleItem2].price;
        isItem2Purchased = false;
        isRandomBox1Purchased = false;
        randomBox1Item = -1;
        randomBox1Cnt = -1;
        isRandomBox2Purchased =false;
        randomBox2Item = -1;
        randomBox2Cnt = -1;
    }

    public void LoadSavedSlot(int slotIndex = 0)
    {
        currentSavedSlotIndex = slotIndex;

        entireItemDict = new Dictionary<int, SavedItemData>();
        entireItemDict.Clear();
        if (SaveLoadManager.Load(currentSavedSlotIndex))
        {
            foreach (var item in SaveLoadManager.Data.savedItemList)
            {
                entireItemDict.Add(item.ItemData.Id, item);
            }
        }

        CurrentGameMode = SaveLoadManager.Data.currentGameMode;
        days = SaveLoadManager.Data.days;
        lastDay = SaveLoadManager.Data.lastDay;
        coins = SaveLoadManager.Data.coins;
        inventoryLevel = SaveLoadManager.Data.inventoryLevel;
        inventoryMinLevel = SaveLoadManager.Data.inventoryMinLevel;
        inventoryMaxLevel = SaveLoadManager.Data.inventoryMaxLevel;
        inventoryCapacity = SaveLoadManager.Data.inventoryCapacity;
        inventoryFee = SaveLoadManager.Data.inventoryFee;
        lentAmount = SaveLoadManager.Data.lentAmount;
        paybackDateCnt = SaveLoadManager.Data.paybackDateCnt;
        investedAmount = SaveLoadManager.Data.investedAmount;
        wholesaleItem1 = SaveLoadManager.Data.wholesaleItem1;
        wholesaleItem1Cnt = SaveLoadManager.Data.wholesaleItem1Cnt;
        wholesaleItem1Cost = SaveLoadManager.Data.wholesaleItem1Cost;
        isItem1Purchased = SaveLoadManager.Data.isItem1Purchased;
        wholesaleItem2 = SaveLoadManager.Data.wholesaleItem2;
        wholesaleItem2Cnt = SaveLoadManager.Data.wholesaleItem2Cnt;
        wholesaleItem2Cost = SaveLoadManager.Data.wholesaleItem2Cost;
        isItem2Purchased = SaveLoadManager.Data.isItem2Purchased;
        isRandomBox1Purchased = SaveLoadManager.Data.isRandomBox1Purchased;
        randomBox1Item = SaveLoadManager.Data.randomBox1Item;
        randomBox1Cnt = SaveLoadManager.Data.randomBox1Cnt;
        isRandomBox2Purchased = SaveLoadManager.Data.isRandomBox2Purchased;
        randomBox2Item = SaveLoadManager.Data.randomBox2Item;
        randomBox2Cnt = SaveLoadManager.Data.randomBox2Cnt;
    }

    private void CallSave()
    {
        SynchronizeWithSaveData();
        SaveLoadManager.Save(currentSavedSlotIndex);
    }

    public void OnSleep()
    {
        if(days == lastDay)
        {
            OnSleepLastDay();
            return;
        }
        if(coins < inventoryFee)
        {
            CallInsufficientCoinEvent();
            return;
        }

        ++days;
        coins -= inventoryFee;
        coins += (int)(investedAmount * 0.04);
        ItemsPriceChangeOnSleep();
        CallSave();
    }

    private void ItemsPriceChangeOnSleep()
    {
        for(int i = ItemDataIndex.minPrimary; i <= ItemDataIndex.maxLuxury; ++i)
        {
            entireItemDict[i].trendRemainingDate--;
            if (entireItemDict[i].priceTrend == PriceTrends.Raising)
            {
                entireItemDict[i].price +=
                    Random.Range(DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MinChangable,
                    DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MaxChangable + 1);
            }
            if (entireItemDict[i].priceTrend == PriceTrends.Descending)
            {
                entireItemDict[i].price -=
                    Random.Range(DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MinChangable,
                    DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MaxChangable + 1);
            }
            entireItemDict[i].price = Mathf.Clamp(entireItemDict[i].price,
                DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MinPrice,
                DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MaxPrice + 1);
            if (entireItemDict[i].trendRemainingDate <= 0)
            {   
                entireItemDict[i].priceTrend = (PriceTrends)Random.Range(0, (int)PriceTrends.Count);
                if (entireItemDict[i].priceTrend != PriceTrends.Stationary)
                {
                    entireItemDict[i].trendRemainingDate = Random.Range(0, 3);
                }
            }
        }
    }

    

    private void OnSleepLastDay()
    {
        
    }

    private void CallInsufficientCoinEvent()
    {

    }
}
