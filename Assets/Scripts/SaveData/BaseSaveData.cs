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
    public int bestScore;
    public int diamonds;
    public GameModes[] gameModes;
    public int[] coins;
    public int[] days;
    public DateTime[] dateTimes;

    public BaseSaveDataV1()
    {
        Version = 1;
    }

    public override BaseSaveData VersionUp()
    {
        throw new System.NotImplementedException();
    }
}