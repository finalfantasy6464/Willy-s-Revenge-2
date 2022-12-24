using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePreview
{
    public string formattedSaveTime;
    public int arenaScore;
    public float completedPercent;
    public float goldenPercent;
    public float challengePercent;
    public float destroyedPercent;
    public bool isEmpty;

    public GameStatePreview(int slot)
    {
        if(!File.Exists(ScriptableGameState.MANUAL_SAVE_PATHS[slot]))
        {
            isEmpty = true;
            return;
        }

        using (BinaryReader reader = new BinaryReader(File.Open(ScriptableGameState.MANUAL_SAVE_PATHS[slot], FileMode.Open)))
        {
            SetSaveTime(reader.ReadInt32()); // Save Time
            reader.ReadInt32();              // Complete
            reader.ReadInt32();              // Golden
            reader.ReadInt32();              // Timer
            arenaScore = reader.ReadInt32(); 

            completedPercent = GetPercent(reader, 102, 100);
            goldenPercent = GetPercent(reader, 102, 100);
            challengePercent = GetPercent(reader, 102, 100);
            destroyedPercent = GetGatePercent(reader);
        }
    }

    float GetPercent(BinaryReader reader, int listSize, int denominator)
    {
        List<bool> cacheList = new List<bool>();
        int fulfilled = 0;
        for (int i = 0; i < listSize; i++)
        {
            bool current = reader.ReadBoolean();
            cacheList.Add(current);
            if(current) fulfilled++;
        }
        return (float)fulfilled / (float)denominator;
    }

    float GetGatePercent(BinaryReader reader)
    {
        int destroyed = 0;
        const int gateAmount = 9;
        for (int i = 0; i < gateAmount; i++)
        {
            if(reader.ReadBoolean())
                destroyed++;
        }
        return (float) destroyed / (float)gateAmount;
    }

    

    void SetSaveTime(int epochTime)
    {
        formattedSaveTime = DateTimeOffset.FromUnixTimeSeconds(epochTime).ToString("MM/dd/yyyy HH:mm:ss");
    }
}