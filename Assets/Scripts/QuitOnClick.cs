using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour
{
    public void QuitByClick()
    {
        StartCoroutine(QuittingRoutine());
    }

    IEnumerator QuittingRoutine()
    {
        yield return null;
        GameControl.control.AutoSave();
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
