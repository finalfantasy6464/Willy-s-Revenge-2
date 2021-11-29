using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableGameState : ScriptableObject
{
    public static string AUTOSAVE_PATH;
    public static string[] MANUAL_SAVE_PATHS;

    public int saveTime;
    public int complete;
    public int golden;
    public int timer;

    public int arenaScore;
    public int characterSkinIndex;

    [Header("Camera")]
    public Vector3 savedCameraPosition;
    public float savedOrtographicSize;
    public OverworldProgressView progressView;

    [Header("World")]
    public float completionPercent;
    public Vector3 savedPinPosition;
    public Vector3 AutosavePosition;

    public List<bool> completedlevels = new List<bool>();
    public List<bool> goldenpellets = new List<bool>();
    public List<bool> timerchallenge = new List<bool>();
    public List<bool> lockedgates = new List<bool>();
    public List<bool> destroyedgates = new List<bool>();
    public List<bool> lockedgatescache = new List<bool>();
    public List<bool> destroyedgatescache = new List<bool>();

    void Awake()
    {
        AUTOSAVE_PATH = Application.persistentDataPath + "/Save_Auto.wr2";
        MANUAL_SAVE_PATHS = new string[]
        {
            Application.persistentDataPath + "/Save_0.wr2",
            Application.persistentDataPath + "/Save_1.wr2",
            Application.persistentDataPath + "/Save_2.wr2"
        };
    }

    public void SaveToFile(int slot)
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(MANUAL_SAVE_PATHS[slot], FileMode.Create)))
        {
            writer.Write(saveTime);
            writer.Write(complete);
            writer.Write(golden);
            writer.Write(timer);
            writer.Write(arenaScore);

            WriteBoolList(writer, completedlevels);
            WriteBoolList(writer, goldenpellets);
            WriteBoolList(writer, timerchallenge);
            WriteBoolList(writer, lockedgates);
            WriteBoolList(writer, destroyedgates);
            WriteBoolList(writer, lockedgatescache);
            WriteBoolList(writer, destroyedgatescache);

            writer.Write(characterSkinIndex);
            writer.Write(savedOrtographicSize);
            writer.Write((Int32)progressView);
            writer.Write(completionPercent);
            
            writer.Write(savedCameraPosition.x);
            writer.Write(savedCameraPosition.y);
            writer.Write(savedCameraPosition.z);
            
            writer.Write(savedPinPosition.x);
            writer.Write(savedPinPosition.y);
            writer.Write(savedPinPosition.z);
            
            writer.Write(AutosavePosition.x);
            writer.Write(AutosavePosition.y);
            writer.Write(AutosavePosition.z);
        }
    }

    public void LoadFromFile(int slot)
    {
        if(!File.Exists(MANUAL_SAVE_PATHS[slot]))
        {
            Debug.LogWarning("Load path did not exist. Aborting");
            return;
        }

        using (BinaryReader reader = new BinaryReader(File.Open(MANUAL_SAVE_PATHS[slot], FileMode.Open)))
        {
            saveTime = reader.ReadInt32();
            complete = reader.ReadInt32();
            golden   = reader.ReadInt32();
            timer    = reader.ReadInt32();
            arenaScore = reader.ReadInt32();

            completedlevels = ReadBoolList(reader, completedlevels.Count);
            goldenpellets   = ReadBoolList(reader, goldenpellets.Count);
            timerchallenge  = ReadBoolList(reader, timerchallenge.Count);
            lockedgates     = ReadBoolList(reader, lockedgates.Count);
            destroyedgates  = ReadBoolList(reader, destroyedgates.Count);
            lockedgatescache = ReadBoolList(reader, lockedgates.Count);
            destroyedgatescache = ReadBoolList(reader, destroyedgates.Count);

            characterSkinIndex = reader.ReadInt32();
            savedOrtographicSize = reader.ReadSingle();
            progressView = (OverworldProgressView)reader.ReadInt32();
            completionPercent = reader.ReadSingle();

            savedCameraPosition = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            savedPinPosition    = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            AutosavePosition    = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }

    List<bool> ReadBoolList(BinaryReader reader, int listSize)
    {
        List<bool> cacheList = new List<bool>();
        for (int i = 0; i < listSize; i++)
            cacheList.Add(reader.ReadBoolean());
        
        return cacheList;
    }

    void WriteBoolList(BinaryWriter writer, List<bool> list)
    {
        foreach (bool value in list)
            writer.Write(value);
    }
}