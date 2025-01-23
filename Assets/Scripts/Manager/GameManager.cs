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

    public static Dictionary<int, SavedItemData> entireItemDict;
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
        entireItemDict = new Dictionary<int, SavedItemData>();
        instance.SetupEntireItemData();
    }

    private void SetupEntireItemData()
    {
        entireItemDict.Clear();
        if (SaveLoadManager.Load(currentSavedSlotIndex))
        {
            foreach (var item in SaveLoadManager.Data.savedItemList)
            {
                entireItemDict.Add(item.ItemData.Id, item);
            }
            isItemInitializingNeeded = false;
        }
        else
        {
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
        }
        if (isItemInitializingNeeded)
        {
            InitializeItemDictData();
        }
    }

    private void InitializeItemDictData()
    {
        var priceTable = DataTableManager.Get<PriceTable>(DataTableIds.Price[0]);
        var priceList = priceTable.GetPriceKeyList();

        for(int i = ItemDataIndex.minPrimary; i <= ItemDataIndex.maxPrimary; ++i)
        {
            int key = Random.Range(PriceDataIndex.minPrimary, PriceDataIndex.maxPrimary);
            while (priceList.Contains(key))
            {
                key = Random.Range(PriceDataIndex.minPrimary, PriceDataIndex.maxPrimary);
            }

            entireItemDict[i].priceID = key;
            entireItemDict[i].price = Random.Range(DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MinPrice, DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MaxPrice + 1);
            priceList.Remove(key);
        }

        for (int i = ItemDataIndex.minSecondary; i <= ItemDataIndex.maxSecondary; ++i)
        {
            int key = Random.Range(PriceDataIndex.minSecondary, PriceDataIndex.maxSecondary);
            while (priceList.Contains(key))
            {
                key = Random.Range(PriceDataIndex.minSecondary, PriceDataIndex.maxSecondary);
            }

            entireItemDict[i].priceID = key;
            entireItemDict[i].price = Random.Range(DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MinPrice, DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MaxPrice + 1);
            priceList.Remove(key);
        }

        for (int i = ItemDataIndex.minLuxury; i <= ItemDataIndex.maxLuxury; ++i)
        {
            int key = Random.Range(PriceDataIndex.minLuxury, PriceDataIndex.maxLuxury);
            while (priceList.Contains(key))
            {
                key = Random.Range(PriceDataIndex.minLuxury, PriceDataIndex.maxLuxury);
            }

            entireItemDict[i].priceID = key;
            entireItemDict[i].price = Random.Range(DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MinPrice, DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).Get(key).MaxPrice + 1);
            priceList.Remove(key);
        }
    }
    
    public void SetupNewGame(GameModes gameMode = GameModes.Default)
    {
        if(gameMode == GameModes.Default)
        {
            SetUpNewDefault();
        }
        SaveLoadManager.Save(currentSavedSlotIndex);
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

    public void LoadSavedSlot(int slotIndex)
    {
        currentSavedSlotIndex = slotIndex;
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
        SaveLoadManager.Save(currentSavedSlotIndex);
    }

    private void ItemsPriceChangeOnSleep()
    {
        for(int i = ItemDataIndex.minPrimary; i <= ItemDataIndex.maxLuxury; ++i)
        {
            if (entireItemDict[i].trendRemainingDate == 0)
            {
                entireItemDict[i].priceTrend = (PriceTrends)Random.Range(0, (int)PriceTrends.Count);
                if (entireItemDict[i].priceTrend != PriceTrends.Stationary)
                {
                    entireItemDict[i].trendRemainingDate = Random.Range(0, 3);
                }
                // 흐름잔여일 0에서도 상승, 하락 1회 처리 로직 추가 필요
            }
            else
            {
                entireItemDict[i].trendRemainingDate--;
                if (entireItemDict[i].priceTrend == PriceTrends.Raising)
                {
                    entireItemDict[i].price +=
                        Random.Range(DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).
                        Get(entireItemDict[i].priceID).MinChangable,
                        DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).
                        Get(entireItemDict[i].priceID).MaxChangable + 1);
                }
                if (entireItemDict[i].priceTrend == PriceTrends.Descending)
                {
                    entireItemDict[i].price -=
                        Random.Range(DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).
                        Get(entireItemDict[i].priceID).MinChangable,
                        DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).
                        Get(entireItemDict[i].priceID).MaxChangable + 1);
                }
                Mathf.Clamp(entireItemDict[i].price,
                    DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).
                        Get(entireItemDict[i].priceID).MinPrice,
                    DataTableManager.Get<PriceTable>(DataTableIds.Price[0]).
                        Get(entireItemDict[i].priceID).MaxPrice);
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
