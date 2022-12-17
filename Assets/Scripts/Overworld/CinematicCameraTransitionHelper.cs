using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraTransitionHelper : MonoBehaviour
{
    public WorldTransitionPressurePlate[] plates;
    public WorldTransitionPressurePlate activePlate;
    public OverworldFollowCamera followCamera;
    public Camera cinematicCamera;
    public OverworldPlayer player;

    public void SetStateName(string name)
    {
        foreach (WorldTransitionPressurePlate plate in plates)
        {
            if(plate.AnimationTriggerName == name)
            {
                activePlate = plate;
                break;
            }
        }
        name = "";
    }

    public void OnPlateAnimationEnd()
    {
        followCamera.SetCameraInstant(cinematicCamera.transform.position,
                cinematicCamera.transform.position, cinematicCamera.orthographicSize);
    }

    public void UpdateCharacterSkin(int skinIndex)
    {
        GameControl.control.currentCharacterSprite = skinIndex;
        player.UpdateCharacterSprite();
    }

    public void UpdateCharacterRotation(float z)
    {
        player.lastLookRotation = Quaternion.Euler(0,0,z);
    }
}
