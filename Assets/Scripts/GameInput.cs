using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GameInput
{
    public static int Vertical;
    public static int Horizontal;
    public static int VerticalCache;
    public static int HorizontalCache;

    static Dictionary<string, KeyCode[]> keyCodeMap = new Dictionary<string, KeyCode[]>
    {
        { "left",   new KeyCode[] {KeyCode.LeftArrow, KeyCode.A} },
        { "up",     new KeyCode[] {KeyCode.UpArrow, KeyCode.W} },
        { "right",  new KeyCode[] {KeyCode.RightArrow, KeyCode.D} },
        { "down",   new KeyCode[] {KeyCode.DownArrow, KeyCode.S} },
        { "select", new KeyCode[] {KeyCode.JoystickButton0, KeyCode.Space, KeyCode.Return, KeyCode.KeypadEnter} },
        { "cancel", new KeyCode[] {KeyCode.JoystickButton1, KeyCode.Backspace} },
        { "pause",  new KeyCode[] {KeyCode.JoystickButton7, KeyCode.Escape} },
        { "reset",  new KeyCode[] {KeyCode.JoystickButton3, KeyCode.R}}
    };

    static string[] directionalKeys = new string[]
    {
        "left", "up", "right", "down"
    };



    public static void Update()
    {
        VerticalCache = Vertical;
        HorizontalCache = Horizontal;

        if (GetKey("up"))
            Vertical = 1;
        else if (GetKey("down"))
            Vertical = -1;
        else
            Vertical = 0;

        if (GetKey("right"))
            Horizontal = 1;
        else if (GetKey("left"))
            Horizontal = -1;
        else
            Horizontal = 0;
    }

    public static bool GetKey(string keyName) 
    {
        if(!IsKeyValid(keyName))
            return false;

        if(AnyInState(keyCodeMap[keyName], Input.GetKey))
            return true;
        else if(Array.Exists(directionalKeys, n => n.Equals(keyName)))
        {
            return (keyName == "up"    && (Vertical > 0))
                || (keyName == "left"  && (Horizontal < 0))
                || (keyName == "right" && (Horizontal > 0))
                || (keyName == "down"  && (Vertical < 0));
        }
        return false;
    }

    public static bool GetKeyDown(string keyName)
    {
        if(!IsKeyValid(keyName))
            return false;

        if(AnyInState(keyCodeMap[keyName], Input.GetKeyDown))
            return true;
        else if(Array.Exists(directionalKeys, n => n.Equals(keyName)))
        {
            return (keyName == "up"    && (Vertical > 0 && VerticalCache <= 0))
                || (keyName == "left"  && (Horizontal < 0 && HorizontalCache >= 0))
                || (keyName == "right" && (Horizontal > 0 && HorizontalCache <= 0))
                || (keyName == "down"  && (Vertical < 0 && VerticalCache >= 0));
        }
        return false;
    }

    public static bool GetKeyUp(string keyName) 
    {
        if(!IsKeyValid(keyName))
            return false;

        if(AnyInState(keyCodeMap[keyName], Input.GetKeyUp))
            return true;
        else if(Array.Exists(directionalKeys, n => n.Equals(keyName)))
        {
            return (keyName == "up"    && (VerticalCache > 0 && Vertical <= 0))
                || (keyName == "left"  && (HorizontalCache < 0 && Horizontal >= 0))
                || (keyName == "right" && (HorizontalCache > 0 && Horizontal <= 0))
                || (keyName == "down"  && (VerticalCache < 0 && Vertical >= 0));
        }
        return false;
    }

    static bool AnyInState(KeyCode[] keys, Func<KeyCode, bool> stateMethod)
    {
        foreach(KeyCode key in keys)
        {
            if(stateMethod(key))
                return true;
        }
        return false;
    }

    static bool IsKeyValid(string keyName)
    {
        if(keyCodeMap.ContainsKey(keyName))
            return true;
        else
        {
            Debug.LogError("No Input Detected");
            return false;
        }
    }
}
