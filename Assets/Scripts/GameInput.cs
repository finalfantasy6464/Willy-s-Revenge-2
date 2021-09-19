using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInput
{
    public static int Vertical;
    public static int Horizontal;
    public static int VerticalCache;
    public static int HorizontalCache;


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

    public static bool GetKey(string keyname) 
    {
        if(keyname == "up")
        {
            return Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("DpadY") > 0 
                    || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        }
        else if (keyname == "left")
        {
            return Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("DpadX") < 0
                    || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        }
        else if (keyname == "right")
        {
            return Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("DpadX") > 0
                    || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        }
        else if (keyname == "down")
        {
            return Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("DpadY") < 0
                    || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        }
        else if (keyname == "select")
        {
            return Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return);
        }
        else if (keyname == "cancel")
        {
            return Input.GetKey(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.Backspace);
        }
        else if (keyname == "pause")
        {
            return Input.GetKey(KeyCode.JoystickButton7) || Input.GetKey(KeyCode.Escape);
        }
        else
        {
            Debug.LogError("No Input Detected");
            return false;
        }
    }
    public static bool GetKeyDown(string keyname)
    {
        if (keyname == "up")
        {
            return (Vertical > 0 && VerticalCache <= 0)
                    || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        }
        else if (keyname == "left")
        {
            return (Horizontal < 0 && HorizontalCache >= 0)
                    || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        }
        else if (keyname == "right")
        {
            return (Horizontal > 0 && HorizontalCache <= 0)
                    || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        }
        else if (keyname == "down")
        {
            return (Vertical < 0 && VerticalCache >= 0)
                    || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
        }
        else if (keyname == "select")
        {
            return Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
        }
        else if (keyname == "cancel")
        {
            return Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Backspace);
        }
        else if (keyname == "pause")
        {
            return Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Escape);
        }
        else
        {
            Debug.LogError("No Input Detected");
            return false;
        }
    }
    public static bool GetKeyUp(string keyname) 
    {
        if (keyname == "up")
        {
            return (VerticalCache > 0 && Vertical <= 0)
                    || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W);
        }
        else if (keyname == "left")
        {
            return (HorizontalCache < 0 && Horizontal >= 0)
                    || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);
        }
        else if (keyname == "right")
        {
            return (HorizontalCache > 0 && Horizontal <= 0)
                    || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);
        }
        else if (keyname == "down")
        {
            return (VerticalCache < 0 && Vertical >= 0)
                    || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S);
        }
        else if (keyname == "select")
        {
            return Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return);
        }
        else if (keyname == "cancel")
        {
            return Input.GetKeyUp(KeyCode.JoystickButton1) || Input.GetKeyUp(KeyCode.Backspace);
        }
        else if (keyname == "pause")
        {
            return Input.GetKeyUp(KeyCode.JoystickButton7) || Input.GetKeyUp(KeyCode.Escape);
        }
        else
        {
            Debug.LogError("No Input Detected");
            return false;
        }
    }
}
