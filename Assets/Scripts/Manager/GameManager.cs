using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

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
    // Temp Vals

    public int investProfitRatio = 4;

    // ~Temp Vals


    private static class ModeDependantVariables
    {
        public static int testIntVar = 0;
    }    

    public UnityEvent onSleepEvent;

    private bool isItemInitializingNeeded = false;
    public int currentSavedSlotIndex;

    public int InventoryOccupancy
    {
        get
        {
            int occupancy = 0;
            foreach(var item in entireItemDict.Values)
            {
                occupancy += item.ItemData.InventoryOccupancy * item.count;
            }
            return occupancy;
        }
    }

    // Datas To be Saved
    public GameModes CurrentGameMode { get; private set; }
    public Dictionary<int, SavedItemData> entireItemDict;
    public Dictionary<int, SavedSalesItemData> salesItemDict;
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
    public int lentPaybackAmout;
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

    private static void InitialCall()
    {
        instance = new GameManager();
        instance.SetupEntireItemData();
    }

    private void SetupEntireItemData()
    {
        onSleepEvent = new UnityEvent();
        entireItemDict = new Dictionary<int, SavedItemData>();
        entireItemDict.Clear();
        salesItemDict = new Dictionary<int, SavedSalesItemData>();
        salesItemDict.Clear();
       
        for(int i = ItemDataIndex.minPrimary; i <= ItemDataIndex.maxLuxury; ++i)
        {
            var tempSavedData = new SavedItemData();
            tempSavedData.ItemData = DataTableManager.ItemTable.Get(i);
            tempSavedData.priceID = -1;
            tempSavedData.price = -1;
            tempSavedData.priceTrend = PriceTrends.Stationary;
            tempSavedData.trendRemainingDate = 0;
            tempSavedData.avgCost = 0;
            tempSavedData.count = 0;

            entireItemDict.Add(tempSavedData.ItemData.Id, tempSavedData);                
        }
        for (int i = SalesItemDataIndex.minPrimary; i <= SalesItemDataIndex.maxLuxury; ++i)
        {
            var tempSavedSalesData = new SavedSalesItemData();
            tempSavedSalesData.SalesItemData = DataTableManager.SalesItemTable.Get(i);
            tempSavedSalesData.isOnSale = Random.Range(0,99) < tempSavedSalesData.SalesItemData.OnSaleProbability ? true : false;
            tempSavedSalesData.stock = Random.Range(tempSavedSalesData.SalesItemData.MinSupply, tempSavedSalesData.SalesItemData.MaxSupply + 1);

            salesItemDict.Add(tempSavedSalesData.SalesItemData.Id, tempSavedSalesData);
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
        var priceList = DataTableManager.PriceTable.GetPriceKeyList();

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
            entireItemDict[i].price = Random.Range(DataTableManager.PriceTable.Get(key).MinPrice, DataTableManager.PriceTable.Get(key).MaxPrice + 1);
            entireItemDict[i].priceTrend = (PriceTrends)Random.Range(0, (int)PriceTrends.Count);
            if (entireItemDict[i].priceTrend != PriceTrends.Stationary)
            {
                entireItemDict[i].trendRemainingDate = Random.Range(0, 3);
            }
            priceList.Remove(key);
        }
    }

    public void SetupNewGame(GameModes gameMode = GameModes.Default)
    {
        if(gameMode == GameModes.Default)
        {
            SetUpNewDefault();
            ItemsPriceChangeOnSleep();
            SalesItemChangeOnSleep();
            WholeSalesUpdateOnSleep();
        }
        CallSave();
        Debug.Log($"Save Result: { SaveLoadManager.Save(currentSavedSlotIndex)}");        
    }        

    private void SetUpItemDatas()
    {
        entireItemDict = new Dictionary<int, SavedItemData>();
        entireItemDict.Clear();
        salesItemDict = new Dictionary<int, SavedSalesItemData>();
        salesItemDict.Clear();

        foreach (var item in DataTableManager.ItemTable.GetItemTable().Values)
        {
            SavedItemData newItemData = new SavedItemData();
            newItemData.ItemData = item;
            newItemData.avgCost = 0;
            newItemData.isSoldOut = false;
            entireItemDict.Add(item.Id, newItemData);
        }
        foreach (var item in DataTableManager.SalesItemTable.GetSalesItemTable().Values)
        {
            SavedSalesItemData newSalesItem = new SavedSalesItemData();
            newSalesItem.SalesItemData = item;
            newSalesItem.isOnSale = true;
            newSalesItem.stock = 0;

            salesItemDict.Add(newSalesItem.SalesItemData.Id, newSalesItem);
        }
    }

    private void SetUpNewDefault(int slotIndex = 0, GameModes gameMode = GameModes.Default)
    {
        SetUpItemDatas();
        InitializeItemDictData();

        currentSavedSlotIndex = slotIndex;

        CurrentGameMode = GameModes.Default;

        days = 1;
        lastDay = 100;
        coins = 10000;

        tipIndex = Random.Range(GameInfos.minTipsIndex, GameInfos.maxTipsIndex+1);
        infoItemIndex = Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxLuxury + 1);
        isDisplayingMinPriceInfo = Random.Range(0, 2) == 0 ? false : true;
        isInfoOpened = false;

        inventoryLevel = 1;
        inventoryMinLevel = GameInfos.minInventoryLevel;
        inventoryMaxLevel = GameInfos.maxInventoryLevel;
        inventoryCapacity = 50;
        inventoryFee = 100;

        lentAmount = Random.Range(GameInfos.minLentAmount, GameInfos.maxLentAmount);
        paybackDateCnt = Random.Range(GameInfos.minPaybackDate, GameInfos.maxPaybackDate + 1);
        lentPaybackAmout = (int)(lentAmount * Random.Range(GameInfos.minLentAmountMultiplier, GameInfos.maxLentAmountMultiplier));
        ifLent = false;
        isBusinessmanAvailable = Random.Range(0, 2) == 0 ? false : true;

        investedAmount = 0;
        innProfit = 0;


        wholesaleItem1 = Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxPrimary+1);
        wholesaleItem1Cnt = Random.Range(1,3)*50;
        wholesaleItem1Cost = (int)(entireItemDict[wholesaleItem1].price * 0.8f);
        isItem1Purchased = false;
        isItem1PickedUp = false;
        isItem1Pickupable = false;

        wholesaleItem2 = Random.Range(ItemDataIndex.minSecondary, ItemDataIndex.maxSecondary + 1);
        wholesaleItem2Cnt = Random.Range(1, 3) * 50;
        wholesaleItem2Cost = (int)(entireItemDict[wholesaleItem2].price * 0.8f);
        isItem2Purchased = false;
        isItem2PickedUp = false;
        isItem2Pickupable = false;


        isRandomBox1Purchased = false;
        isRandomBox1PickedUp = false;
        randomBox1Item = -1;
        randomBox1Cnt = -1;

        isRandomBox2Purchased =false;
        isRandomBox2PickedUp = false;
        randomBox2Item = -1;
        randomBox2Cnt = -1;
    }

    public void LoadSavedSlot(int slotIndex = 0)
    {
        currentSavedSlotIndex = slotIndex;

        // ItemList and SalesItemList
        entireItemDict = new Dictionary<int, SavedItemData>();
        entireItemDict.Clear();
        salesItemDict = new Dictionary<int, SavedSalesItemData>();
        salesItemDict.Clear();
        if (SaveLoadManager.Load(currentSavedSlotIndex))
        {
            foreach (var item in SaveLoadManager.Data.savedItemList)
            {
                entireItemDict.Add(item.ItemData.Id, item);
            }
            foreach (var item in SaveLoadManager.Data.savedSalesItemList)
            {
                salesItemDict.Add(item.SalesItemData.Id, item);
            }
        }

        // GameMode and dependant data
        CurrentGameMode = SaveLoadManager.Data.currentGameMode;
        lastDay = SaveLoadManager.Data.lastDay;
        inventoryMinLevel = SaveLoadManager.Data.inventoryMinLevel;
        inventoryMaxLevel = SaveLoadManager.Data.inventoryMaxLevel;

        // Game Core Data
        days = SaveLoadManager.Data.days;
        coins = SaveLoadManager.Data.coins;

            // Availabilities
        tipIndex = SaveLoadManager.Data.tipIndex;
        infoItemIndex = SaveLoadManager.Data.infoItemIndex;
        isDisplayingMinPriceInfo = SaveLoadManager.Data.isDisplayingMinPriceInfo;
        isInfoOpened = SaveLoadManager.Data.isInfoOpened;

            // Inventory
        inventoryLevel = SaveLoadManager.Data.inventoryLevel;
        inventoryCapacity = SaveLoadManager.Data.inventoryCapacity;
        inventoryFee = SaveLoadManager.Data.inventoryFee;

            // Loan
        lentAmount = SaveLoadManager.Data.lentAmount;
        paybackDateCnt = SaveLoadManager.Data.paybackDateCnt;
        lentPaybackAmout = SaveLoadManager.Data.lentPaybackAmount;
        ifLent = SaveLoadManager.Data.ifLent;
        isBusinessmanAvailable = SaveLoadManager.Data.isBusinessmanAvailable;

            // Inn
        investedAmount = SaveLoadManager.Data.investedAmount;
        innProfit = SaveLoadManager.Data.innProfit;

            // Wholesale
        isItem1Purchased = SaveLoadManager.Data.isItem1Purchased;
        isItem1PickedUp = SaveLoadManager.Data.isItem1PickedUp;
        isItem1Pickupable = SaveLoadManager.Data.isItem1Pickupable;
        wholesaleItem1 = SaveLoadManager.Data.wholesaleItem1;
        wholesaleItem1Cnt = SaveLoadManager.Data.wholesaleItem1Cnt;
        wholesaleItem1Cost = SaveLoadManager.Data.wholesaleItem1Cost;

        isItem2Purchased = SaveLoadManager.Data.isItem2Purchased;
        isItem2PickedUp = SaveLoadManager.Data.isItem2PickedUp;
        isItem2Pickupable = SaveLoadManager.Data.isItem2Pickupable;
        wholesaleItem2 = SaveLoadManager.Data.wholesaleItem2;
        wholesaleItem2Cnt = SaveLoadManager.Data.wholesaleItem2Cnt;
        wholesaleItem2Cost = SaveLoadManager.Data.wholesaleItem2Cost;

            // RandomBox
        isRandomBox1Purchased = SaveLoadManager.Data.isRandomBox1Purchased;
        isRandomBox1PickedUp = SaveLoadManager.Data.isRandomBox1PickedUp;
        randomBox1Item = SaveLoadManager.Data.randomBox1Item;
        randomBox1Cnt = SaveLoadManager.Data.randomBox1Cnt;

        isRandomBox2Purchased = SaveLoadManager.Data.isRandomBox2Purchased;
        isRandomBox2PickedUp = SaveLoadManager.Data.isRandomBox2PickedUp;
        randomBox2Item = SaveLoadManager.Data.randomBox2Item;
        randomBox2Cnt = SaveLoadManager.Data.randomBox2Cnt;
    }

    private void SynchronizeWithSaveData()
    {
        // ItemList and SalesItemList
        SaveLoadManager.Data.savedItemList.Clear();
        SaveLoadManager.Data.savedSalesItemList.Clear();
        foreach (var saveData in entireItemDict.Values.ToList())
        {
            SaveLoadManager.Data.savedItemList.Add(saveData);
        }
        foreach (var saveData in salesItemDict.Values.ToList())
        {
            SaveLoadManager.Data.savedSalesItemList.Add(saveData);
        }

        // GameMode and dependant data
        SaveLoadManager.Data.currentGameMode = CurrentGameMode;
        SaveLoadManager.Data.lastDay = lastDay;
        SaveLoadManager.Data.inventoryMinLevel = inventoryMinLevel;
        SaveLoadManager.Data.inventoryMaxLevel = inventoryMaxLevel;

        // Game Core Data
        SaveLoadManager.Data.days = days;
        SaveLoadManager.Data.coins = coins;

        // Availabilities
        SaveLoadManager.Data.tipIndex = tipIndex;
        SaveLoadManager.Data.infoItemIndex = infoItemIndex;
        SaveLoadManager.Data.isDisplayingMinPriceInfo = isDisplayingMinPriceInfo;
        SaveLoadManager.Data.isInfoOpened = isInfoOpened;

        // Inventory
        SaveLoadManager.Data.inventoryLevel = inventoryLevel;
        SaveLoadManager.Data.inventoryCapacity = inventoryCapacity;
        SaveLoadManager.Data.inventoryFee = inventoryFee;

        // Loan
        SaveLoadManager.Data.lentAmount = lentAmount;
        SaveLoadManager.Data.paybackDateCnt = paybackDateCnt;
        SaveLoadManager.Data.lentPaybackAmount = lentPaybackAmout;
        SaveLoadManager.Data.ifLent = ifLent;
        SaveLoadManager.Data.isBusinessmanAvailable = isBusinessmanAvailable;

        // Inn
        SaveLoadManager.Data.investedAmount = investedAmount;
        SaveLoadManager.Data.innProfit = innProfit;

        // Wholesale
        SaveLoadManager.Data.isItem1Purchased = isItem1Purchased;
        SaveLoadManager.Data.isItem1PickedUp = isItem1PickedUp;
        SaveLoadManager.Data.isItem1Pickupable = isItem1Pickupable;
        SaveLoadManager.Data.wholesaleItem1 = wholesaleItem1;
        SaveLoadManager.Data.wholesaleItem1Cnt = wholesaleItem1Cnt;
        SaveLoadManager.Data.wholesaleItem1Cost = wholesaleItem1Cost;

        SaveLoadManager.Data.isItem2Purchased = isItem2Purchased;
        SaveLoadManager.Data.isItem2PickedUp = isItem2PickedUp;
        SaveLoadManager.Data.isItem2Pickupable = isItem2Pickupable;
        SaveLoadManager.Data.wholesaleItem2 = wholesaleItem2;
        SaveLoadManager.Data.wholesaleItem2Cnt = wholesaleItem2Cnt;
        SaveLoadManager.Data.wholesaleItem2Cost = wholesaleItem2Cost;

        // RandomBox
        SaveLoadManager.Data.isRandomBox1Purchased = isRandomBox1Purchased;
        SaveLoadManager.Data.isRandomBox1PickedUp = isRandomBox1PickedUp;
        SaveLoadManager.Data.randomBox1Item = randomBox1Item;
        SaveLoadManager.Data.randomBox1Cnt = randomBox1Cnt;

        SaveLoadManager.Data.isRandomBox2Purchased = isRandomBox2Purchased;
        SaveLoadManager.Data.isRandomBox2PickedUp = isRandomBox2PickedUp;
        SaveLoadManager.Data.randomBox2Item = randomBox2Item;
        SaveLoadManager.Data.randomBox2Cnt = randomBox2Cnt;
    }


    public void CallSave()
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
        tipIndex = Random.Range(GameInfos.minTipsIndex, GameInfos.maxTipsIndex + 1);
        
        ItemsPriceChangeOnSleep();
        SalesItemChangeOnSleep();
        LoanUpdateOnSleep();
        InnUpdateOnSleep();
        WholeSalesUpdateOnSleep(); // must be called after price change
        CallSave();
        onSleepEvent?.Invoke();
    }

    private void LoanUpdateOnSleep()
    {
        if(ifLent)
        {
            --paybackDateCnt;
            paybackDateCnt = Mathf.Max(paybackDateCnt, 0);
        }
        else
        {
            lentAmount = Random.Range(GameInfos.minLentAmount, GameInfos.maxLentAmount);
            paybackDateCnt = Random.Range(GameInfos.minPaybackDate, GameInfos.maxPaybackDate+1);
            lentPaybackAmout = (int)(lentAmount * Random.Range(GameInfos.minLentAmountMultiplier, GameInfos.maxLentAmountMultiplier));
        }
        isBusinessmanAvailable = Random.Range(0, 2) > 0 ? true : false;
    }

    private void InnUpdateOnSleep()
    {
        innProfit += (int)(investedAmount * 0.010001f * investProfitRatio);
        int newInfoItemIndex = Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxLuxury + 1);
        while (infoItemIndex == newInfoItemIndex)
        {
            newInfoItemIndex = Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxLuxury + 1);
        }
        infoItemIndex = newInfoItemIndex;
        isDisplayingMinPriceInfo = Random.Range(0, 2) == 0 ? false : true;
        isInfoOpened = false;
    }
    private void WholeSalesUpdateOnSleep()
    {
        if(isItem1Purchased && !isItem1PickedUp)
        {
            // not change
            isItem1Pickupable = true;
        }
        else
        {
            // change
            wholesaleItem1 = Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxPrimary + 1);
            wholesaleItem1Cnt = Random.Range(1, 3) * 50;
            wholesaleItem1Cost = (int)(entireItemDict[wholesaleItem1].price * 0.8f);
            isItem1Purchased = false;
            isItem1Pickupable = false;
            isItem1PickedUp = false;
        }

        if (isItem2Purchased && !isItem2PickedUp)
        {
            // not change
            isItem2Pickupable = true;
        }
        else
        {
            // change
            wholesaleItem2 = Random.Range(ItemDataIndex.minSecondary, ItemDataIndex.maxSecondary + 1);
            wholesaleItem2Cnt = Random.Range(1, 3) * 50;
            wholesaleItem2Cost = (int)(entireItemDict[wholesaleItem2].price * 0.8f);
            isItem2Purchased = false;
            isItem2Pickupable = false;
            isItem2PickedUp = false;
        }
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

    private void SalesItemChangeOnSleep()
    {
        for (int i = SalesItemDataIndex.minPrimary; i <= SalesItemDataIndex.maxLuxury; ++i)
        {            
            salesItemDict[i].isOnSale = Random.Range(0, 99) < salesItemDict[i].SalesItemData.OnSaleProbability ? true : false;
            salesItemDict[i].stock = Random.Range(salesItemDict[i].SalesItemData.MinSupply, salesItemDict[i].SalesItemData.MaxSupply + 1);
        }
    }

    private void OnSleepLastDay()
    {
        
    }

    private void CallInsufficientCoinEvent()
    {

    }

    public void ChangeInventoryLevel(int level)
    {
        inventoryLevel = level;
        var tempTable = DataTableManager.InventoryTable.Get(inventoryLevel);
        inventoryFee = tempTable.DailyCost;
        inventoryCapacity = tempTable.Capacity;
    }
}
