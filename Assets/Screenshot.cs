using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Screenshot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot();
        }
    }

    public void TakeScreenshot()
    {
        string timeNow = DateTime.Now.ToString("dd-MMMM-yyyy HHmmss");

        ScreenCapture.CaptureScreenshot("Assets/LevelPreviews/LevelPreview.png");
    }
}
