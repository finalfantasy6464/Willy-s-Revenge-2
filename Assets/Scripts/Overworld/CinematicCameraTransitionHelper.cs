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

    public void UpdateBackgroundColor(int index)
    {
        Color NewColor = new Color();
        if(index == 0)
        {
            NewColor = new Color(0.8745098f, 0.8823529f, 0.9803922f, 1);
        }
        else if(index == 1)
        {
            NewColor = new Color(1f, 0.806803f, 0.4292453f, 1);
        }
        else if(index == 2)
        {
            NewColor = new Color(0.1843137f, 0.2f, 0.3294118f, 1);
        }
        else if(index == 3)
        {
            NewColor = new Color(0f,0f,0f,1f);
        }
        cinematicCamera.backgroundColor = NewColor;
        followCamera.overworldCamera.backgroundColor = NewColor;
        GameControl.control.backgroundColor = NewColor;
    }
}
