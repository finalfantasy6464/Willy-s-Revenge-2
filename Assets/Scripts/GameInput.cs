using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;

public static class GameInput
{
    //Currently, holding a direction is not respected (i.e; holding 'up' then pressing 'left' goes left but letting go does not return to 'up') However,
    //Going from vertical to Horizontal directions does work as desired. Commented code below does not change outcome. Playable but not enjoyable.

    //Also for love of christ, please find a way to remove thousands of null checks per minute <3

    public static int Vertical;
    public static int Horizontal;
    public static int VerticalCache;
    public static int HorizontalCache;

    public static Dictionary<string, Func<bool>> InputMapPressedDown = new Dictionary<string, Func<bool>>
    {
        { "left", () => GetDirectionDown(Gamepad.current != null ? Gamepad.current.leftStick.left : null, Gamepad.current != null ? Gamepad.current.dpad.left : null, Keyboard.current != null ? Keyboard.current.leftArrowKey : null, Keyboard.current != null ? Keyboard.current.aKey : null)},
        { "up",   () => GetDirectionDown(Gamepad.current != null ? Gamepad.current.leftStick.up : null, Gamepad.current != null ? Gamepad.current.dpad.up : null, Keyboard.current != null ? Keyboard.current.upArrowKey : null, Keyboard.current != null ? Keyboard.current.wKey : null)},
        { "right",() => GetDirectionDown(Gamepad.current != null ? Gamepad.current.leftStick.right : null, Gamepad.current != null ? Gamepad.current.dpad.right : null, Keyboard.current != null ? Keyboard.current.rightArrowKey : null, Keyboard.current != null ? Keyboard.current.dKey : null)},
        { "down", () => GetDirectionDown(Gamepad.current != null ? Gamepad.current.leftStick.down : null, Gamepad.current != null ? Gamepad.current.dpad.down : null, Keyboard.current != null ? Keyboard.current.downArrowKey : null, Keyboard.current != null ? Keyboard.current.sKey : null)},

        { "select", () => GetActionDown(Gamepad.current != null ? Gamepad.current.buttonSouth : null, Keyboard.current != null ? Keyboard.current.enterKey : null, Keyboard.current != null ? Keyboard.current.spaceKey : null)},
        { "cancel", () => GetActionDown(Gamepad.current != null ? Gamepad.current.buttonEast : null, Keyboard.current != null ? Keyboard.current.backspaceKey : null, Keyboard.current != null ? Keyboard.current.deleteKey : null)},
        { "pause",  () => GetActionDown(Gamepad.current != null ? Gamepad.current.startButton : null, Keyboard.current != null ? Keyboard.current.escapeKey : null, Keyboard.current != null ? Keyboard.current.pauseKey : null)},
        { "reset",  () => GetActionDown(Gamepad.current != null ? Gamepad.current.buttonNorth : null, Keyboard.current != null ? Keyboard.current.rKey : null, null)}
    };

    public static Dictionary<string, Func<bool>> InputMapPressed = new Dictionary<string, Func<bool>>
    {
        { "left", () => GetDirection(Gamepad.current != null ? Gamepad.current.leftStick.left : null, Gamepad.current != null ? Gamepad.current.dpad.left : null, Keyboard.current != null ? Keyboard.current.leftArrowKey : null, Keyboard.current != null ? Keyboard.current.aKey : null)},
        { "up",   () => GetDirection(Gamepad.current != null ? Gamepad.current.leftStick.up : null, Gamepad.current != null ? Gamepad.current.dpad.up : null, Keyboard.current != null ? Keyboard.current.upArrowKey : null, Keyboard.current != null ? Keyboard.current.wKey : null)},
        { "right",() => GetDirection(Gamepad.current != null ? Gamepad.current.leftStick.right : null, Gamepad.current != null ? Gamepad.current.dpad.right : null, Keyboard.current != null ? Keyboard.current.rightArrowKey : null, Keyboard.current != null ? Keyboard.current.dKey : null)},
        { "down", () => GetDirection(Gamepad.current != null ? Gamepad.current.leftStick.down : null, Gamepad.current != null ? Gamepad.current.dpad.down : null, Keyboard.current != null ? Keyboard.current.downArrowKey : null, Keyboard.current != null ? Keyboard.current.sKey : null)},

        { "select", () => GetAction(Gamepad.current != null ? Gamepad.current.buttonSouth : null, Keyboard.current != null ? Keyboard.current.enterKey : null, Keyboard.current != null ? Keyboard.current.spaceKey : null)},
        { "cancel", () => GetAction(Gamepad.current != null ? Gamepad.current.buttonEast : null, Keyboard.current != null ? Keyboard.current.backspaceKey : null, Keyboard.current != null ? Keyboard.current.deleteKey : null)},
        { "pause",  () => GetAction(Gamepad.current != null ? Gamepad.current.startButton : null, Keyboard.current != null ? Keyboard.current.escapeKey : null, Keyboard.current != null ? Keyboard.current.pauseKey : null)},
        { "reset",  () => GetAction(Gamepad.current != null ? Gamepad.current.buttonNorth : null, Keyboard.current != null ? Keyboard.current.rKey : null, null)}
    };

