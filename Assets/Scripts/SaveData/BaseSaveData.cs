using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSaveData
{
    public int Version { get; protected set; }
    public abstract BaseSaveData VersionUp();
}

public class BaseSaveDataV1 : BaseSaveData
{    
    // Saved Data
        // Resource
    public int diamonds;
    public int diamondsSpent;

        // AvailableOnce
    public bool isEnclopediaEnabled;
    public bool isMarketDominansEnabled;
    public bool isBulletinBoardAdditionalEnabled;
    public bool isPriceChangeDetectorEnabled;
    public bool isRandomBoxCountDetectorEnabled;

    // Enclopedia
    public bool[] isItemRevealed;

    // Upgradables
    public int[] upgradeCounts;

    // Best Score Data
    public int[] bestScore;

    // Slot Data
    public GameModes[] gameModes;
    public int[] coins;
    public int[] days;
    public DateTime[] dateTimes;

    // Game Settings
    public float bgmVolume;
    public float sfxVolume;
    public Languages lastLanguageSetting;
    public MerchantRanks MerchantRank;

    public BaseSaveDataV1()
    {
        Version = 1;
    }

    public override BaseSaveData VersionUp()
    {
        throw new System.NotImplementedException();
    }
}