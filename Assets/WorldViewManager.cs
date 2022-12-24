using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldViewManager : MonoBehaviour
{
    public List<GameObject> EnabledObjects = new List<GameObject>();
    public List<GameObject> DisabledObjects = new List<GameObject>();

    public GameObject WorldLeft;
    public GameObject WorldRight;
    public GameObject BlurWorld;
    public GameObject[] darkClouds;
    public GameObject Clouds;
    public GameObject Moon;
    public GameObject UFO;

    public void UpdateDrawnObjects(int index)
    {
        EnabledObjects.Clear();
        DisabledObjects.Clear();

        if(index == (int)overworldProgressUpdate.WorldViews.WorldLeft)
        {
            EnabledObjects.Add(WorldLeft);
            DisabledObjects.Add(WorldRight);
            DisabledObjects.Add(BlurWorld);
            DisabledObjects.AddRange(darkClouds);
            DisabledObjects.Add(Clouds);
            DisabledObjects.Add(Moon);
            DisabledObjects.Add(UFO);
        }
        else if(index == (int)overworldProgressUpdate.WorldViews.WorldRight)
        {
            EnabledObjects.Add(WorldRight);
            EnabledObjects.AddRange(darkClouds);
            DisabledObjects.Add(WorldLeft);
            DisabledObjects.Add(BlurWorld);
            DisabledObjects.Add(Clouds);
            DisabledObjects.Add(Moon);
            DisabledObjects.Add(UFO);
        }
        else if(index == (int)overworldProgressUpdate.WorldViews.Clouds)
        {
            EnabledObjects.Add(Clouds);
            EnabledObjects.Add(BlurWorld);
            DisabledObjects.Add(WorldLeft);
            DisabledObjects.Add(WorldRight);
            DisabledObjects.AddRange(darkClouds);
            DisabledObjects.Add(Moon);
            DisabledObjects.Add(UFO);
        }
        else if(index == (int)overworldProgressUpdate.WorldViews.Moon)
        {
            EnabledObjects.Add(Moon);
            DisabledObjects.Add(WorldLeft);
            DisabledObjects.Add(WorldRight);
            DisabledObjects.Add(BlurWorld);
            DisabledObjects.AddRange(darkClouds);
            DisabledObjects.Add(Clouds);
            DisabledObjects.Add(UFO);
        }
        else if(index == (int)overworldProgressUpdate.WorldViews.UFO)
        {
            EnabledObjects.Add(UFO);
            DisabledObjects.Add(WorldLeft);
            DisabledObjects.Add(WorldRight);
            DisabledObjects.Add(BlurWorld);
            DisabledObjects.AddRange(darkClouds);
            DisabledObjects.Add(Clouds);
            DisabledObjects.Add(Moon);
        }

        foreach (GameObject obj in EnabledObjects)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in DisabledObjects)
        {
            obj.SetActive(false);
        }
    }
}
