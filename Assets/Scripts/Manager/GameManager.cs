using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int days;
    public int Days {  get { return days; } }
    [SerializeField] private int lastDay;

    [SerializeField] private int coins;
    public int Coins { get { return coins; } }
    [SerializeField] private int inventoryLevel;
    [SerializeField] private int inventoryMaxLevel;

    private int inventoryCapacity;
    public int InventoryCapacity { get { return inventoryCapacity; } }

    [SerializeField] private int lentAmount;
    [SerializeField] private int paybackDateCnt;

    [SerializeField] private int investedAmount;

    private int wholesaleItem1;
    private int wholesaleItem1Cnt;
    private int wholesaleItem1Cost;
    private bool isItem1Purchased;
    private int wholesaleItem2;
    private int wholesaleItem2Cnt;
    private int wholesaleItem2Cost;
    private bool isItem2Purchased;

    private bool isRandomBox1Purchased;
    private int randomBox1Item;
    private int randomBox1Cnt;
    private bool isRandomBox2Purchased;
    private int randomBox2Item;
    private int randomBox2Cnt;

}
