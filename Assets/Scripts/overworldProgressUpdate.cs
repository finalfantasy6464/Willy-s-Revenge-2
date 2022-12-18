using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overworldProgressUpdate : MonoBehaviour
{
    public OverworldMusicSelector overworldMusic;
    public WorldViewManager worldview;

    public void SetOverworldMusicProgress(int index)
    {
        GameControl.control.overworldMusicProgress = index;
        overworldMusic.currentProgress = index;
        overworldMusic.overworldmusicCheck();
    }

    public void UpdateWorldView(int index)
    {
        GameControl.control.currentWorldView = index;
        worldview.UpdateDrawnObjects(index);
    }
}
