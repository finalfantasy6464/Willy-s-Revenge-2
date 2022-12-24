using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlateToggleControl : MonoBehaviour
{
    public List<WorldTransitionPressurePlate> ActivePlates = new List<WorldTransitionPressurePlate>();
    public List<WorldTransitionPressurePlate> InactivePlates = new List<WorldTransitionPressurePlate>();

    public WorldTransitionPressurePlate[] LeftPlates;
    public WorldTransitionPressurePlate[] RightPlates;

    public WorldTransitionPressurePlate[] CloudPlates;
    public WorldTransitionPressurePlate[] MoonPlates;
    public WorldTransitionPressurePlate UFOPlate;

    public void UpdatePlateState(int index)
    {
        ActivePlates.Clear();
        InactivePlates.Clear();

        if(index == (int)overworldProgressUpdate.WorldViews.WorldLeft)
        {
            ActivePlates.AddRange(LeftPlates);
            InactivePlates.AddRange(RightPlates);
            InactivePlates.AddRange(CloudPlates);
            InactivePlates.AddRange(MoonPlates);
            InactivePlates.Add(UFOPlate);
        }
        else if (index == (int)overworldProgressUpdate.WorldViews.WorldRight)
        {
            ActivePlates.AddRange(RightPlates);
            InactivePlates.AddRange(LeftPlates);
            InactivePlates.AddRange(CloudPlates);
            InactivePlates.AddRange(MoonPlates);
            InactivePlates.Add(UFOPlate);
        }
        else if (index == (int)overworldProgressUpdate.WorldViews.Clouds)
        {
            ActivePlates.AddRange(CloudPlates);
            InactivePlates.AddRange(LeftPlates);
            InactivePlates.AddRange(RightPlates);
            InactivePlates.AddRange(MoonPlates);
            InactivePlates.Add(UFOPlate);
        }
        else if (index == (int)overworldProgressUpdate.WorldViews.Moon)
        {
            ActivePlates.AddRange(MoonPlates);
            InactivePlates.AddRange(LeftPlates);
            InactivePlates.AddRange(RightPlates);
            InactivePlates.AddRange(CloudPlates);
            InactivePlates.Add(UFOPlate);
        }
        else if(index == (int)overworldProgressUpdate.WorldViews.UFO)
        {
            ActivePlates.Add(UFOPlate);
            InactivePlates.AddRange(LeftPlates);
            InactivePlates.AddRange(RightPlates);
            InactivePlates.AddRange(CloudPlates);
            InactivePlates.AddRange(MoonPlates);
        }

        foreach(WorldTransitionPressurePlate plate in ActivePlates)
        {
            plate.worldPlateCollider.enabled = true;
            plate.worldPlateRenderer.enabled = true;
            plate.worldPlateLight.enabled = true;
            plate.worldPlateParticle.Play();
        }

        foreach (WorldTransitionPressurePlate plate in InactivePlates)
        {
            plate.worldPlateCollider.enabled = false;
            plate.worldPlateRenderer.enabled = false;
            plate.worldPlateLight.enabled = false;
            plate.worldPlateParticle.Stop();
        }
    }
}
