using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void Init()
    {
        //SaveLoadManager.Load();
    }

    // Temp Vals

    public bool isFirstTimeEver = false;
    public bool isDisplayTutorial = false;
    public int investProfitRatio = 4;
    public List<int> tempBulletinBoardContents;

    // ~Temp Vals


    private static class ModeDependantVariables
    {
        public static int testIntVar = 0;
    }    

    public UnityEvent onSleepEvent;

    private bool isItemInitializingNeeded = false;
    public int currentSavedSlotIndex { get; private set; }

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
    // BaseData

    public int diamonds;

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
    public bool isPrimaryShopAvailable;
    public bool isSecondaryShopAvailable;
    public bool isLuxuryShopAvailable;
    public List<int> notOnSaleItemsIds;
    public List<int> specialPriceItemIndexes;

    // Bulletin Board
    public List<int> bulletinBoardContentsId;

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
    public int randomBox1Price;
    public int randomBox1Cnt;

    public bool isRandomBox2Purchased;
    public bool isRandomBox2PickedUp;
    public int randomBox2Item;
    public int randomBox2Price;
    public int randomBox2Cnt;


    private static void InitialCall()
    {
        instance = new GameManager();
        instance.SetupEntireItemData();
    }

    private void DefaultSettings()
    {

    }

    private void SetupEntireItemData()
    {
        onSleepEvent = new UnityEvent();
        entireItemDict = new Dictionary<int, SavedItemData>();
        salesItemDict = new Dictionary<int, SavedSalesItemData>();
        bulletinBoardContentsId = new List<int>();

        for (int i = ItemDataIndex.minPrimary; i <= ItemDataIndex.maxLuxury; ++i)
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
            tempSavedSalesData.isOnSale = UnityEngine.Random.Range(0,99) < tempSavedSalesData.SalesItemData.OnSaleProbability ? true : false;
            tempSavedSalesData.stock = UnityEngine.Random.Range(tempSavedSalesData.SalesItemData.MinSupply, tempSavedSalesData.SalesItemData.MaxSupply + 1);

            salesItemDict.Add(tempSavedSalesData.SalesItemData.Id, tempSavedSalesData);
        }

        isItemInitializingNeeded = true;

        notOnSaleItemsIds = new List<int>();
        specialPriceItemIndexes = new List<int>();
        
        if (isItemInitializingNeeded)
        {
            InitializeItemDictData();
        }
        // SynchronizeWithSaveData();
    }

    private void InitializeItemDictData()
    {       
        var priceList = DataTableManager.PriceTable.GetPriceKeyList();

        for(int i = ItemDataIndex.minPrimary; i <= ItemDataIndex.maxPrimary; ++i)
        {
            int key = UnityEngine.Random.Range(PriceDataIndex.minPrimary, PriceDataIndex.maxPrimary + 1);
            while (!priceList.Contains(key))
            {
                key = UnityEngine.Random.Range(PriceDataIndex.minPrimary, PriceDataIndex.maxPrimary + 1);
            }
            entireItemDict[i].isOnBoardRecently = false;
            entireItemDict[i].bulletinBoardId = 0; // tempData
            entireItemDict[i].priceID = key;
            entireItemDict[i].price = UnityEngine.Random.Range(DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MinPrice, DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MaxPrice + 1);
            entireItemDict[i].priceTrend = (PriceTrends)UnityEngine.Random.Range(0,(int)PriceTrends.Count);
            if(entireItemDict[i].priceTrend != PriceTrends.Stationary)
            {
                entireItemDict[i].trendRemainingDate = UnityEngine.Random.Range(0, 3);
            }
            priceList.Remove(key);
        }

        for (int i = ItemDataIndex.minSecondary; i <= ItemDataIndex.maxSecondary; ++i)
        {
            int key = UnityEngine.Random.Range(PriceDataIndex.minSecondary, PriceDataIndex.maxSecondary + 1);
            while (!priceList.Contains(key))
            {
                key = UnityEngine.Random.Range(PriceDataIndex.minSecondary, PriceDataIndex.maxSecondary + 1);
            }
            entireItemDict[i].isOnBoardRecently = false;
            entireItemDict[i].bulletinBoardId = 0; // tempData
            entireItemDict[i].priceID = key;
            entireItemDict[i].price = UnityEngine.Random.Range(DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MinPrice, DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MaxPrice + 1);
            entireItemDict[i].priceTrend = (PriceTrends)UnityEngine.Random.Range(0, (int)PriceTrends.Count);
            if (entireItemDict[i].priceTrend != PriceTrends.Stationary)
            {
                entireItemDict[i].trendRemainingDate = UnityEngine.Random.Range(0, 3);
            }
            priceList.Remove(key);
        }

        for (int i = ItemDataIndex.minLuxury; i <= ItemDataIndex.maxLuxury; ++i)
        {
            int key = UnityEngine.Random.Range(PriceDataIndex.minLuxury, PriceDataIndex.maxLuxury + 1);
            while (!priceList.Contains(key))
            {
                key = UnityEngine.Random.Range(PriceDataIndex.minLuxury, PriceDataIndex.maxLuxury + 1);
            }
            entireItemDict[i].isOnBoardRecently = false;
            entireItemDict[i].bulletinBoardId = 0; // tempData
            entireItemDict[i].priceID = key;
            entireItemDict[i].price = UnityEngine.Random.Range(DataTableManager.PriceTable.Get(key).MinPrice, DataTableManager.PriceTable.Get(key).MaxPrice + 1);
            entireItemDict[i].priceTrend = (PriceTrends)UnityEngine.Random.Range(0, (int)PriceTrends.Count);
            if (entireItemDict[i].priceTrend != PriceTrends.Stationary)
            {
                entireItemDict[i].trendRemainingDate = UnityEngine.Random.Range(0, 3);
            }
            priceList.Remove(key);
        }
    }

    public void Restart()
    {
        SetupNewGame(CurrentGameMode, currentSavedSlotIndex);
    }

    public void SetupNewGame(GameModes gameMode, int SaveSlotIndex)
    {
        CurrentGameMode = gameMode;
        currentSavedSlotIndex = SaveSlotIndex;
       if(gameMode == GameModes.Default)
       {
            isDisplayTutorial = true;
       }
        SetUpGameMode(gameMode, SaveSlotIndex);
        ItemsPriceChangeOnSleep();
        SalesItemChangeOnSleepWithSelection();
        LoanUpdateOnSleep();
        InnUpdateOnSleep();
        WholeSalesUpdateOnSleep();
        RandomBoxUpdateOnSleep();
        BulletinBoardContentsUpdateOnSleep();
        CallSave();
        Debug.Log($"New Game Set and Save: { SaveLoadManager.Save(currentSavedSlotIndex)}");        
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

    private void SetUpGameMode(GameModes gameMode, int slotIndex)
    {
        SetUpItemDatas();
        InitializeItemDictData();

        currentSavedSlotIndex = slotIndex;
        CurrentGameMode = gameMode;

        //lastDay = 100;
        //inventoryMinLevel = GameInfos.minInventoryLevel;
        //inventoryMaxLevel = GameInfos.maxInventoryLevel;


        // GameMode and datas dependant to the GameMode
        lastDay = DataTableManager.GameModeTable.Get(CurrentGameMode).LastDay;
        inventoryMinLevel = DataTableManager.GameModeTable.Get(CurrentGameMode).InventoryMinLv;
        inventoryMaxLevel = DataTableManager.GameModeTable.Get(CurrentGameMode).InventoryMaxLv;

        // Game Core Data
        days = 1;
        coins = DataTableManager.GameModeTable.Get(CurrentGameMode).InitialCoin;

        // Availabilities
        tipIndex = UnityEngine.Random.Range(GameInfos.minTipsIndex, GameInfos.maxTipsIndex+1);
        infoItemIndex = UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxSecondary + 1);
        isDisplayingMinPriceInfo = UnityEngine.Random.Range(0, 2) == 0 ? false : true;
        isInfoOpened = false;
                
        isPrimaryShopAvailable = true;
        isSecondaryShopAvailable = true;
        isLuxuryShopAvailable = true;

        // Inventory
        inventoryLevel = DataTableManager.GameModeTable.Get(CurrentGameMode).InventoryInitialLv;
        inventoryCapacity = DataTableManager.InventoryTable.Get(inventoryLevel).Capacity;
        inventoryFee = DataTableManager.InventoryTable.Get(inventoryLevel).DailyCost;

        // Loan
        lentAmount = UnityEngine.Random.Range(GameInfos.minLentAmount, GameInfos.maxLentAmount);
        paybackDateCnt = UnityEngine.Random.Range(GameInfos.minPaybackDate, GameInfos.maxPaybackDate + 1);
        lentPaybackAmout = (int)(lentAmount * UnityEngine.Random.Range(GameInfos.minLentAmountMultiplier, GameInfos.maxLentAmountMultiplier));
        ifLent = false;
        isBusinessmanAvailable = UnityEngine.Random.Range(0, 2) == 0 ? false : true;

        // Inn
        investedAmount = 0;
        innProfit = 0;

        // Wholesale
        wholesaleItem1 = UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxPrimary+1);
        wholesaleItem1Cnt = UnityEngine.Random.Range(1,3)*50;
        wholesaleItem1Cost = (int)(entireItemDict[wholesaleItem1].price * 0.8f);
        isItem1Purchased = false;
        isItem1PickedUp = false;
        isItem1Pickupable = false;

        wholesaleItem2 = UnityEngine.Random.Range(ItemDataIndex.minSecondary, ItemDataIndex.maxSecondary + 1);
        wholesaleItem2Cnt = UnityEngine.Random.Range(1, 3) * 50;
        wholesaleItem2Cost = (int)(entireItemDict[wholesaleItem2].price * 0.8f);
        isItem2Purchased = false;
        isItem2PickedUp = false;
        isItem2Pickupable = false;

        // RandomBox
        isRandomBox1Purchased = false;
        isRandomBox1PickedUp = false;
        randomBox1Item = UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxPrimary +1);
        randomBox1Cnt = UnityEngine.Random.Range(1,6);
        randomBox1Price = entireItemDict[UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxPrimary + 1)].price;

        isRandomBox2Purchased =false;
        isRandomBox2PickedUp = false;
        randomBox2Item = UnityEngine.Random.Range(ItemDataIndex.minSecondary, ItemDataIndex.maxSecondary + 1);
        randomBox2Cnt = UnityEngine.Random.Range(1, 6);
        randomBox2Price = entireItemDict[UnityEngine.Random.Range(ItemDataIndex.minSecondary, ItemDataIndex.maxSecondary + 1)].price;
    }

    public void LoadSavedSlot(int slotIndex)
    {
        currentSavedSlotIndex = slotIndex;

        // ItemList and SalesItemList
        entireItemDict = new Dictionary<int, SavedItemData>();
        entireItemDict.Clear();
        salesItemDict = new Dictionary<int, SavedSalesItemData>();
        salesItemDict.Clear();
        if (!SaveLoadManager.Load(currentSavedSlotIndex))
        {
            Debug.LogError("LoadSlot failed");
        }

        foreach (var item in SaveLoadManager.GameData.savedItemList)
        {
            entireItemDict.Add(item.ItemData.Id, item);
        }
        foreach (var item in SaveLoadManager.GameData.savedSalesItemList)
        {
            salesItemDict.Add(item.SalesItemData.Id, item);
        }
        

        // GameMode and dependant data
        CurrentGameMode = SaveLoadManager.GameData.currentGameMode;
        lastDay = SaveLoadManager.GameData.lastDay;
        inventoryMinLevel = SaveLoadManager.GameData.inventoryMinLevel;
        inventoryMaxLevel = SaveLoadManager.GameData.inventoryMaxLevel;

        // Game Core Data
        days = SaveLoadManager.GameData.days;
        coins = SaveLoadManager.GameData.coins;

            // Availabilities
        tipIndex = SaveLoadManager.GameData.tipIndex;
        infoItemIndex = SaveLoadManager.GameData.infoItemIndex;
        isDisplayingMinPriceInfo = SaveLoadManager.GameData.isDisplayingMinPriceInfo;
        isInfoOpened = SaveLoadManager.GameData.isInfoOpened;
        isPrimaryShopAvailable = SaveLoadManager.GameData.isPrimaryShopAvailable;
        isSecondaryShopAvailable = SaveLoadManager.GameData.isSecondaryShopAvailable;
        isLuxuryShopAvailable = SaveLoadManager.GameData.isLuxuryShopAvailable;
        notOnSaleItemsIds = SaveLoadManager.GameData.notOnSaleItemsIds;
        specialPriceItemIndexes = SaveLoadManager.GameData.specialPriceItemIndexes;

        // Bulletin Board
        bulletinBoardContentsId = SaveLoadManager.GameData.bulletinBoardContentsId;

        // Inventory
        inventoryLevel = SaveLoadManager.GameData.inventoryLevel;
        inventoryCapacity = SaveLoadManager.GameData.inventoryCapacity;
        inventoryFee = SaveLoadManager.GameData.inventoryFee;

            // Loan
        lentAmount = SaveLoadManager.GameData.lentAmount;
        paybackDateCnt = SaveLoadManager.GameData.paybackDateCnt;
        lentPaybackAmout = SaveLoadManager.GameData.lentPaybackAmount;
        ifLent = SaveLoadManager.GameData.ifLent;
        isBusinessmanAvailable = SaveLoadManager.GameData.isBusinessmanAvailable;

            // Inn
        investedAmount = SaveLoadManager.GameData.investedAmount;
        innProfit = SaveLoadManager.GameData.innProfit;

            // Wholesale
        isItem1Purchased = SaveLoadManager.GameData.isItem1Purchased;
        isItem1PickedUp = SaveLoadManager.GameData.isItem1PickedUp;
        isItem1Pickupable = SaveLoadManager.GameData.isItem1Pickupable;
        wholesaleItem1 = SaveLoadManager.GameData.wholesaleItem1;
        wholesaleItem1Cnt = SaveLoadManager.GameData.wholesaleItem1Cnt;
        wholesaleItem1Cost = SaveLoadManager.GameData.wholesaleItem1Cost;

        isItem2Purchased = SaveLoadManager.GameData.isItem2Purchased;
        isItem2PickedUp = SaveLoadManager.GameData.isItem2PickedUp;
        isItem2Pickupable = SaveLoadManager.GameData.isItem2Pickupable;
        wholesaleItem2 = SaveLoadManager.GameData.wholesaleItem2;
        wholesaleItem2Cnt = SaveLoadManager.GameData.wholesaleItem2Cnt;
        wholesaleItem2Cost = SaveLoadManager.GameData.wholesaleItem2Cost;

            // RandomBox
        isRandomBox1Purchased = SaveLoadManager.GameData.isRandomBox1Purchased;
        isRandomBox1PickedUp = SaveLoadManager.GameData.isRandomBox1PickedUp;
        randomBox1Item = SaveLoadManager.GameData.randomBox1Item;
        randomBox1Price = SaveLoadManager.GameData.randomBox1Price;
        randomBox1Cnt = SaveLoadManager.GameData.randomBox1Cnt;

        isRandomBox2Purchased = SaveLoadManager.GameData.isRandomBox2Purchased;
        isRandomBox2PickedUp = SaveLoadManager.GameData.isRandomBox2PickedUp;
        randomBox2Item = SaveLoadManager.GameData.randomBox2Item;
        randomBox2Price = SaveLoadManager.GameData.randomBox2Price;
        randomBox2Cnt = SaveLoadManager.GameData.randomBox2Cnt;
    }

    private void SynchronizeWithSaveData()
    {
        // To Avoid Null
        SaveLoadManager.AvoidNull();

        // ItemList and SalesItemList
        SaveLoadManager.GameData.savedItemList.Clear();
        SaveLoadManager.GameData.savedSalesItemList.Clear();
        foreach (var saveData in entireItemDict.Values.ToList())
        {
            SaveLoadManager.GameData.savedItemList.Add(saveData);
        }
        foreach (var saveData in salesItemDict.Values.ToList())
        {
            SaveLoadManager.GameData.savedSalesItemList.Add(saveData);
        }

        // GameMode and dependant data
        SaveLoadManager.GameData.currentGameMode = CurrentGameMode;
        SaveLoadManager.GameData.lastDay = lastDay;
        SaveLoadManager.GameData.inventoryMinLevel = inventoryMinLevel;
        SaveLoadManager.GameData.inventoryMaxLevel = inventoryMaxLevel;

        // Game Core Data
        SaveLoadManager.GameData.days = days;
        SaveLoadManager.GameData.coins = coins;

        // Availabilities
        SaveLoadManager.GameData.tipIndex = tipIndex;
        SaveLoadManager.GameData.infoItemIndex = infoItemIndex;
        SaveLoadManager.GameData.isDisplayingMinPriceInfo = isDisplayingMinPriceInfo;
        SaveLoadManager.GameData.isInfoOpened = isInfoOpened;
        SaveLoadManager.GameData.isPrimaryShopAvailable = isPrimaryShopAvailable;
        SaveLoadManager.GameData.isSecondaryShopAvailable = isSecondaryShopAvailable;
        SaveLoadManager.GameData.isLuxuryShopAvailable = isLuxuryShopAvailable;
        SaveLoadManager.GameData.notOnSaleItemsIds = notOnSaleItemsIds;
        SaveLoadManager.GameData.specialPriceItemIndexes = specialPriceItemIndexes;

        // Bulletin Board
        SaveLoadManager.GameData.bulletinBoardContentsId = bulletinBoardContentsId;

        // Inventory
        SaveLoadManager.GameData.inventoryLevel = inventoryLevel;
        SaveLoadManager.GameData.inventoryCapacity = inventoryCapacity;
        SaveLoadManager.GameData.inventoryFee = inventoryFee;

        // Loan
        SaveLoadManager.GameData.lentAmount = lentAmount;
        SaveLoadManager.GameData.paybackDateCnt = paybackDateCnt;
        SaveLoadManager.GameData.lentPaybackAmount = lentPaybackAmout;
        SaveLoadManager.GameData.ifLent = ifLent;
        SaveLoadManager.GameData.isBusinessmanAvailable = isBusinessmanAvailable;

        // Inn
        SaveLoadManager.GameData.investedAmount = investedAmount;
        SaveLoadManager.GameData.innProfit = innProfit;

        // Wholesale
        SaveLoadManager.GameData.isItem1Purchased = isItem1Purchased;
        SaveLoadManager.GameData.isItem1PickedUp = isItem1PickedUp;
        SaveLoadManager.GameData.isItem1Pickupable = isItem1Pickupable;
        SaveLoadManager.GameData.wholesaleItem1 = wholesaleItem1;
        SaveLoadManager.GameData.wholesaleItem1Cnt = wholesaleItem1Cnt;
        SaveLoadManager.GameData.wholesaleItem1Cost = wholesaleItem1Cost;

        SaveLoadManager.GameData.isItem2Purchased = isItem2Purchased;
        SaveLoadManager.GameData.isItem2PickedUp = isItem2PickedUp;
        SaveLoadManager.GameData.isItem2Pickupable = isItem2Pickupable;
        SaveLoadManager.GameData.wholesaleItem2 = wholesaleItem2;
        SaveLoadManager.GameData.wholesaleItem2Cnt = wholesaleItem2Cnt;
        SaveLoadManager.GameData.wholesaleItem2Cost = wholesaleItem2Cost;

        // RandomBox
        SaveLoadManager.GameData.isRandomBox1Purchased = isRandomBox1Purchased;
        SaveLoadManager.GameData.isRandomBox1PickedUp = isRandomBox1PickedUp;
        SaveLoadManager.GameData.randomBox1Item = randomBox1Item;
        SaveLoadManager.GameData.randomBox1Price = randomBox1Price;
        SaveLoadManager.GameData.randomBox1Cnt = randomBox1Cnt;

        SaveLoadManager.GameData.isRandomBox2Purchased = isRandomBox2Purchased;
        SaveLoadManager.GameData.isRandomBox2PickedUp = isRandomBox2PickedUp;
        SaveLoadManager.GameData.randomBox2Item = randomBox2Item;
        SaveLoadManager.GameData.randomBox2Price = randomBox2Price;
        SaveLoadManager.GameData.randomBox2Cnt = randomBox2Cnt;
    }

    private void SynchronizeWithBaseSaveData()
    {
        SaveLoadManager.BaseData.gameModes[currentSavedSlotIndex-1] = CurrentGameMode;
        SaveLoadManager.BaseData.days[currentSavedSlotIndex-1] = days;
        SaveLoadManager.BaseData.coins[currentSavedSlotIndex-1] = coins;
        SaveLoadManager.BaseData.dateTimes[currentSavedSlotIndex-1] = DateTime.Now;
        SaveLoadManager.BaseData.bgmVolume = SoundManager.Instance.BgmVolume;
        SaveLoadManager.BaseData.sfxVolume = SoundManager.Instance.SfxVolume;
    }

    public void CallSave()
    {
        SynchronizeWithSaveData();
        SaveLoadManager.Save(currentSavedSlotIndex);
        SynchronizeWithBaseSaveData();
        SaveLoadManager.SaveBase();
        Debug.Log($"Save Called in GM to slot:{currentSavedSlotIndex}");
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
        tipIndex = UnityEngine.Random.Range(GameInfos.minTipsIndex, GameInfos.maxTipsIndex + 1);
        ItemsPriceChangeOnSleep();
        //SalesItemChangeOnSleepWithProbability();
        SalesItemChangeOnSleepWithSelection();
        LoanUpdateOnSleep();
        InnUpdateOnSleep();
        WholeSalesUpdateOnSleep(); // must be called after price change
        RandomBoxUpdateOnSleep();
        BulletinBoardContentsUpdateOnSleep(); // must be called after price change
        OnSleepVariousModes();
        CallSave();
        onSleepEvent?.Invoke();
    }

    public void OnSleepVariousModes()
    {
        switch (CurrentGameMode)
        {
            case GameModes.Default:
                if(days == 51)
                {
                    coins -= 20000;
                }
                break;
            case GameModes.ShortGame:
                break;
            case GameModes.Endless:
                break;
            case GameModes.Poverty:
                break;
            case GameModes.ShowMeTheMoney:
                break;
            case GameModes.ProdigalSon:
                coins /= 2;
                break;
            case GameModes.ProdigalSons:
                coins = 0;
                break;
            case GameModes.BigInventory:
                break;
            case GameModes.SmallInventory:
                break;
            case GameModes.IsAnyoneThere:
                isPrimaryShopAvailable = false;
                isSecondaryShopAvailable = false;
                isLuxuryShopAvailable = false;
                break;
        }
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
            lentAmount = UnityEngine.Random.Range(GameInfos.minLentAmount, GameInfos.maxLentAmount);
            paybackDateCnt = UnityEngine.Random.Range(GameInfos.minPaybackDate, GameInfos.maxPaybackDate+1);
            lentPaybackAmout = (int)(lentAmount * UnityEngine.Random.Range(GameInfos.minLentAmountMultiplier, GameInfos.maxLentAmountMultiplier));
        }
        isBusinessmanAvailable = UnityEngine.Random.Range(0, 2) > 0 ? true : false;
    }

    private void InnUpdateOnSleep()
    {
        innProfit += (int)(investedAmount * 0.010001f * investProfitRatio);
        int newInfoItemIndex = UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxSecondary + 1);
        while (infoItemIndex == newInfoItemIndex)
        {
            newInfoItemIndex = UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxSecondary + 1);
        }
        infoItemIndex = newInfoItemIndex;
        isDisplayingMinPriceInfo = UnityEngine.Random.Range(0, 2) == 0 ? false : true;
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
            wholesaleItem1 = UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxPrimary + 1);
            wholesaleItem1Cnt = UnityEngine.Random.Range(1, 3) * 50;
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
            wholesaleItem2 = UnityEngine.Random.Range(ItemDataIndex.minSecondary, ItemDataIndex.maxSecondary + 1);
            wholesaleItem2Cnt = UnityEngine.Random.Range(1, 3) * 50;
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
            entireItemDict[i].isOnBoardRecently = false;
            if (tempBulletinBoardContents == null)
            {
                tempBulletinBoardContents = new List<int>();
            }

            if (entireItemDict[i].priceTrend == PriceTrends.Rising)
            {
                if (entireItemDict[i].price > DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MaxChangable * entireItemDict[i].ItemData.InventoryOccupancy)
                {
                    entireItemDict[i].price +=
                        UnityEngine.Random.Range(DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MinChangable,
                        DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MaxChangable + 1) * entireItemDict[i].ItemData.InventoryOccupancy
                        - UnityEngine.Random.Range(0, entireItemDict[i].ItemData.InventoryOccupancy + 1);
                }
                else
                {
                    entireItemDict[i].price = (int)(entireItemDict[i].price * UnityEngine.Random.Range(1f, 2f));
                }
            }
            if (entireItemDict[i].priceTrend == PriceTrends.Declining)
            {
                if (entireItemDict[i].price * 0.5 > DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MaxChangable * entireItemDict[i].ItemData.InventoryOccupancy)
                {
                    entireItemDict[i].price -=
                    UnityEngine.Random.Range(DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MinChangable,
                    DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MaxChangable * entireItemDict[i].ItemData.InventoryOccupancy
                    + UnityEngine.Random.Range(0, entireItemDict[i].ItemData.InventoryOccupancy + 1));
                }
                else
                {
                    entireItemDict[i].price = (int)(entireItemDict[i].price * UnityEngine.Random.Range(0.5f, 1f));
                }
            }
            entireItemDict[i].price = Mathf.Clamp(entireItemDict[i].price,
                DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MinPrice * entireItemDict[i].ItemData.InventoryOccupancy,
                (DataTableManager.PriceTable.Get(entireItemDict[i].priceID).MaxPrice + 1)
                * entireItemDict[i].ItemData.InventoryOccupancy);
            if (entireItemDict[i].trendRemainingDate <= 0)
            {                   
                // remove contents ran out of trend date from bulletin board.
                if (bulletinBoardContentsId.Contains(entireItemDict[i].ItemData.Id))
                {
                    bulletinBoardContentsId.Remove(entireItemDict[i].ItemData.Id);      
                }

                entireItemDict[i].priceTrend = (PriceTrends)UnityEngine.Random.Range(0, (int)PriceTrends.Count);
                if (entireItemDict[i].priceTrend != PriceTrends.Stationary)
                {
                    entireItemDict[i].trendRemainingDate = UnityEngine.Random.Range(0, 4);
                    // temp List of bulletin contents out of items with recent price change
                    if (entireItemDict[i].trendRemainingDate >= 2 && i <= ItemDataIndex.maxSecondary)
                    {                       

                        entireItemDict[i].isOnBoardRecently = true;
                        entireItemDict[i].bulletinBoardId =
                            GameInfos.GetBulletinInfoStringId(entireItemDict[i].ItemData.ItemType,
                            entireItemDict[i].priceTrend);
                        tempBulletinBoardContents.Add(entireItemDict[i].ItemData.Id);
                    }
                }
            }
        }
    }

    private void BulletinBoardContentsUpdateOnSleep()
    {                
        if (tempBulletinBoardContents == null)
        {
            tempBulletinBoardContents = new List<int>();
        }

        int numberOfNewInfo = UnityEngine.Random.Range(0, 4);
        while(bulletinBoardContentsId.Count + numberOfNewInfo <=1)
        {
            numberOfNewInfo = UnityEngine.Random.Range(1, 4);
        }

        while(bulletinBoardContentsId.Count + numberOfNewInfo >= 7)
        {
            bulletinBoardContentsId.RemoveAt(0);
        }

        while(numberOfNewInfo > 0 && tempBulletinBoardContents.Count != 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, tempBulletinBoardContents.Count);
            if (!bulletinBoardContentsId.Contains(tempBulletinBoardContents[randomIndex]))
            {
                bulletinBoardContentsId.Add(tempBulletinBoardContents[randomIndex]);
            }
            tempBulletinBoardContents.RemoveAt(randomIndex);            
            numberOfNewInfo--;
            if (tempBulletinBoardContents.Count == 0)
            {
                numberOfNewInfo = 0;
            }
        }
        tempBulletinBoardContents.Clear();
    }

    private void SalesItemChangeOnSleepWithProbability()
    {
        for (int i = SalesItemDataIndex.minPrimary; i <= SalesItemDataIndex.maxLuxury; ++i)
        {            
            salesItemDict[i].isOnSale = UnityEngine.Random.Range(1f, 100f) <= salesItemDict[i].SalesItemData.OnSaleProbability ? true : false;
            salesItemDict[i].stock = UnityEngine.Random.Range(salesItemDict[i].SalesItemData.MinSupply, salesItemDict[i].SalesItemData.MaxSupply + 1);
        }
    }

    private void SalesItemChangeOnSleepWithSelection()
    {
        isPrimaryShopAvailable = true;
        isSecondaryShopAvailable = true;
        isLuxuryShopAvailable = true;
        notOnSaleItemsIds.Clear();

        // Primary
        List<int> tempList = new List<int>();
        while(tempList.Count < 5)
        {
            var randPrimaryIndex = UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxPrimary + 1);
            if(!tempList.Contains(randPrimaryIndex))
            {
                tempList.Add(randPrimaryIndex);
            }
        }
        
        foreach(var item in tempList)
        {
            notOnSaleItemsIds.Add(item);
        }

        // Secondary
        tempList.Clear();
        while(tempList.Count < 5)
        {
            var randSecondaryIndex = UnityEngine.Random.Range(ItemDataIndex.minSecondary, ItemDataIndex.maxSecondary + 1);
            if (!tempList.Contains(randSecondaryIndex))
            {
                tempList.Add(randSecondaryIndex);
            }
        }
        foreach (var item in tempList)
        {
            notOnSaleItemsIds.Add(item);
        }

        // Special Merchant Items
        specialPriceItemIndexes.Clear();
        while(specialPriceItemIndexes.Count < 3)
        {
            var randSpecialPriceItemIndex = UnityEngine.Random.Range(0, notOnSaleItemsIds.Count);
            var id = notOnSaleItemsIds[randSpecialPriceItemIndex];
            if(!specialPriceItemIndexes.Contains(id))
            {
                specialPriceItemIndexes.Add(id);
            }
        }

        // Apply to itemLists
        foreach(var item in salesItemDict.Values)
        {            
            if(notOnSaleItemsIds.Contains(item.SalesItemData.SalesItemId))
            {
                item.isOnSale = false;
            }
            else
            {
                item.isOnSale = true;
                item.stock = UnityEngine.Random.Range(item.SalesItemData.MinSupply, item.SalesItemData.MaxSupply + 1);
            }
        }
    }

    private void RandomBoxUpdateOnSleep()
    {
        randomBox1Item = UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxPrimary + 1);
        randomBox2Item = UnityEngine.Random.Range(ItemDataIndex.minSecondary, ItemDataIndex.maxSecondary + 1);

        randomBox1Cnt = UnityEngine.Random.Range(1, 6);
        randomBox2Cnt = UnityEngine.Random.Range(1, 6);

        randomBox1Price = (int)(entireItemDict[UnityEngine.Random.Range(ItemDataIndex.minPrimary, ItemDataIndex.maxPrimary + 1)]
            .price * UnityEngine.Random.Range(1f, 5f));
        randomBox2Price = (int)(entireItemDict[UnityEngine.Random.Range(ItemDataIndex.minSecondary, ItemDataIndex.maxSecondary + 1)]
            .price * UnityEngine.Random.Range(1f, 5f));

        isRandomBox1PickedUp = false;
        isRandomBox2PickedUp = false;
        isRandomBox1Purchased = false;
        isRandomBox2Purchased = false;
    }

    public void OnSleepLastDay()
    {
        if(coins > SaveLoadManager.BaseData.bestScore[(int)CurrentGameMode])
        {
            SaveLoadManager.BaseData.bestScore[(int)CurrentGameMode] = coins;
        }
        if(coins >= DataTableManager.GameModeTable.Get(CurrentGameMode).DiamondGoal)
        {
            int bonusDiamonds = (int)((coins - DataTableManager.GameModeTable.Get(CurrentGameMode).DiamondGoal)
                * DataTableManager.GameModeTable.Get(CurrentGameMode).DiamondPaybackRate);
            SaveLoadManager.BaseData.diamonds += bonusDiamonds;
            SaveLoadManager.BaseData.diamonds += 100;
        }
        
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
