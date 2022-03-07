using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaInstantRetry : MonoBehaviour
{
    public Button playButton;

    public void Retry()
    {
        playButton.onClick.Invoke();
    }
}
