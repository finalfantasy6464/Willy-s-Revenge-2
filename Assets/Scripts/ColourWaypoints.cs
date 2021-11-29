using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourWaypoints : MonoBehaviour
{
    public Transform checkStart;
    public Transform checkEnd;

    public int progressIndex;

    public OverworldCharacter character;

    public BackgroundColourController bgController;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if(!enabled) return;
        if (coll.CompareTag("Player"))
        {
            WaypointBehaviour();
        }
    }

    public void WaypointBehaviour()
    {
        bgController.progressIndex = progressIndex;
        bgController.SetVectorTransforms(checkStart, checkEnd);
    }
}
