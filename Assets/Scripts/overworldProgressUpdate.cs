using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overworldProgressUpdate : MonoBehaviour
{
    public OverworldMusicSelector overworldMusic;
    public WorldViewManager worldview;
    public WorldPlateToggleControl plateControl;

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
        plateControl.UpdatePlateState(index);
    }

    public enum WorldViews
    {
        WorldLeft,
        WorldRight,
        Clouds,
        Moon,
        UFO
    }
}