    private static bool GetDirectionDown(ButtonControl stick, ButtonControl dPad, KeyControl arrow, KeyControl alt)
    {
        return
        stick != null && stick.wasPressedThisFrame ||
        dPad != null && dPad.wasPressedThisFrame ||
        arrow != null && arrow.wasPressedThisFrame ||
        alt != null && alt.wasPressedThisFrame;
    }

    private static bool GetDirection(ButtonControl stick, ButtonControl dPad, KeyControl arrow, KeyControl alt)
    {
        return
        stick != null && stick.isPressed ||
        dPad != null && dPad.isPressed ||
        arrow != null && arrow.isPressed ||
        alt != null && alt.isPressed;
    }

    private static bool GetActionDown(ButtonControl padInput, KeyControl keyboardInput, KeyControl altkeyboardInput)
    {
        return
        padInput != null && padInput.wasPressedThisFrame ||
        keyboardInput != null && keyboardInput.wasPressedThisFrame ||
        altkeyboardInput != null && altkeyboardInput.wasPressedThisFrame;
    }

    private static bool GetAction(ButtonControl padInput, KeyControl keyboardInput, KeyControl altkeyboardInput)
    {
        return
        padInput != null && padInput.isPressed ||
        keyboardInput != null && keyboardInput.isPressed ||
        altkeyboardInput != null && altkeyboardInput.isPressed;
    }

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

        //Debug.Log($"Vertical : {Vertical}, VCache : {VerticalCache}, Horizontal : {Horizontal}, HCache : {HorizontalCache}");
    }



    public static bool GetKeyDown(string keyName)
    {
        if(!IsKeyValidDown(keyName))
            return false;

        return InputMapPressedDown[keyName]();

        /*else if(Array.Exists(directionalKeys, n => n.Equals(keyName)))
        {
            return (keyName == "up" && Vertical > 0 && VerticalCache <= 0)
                || (keyName == "left" && Horizontal < 0 && HorizontalCache >= 0)
                || (keyName == "right" && Horizontal > 0 && HorizontalCache <= 0)
                || (keyName == "down" && Vertical < 0 && VerticalCache >= 0);
        }
        return false;*/
    }

    public static bool GetKey(string keyName)
    {
        if (!IsKeyValid(keyName))
            return false;

        return InputMapPressed[keyName]();

        /*if (InputMapPressed[keyName]())
            return true;

        else if (Array.Exists(directionalKeys, n => n.Equals(keyName)))
        {
            return (keyName == "up" && (Vertical > 0))
                || (keyName == "left" && (Horizontal < 0))
                || (keyName == "right" && (Horizontal > 0))
                || (keyName == "down" && (Vertical < 0));
        }
        return false;*/
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

    static bool IsKeyValidDown(string keyName)
    {
        if(InputMapPressedDown.ContainsKey(keyName))
            return true;
        else
        {
            Debug.LogError("No Input Detected");
            return false;
        }
    }

    static bool IsKeyValid(string keyName)
    {
        if (InputMapPressed.ContainsKey(keyName))
            return true;
        else
        {
            Debug.LogError("No Input Detected");
            return false;
        }
    }
}
