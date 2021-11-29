using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
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

    void OnEnable()
    {
        AUTOSAVE_PATH = Application.persistentDataPath + "/Save_Auto.wr2";
        MANUAL_SAVE_PATHS = new string[]
        {
            Application.persistentDataPath + "/Save_0.wr2",
            Application.persistentDataPath + "/Save_1.wr2",
            Application.persistentDataPath + "/Save_2.wr2"
        };
    }

    private void SaveToFile(string path)
    {
        saveTime = (int)DateTime.Now.Subtract(new DateTime(1970,1,1,0,0,0)).TotalSeconds;

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write((Int32)saveTime);
            writer.Write((Int32)complete);
            writer.Write((Int32)golden);
            writer.Write((Int32)timer);
            writer.Write((Int32)arenaScore);

            WriteBoolList(writer, completedlevels);
            WriteBoolList(writer, goldenpellets);
            WriteBoolList(writer, timerchallenge);
            WriteBoolList(writer, lockedgates);
            WriteBoolList(writer, destroyedgates);
            WriteBoolList(writer, lockedgatescache);
            WriteBoolList(writer, destroyedgatescache);

            writer.Write((Int32)characterSkinIndex);
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

    public void DeleteFile(string path)
    {
        if(File.Exists(path))
            File.Delete(path);
    }

    private bool TryLoadFromFile(string path)
    {
        if(!File.Exists(path))
        {
            Debug.LogWarning("Load path did not exist. Aborting");
            return false;
        }

        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
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
            lockedgatescache = ReadBoolList(reader, lockedgatescache.Count);
            destroyedgatescache = ReadBoolList(reader, destroyedgatescache.Count);

            characterSkinIndex = reader.ReadInt32();
            savedOrtographicSize = reader.ReadSingle();
            progressView = (OverworldProgressView)reader.ReadInt32();
            completionPercent = reader.ReadSingle();

            savedCameraPosition = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            savedPinPosition    = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            AutosavePosition    = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        return true;
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

    public void WriteToManual(int saveSlot)
    {
        SaveToFile(MANUAL_SAVE_PATHS[saveSlot]);
    }

    public void SetFromGameControl(GameControl control)
    {
        complete = control.complete;
        golden   = control.golden;
        timer    = control.timer;

        arenaScore = (int)control.ArenahighScore;
        completedlevels =  new List<bool>(completedlevels);
        goldenpellets   =  new List<bool>(goldenpellets);
        timerchallenge  =  new List<bool>(timerchallenge);
        lockedgates     =  new List<bool>(lockedgates);
        destroyedgates  =  new List<bool>(destroyedgates);
        lockedgatescache = new List<bool>(lockedgatescache);
        destroyedgatescache =   new List<bool>(destroyedgatescache);

        characterSkinIndex = control.currentCharacterSprite;
        savedOrtographicSize = control.savedOrtographicSize;
        progressView = control.progressView;
        completionPercent = control.completionPercent;

        savedCameraPosition = control.savedCameraPosition;
        savedPinPosition = control.savedPinPosition;
        AutosavePosition = control.AutosavePosition;
    }

    public void WriteToAuto()
    {
        SaveToFile(AUTOSAVE_PATH);
    }

    public bool SetFromManual(int saveSlot)
    {
        return TryLoadFromFile(MANUAL_SAVE_PATHS[saveSlot]);
    }

    public bool SetFromAuto()
    {
        return TryLoadFromFile(AUTOSAVE_PATH);
    }

    public void DeleteManualSave(int saveSlot)
    {
        throw new NotImplementedException();
    }
}