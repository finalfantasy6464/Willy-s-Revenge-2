using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class OverworldCamera : MonoBehaviour
{
    public Camera gameCamera;
    public OverworldViewToggler viewToggler;
    public BackgroundColourController bgColour;
    public OverworldCharacter character;
    public ProgressSetting progressCalculation;
    public Checkpoint currentCheckpointA;
    public Checkpoint currentCheckpointB;
    public Checkpoint[] checkpoints;
    public Transform[] checkpointSteps;

    GUIStyle debugStyle;
    float progress;
    float lastProgress;

    public enum ProgressSetting
    {
        AnchorDistance,
        CheckpointDistance,
        PinOrder,
        CustomSteps
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(debugStyle == null)
            debugStyle = new GUIStyle();
            
        if(checkpoints == null || checkpoints.Length == 0) return;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if(checkpoint.hideDebug)
                continue;

            debugStyle.normal.textColor = checkpoint.color;
            Handles.color = checkpoint.color;
            Handles.Label(checkpoint.bottomLeft, checkpoint.anchor?.name, debugStyle);
            Handles.DrawDottedLine(checkpoint.xCenterLeft, checkpoint.xCenterRight, 0.5f);
            Handles.DrawDottedLine(checkpoint.yCenterBottom, checkpoint.yCenterTop, 0.5f);
            Gizmos.color = checkpoint.color;
            Gizmos.DrawWireCube(checkpoint.rect.center, checkpoint.rect.size);
        }
    }
    #endif

    void Update()
    {
        SetFromPin(character.currentPin);
    }

    public void SetCheckpoints(Checkpoint a, Checkpoint b)
    {
        currentCheckpointA = a;
        currentCheckpointB = b;
    }

    public void SetFromSaved(Vector3 position, float ortographicSize, Color backgroundColor)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        gameCamera.orthographicSize = ortographicSize;
        gameCamera.backgroundColor = backgroundColor;
        viewToggler.Set(GameControl.control.progressView);
    }

    public void SetFromPin(NavigationPin pin)
    {
        Checkpoint a = null;
        Checkpoint b = null;
        int aIndex = 0;
        int bIndex = 0;
        int pinIndex = 0;

        for (int i = 0; i < checkpointSteps.Length - 1; i++)
        {
            if(IsCheckpointAnchor(checkpointSteps[i], out Checkpoint previousCheckpoint))
            {
                a = previousCheckpoint;
                aIndex = i;
            }

            if(checkpointSteps[i] == pin.transform)
            {
                pinIndex = i;
                int j = i;
                while(j < checkpointSteps.Length)
                {
                    if(IsCheckpointAnchor(checkpointSteps[j],
                            out Checkpoint nextCheckpoint))
                    {
                        b = nextCheckpoint;
                        bIndex = j;
                        break;
                    }
                    j++;
                }

                if(a == null || b == null)
                {
                    Debug.LogError("Closest Checkpoint was never found, aborting.");
                    return;
                }
                break;
            }
        }
            
        SetCheckpoints(a, b);
        float currentProgress = Mathf.InverseLerp(aIndex, bIndex, GetStepIndex(character.currentPin.transform));
        float targetProgress = character.targetPin == null ? currentProgress :
                Mathf.InverseLerp(aIndex, bIndex, GetStepIndex(character.targetPin.transform));
        SetProgress(Mathf.Lerp(currentProgress, targetProgress, character.currentPathTime));
        UpdateCamera();
        return;
    }

    public int GetStepIndex(Transform t)
    {
        for (int i = 0; i < checkpointSteps.Length; i++)
        {
            if(checkpointSteps[i] == t)
                return i;
        }
        return -1;
    }


    bool IsCheckpointAnchor(Transform t, out Checkpoint result)
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if(checkpoint.anchor == t)
            {
                result = checkpoint;
                return true;
            }
        } 
        result = null;
        return false;
    }

    public void SetCamera(Camera c)
    {
        gameCamera = c;
    }

    public void SetProgress(float value)
    {
        lastProgress = progress;
        progress = value;   
    }

    public void UpdateCamera()
    {
        Vector3 newPosition = Vector2.Lerp(
                currentCheckpointA.rect.center, currentCheckpointB.rect.center, progress);
        newPosition.z = gameCamera.transform.position.z;
        gameCamera.transform.position = newPosition;
        gameCamera.orthographicSize = Mathf.Lerp(
                currentCheckpointA.orthographicSize, currentCheckpointB.orthographicSize, progress);
    }


    float Vector3InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    [System.Serializable, ExecuteAlways]
    public class Checkpoint
    {
        public bool hideDebug;
        public Transform anchor;
        public Color color;
        public Vector2 bottomLeft;
        public float size;
        public Vector2 xCenterLeft => new Vector2(rect.xMin, (rect.yMax + rect.yMin) * 0.5f);
        public Vector2 xCenterRight => xCenterLeft + Vector2.right * rect.width;
        public Vector2 yCenterBottom => new Vector2((rect.xMin + rect.xMax) * 0.5f, rect.yMin);
        public Vector2 yCenterTop => yCenterBottom + Vector2.up * rect.height;
        public Rect rect => new Rect(bottomLeft.x, bottomLeft.y, size * 1.6f, size * 0.9f);
        public float orthographicSize => rect.yMax - rect.center.y;
    }
}
