using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlAutoLoader : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(AutoLoadChecker());
    }
    IEnumerator AutoLoadChecker()
    {
        yield return null;
        GameControl.control.AutoLoadCheck();
        yield return null;
        BeginAutoloading();
    }

    void BeginAutoloading()
    {
        if (GameControl.control.autoloadSuccessful)
        {
            GameControl.control.LoadIntoOverworld();
        }
    }
}
