using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class OverworldCamera : MonoBehaviour
{
    public Camera gameCamera;
    public OverworldCharacter character;
    public Checkpoint checkpointA;
    public Checkpoint checkpointB;
    public Checkpoint[] checkpoints;
    public ProgressSetting progressCalculation;
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

    void Start()
    {
        SetFromPin(character.currentPin);
    }

    void Update()
    {
        SetFromPin(character.currentPin);
    }

    public void SetCheckpoints(Checkpoint a, Checkpoint b)
    {
        checkpointA = a;
        checkpointB = b;
    }

    public void SetFromPin(NavigationPin pin)
    {
        for (int i = 0; i < checkpoints.Length - 1; i++)
        {
            if ((pin is GatePin && pin.transform == checkpoints[i].anchor)
                    || (pin is LevelPin level && level.previousGate == checkpoints[i].anchor))
            {
                Checkpoint a = checkpoints[i];
                Checkpoint b = checkpoints[i + 1];
                float pinProgress = 0f;
                
                if(pin is LevelPin)
                    pinProgress = GetLevelPinProgress(a, b);

                SetCheckpoints(checkpoints[i], checkpoints[i + 1]);
                SetProgress(pinProgress);
                UpdateCamera();
                return;
            }
        }
        Debug.LogError("Previous GatePin was not found. Is it included in your Checkpoints?");
    }

    private float GetLevelPinProgress(Checkpoint a, Checkpoint b)
    {
        if(progressCalculation == ProgressSetting.AnchorDistance)
            return Mathf.Lerp(0, 1, Vector3InverseLerp(
                    a.anchor.position, b.anchor.position, character.transform.position));
        else if(progressCalculation == ProgressSetting.CheckpointDistance)
            return Mathf.Lerp(0, 1, Vector3InverseLerp(a.rect.center, b.rect.center, character.transform.position));
        else if(progressCalculation == ProgressSetting.PinOrder)
        {
            float aIndex = (float)character.currentPin.transform.GetSiblingIndex();
            float bIndex = 0f;
            if(character.targetPin != null)
                bIndex = (float)character.targetPin.transform.GetSiblingIndex();
            return Mathf.Lerp(aIndex / 10f, bIndex / 10f, character.currentPathTime);
        }
        else // ProgressSetting.CustomSteps
        {
            int aIndex = character.currentPin.transform.GetSiblingIndex();
            int bIndex = 0;
            if(character.targetPin != null)
                bIndex = character.targetPin.transform.GetSiblingIndex();
            return Mathf.Lerp(a.customSteps[aIndex],
                    a.customSteps[bIndex], character.currentPathTime);
        }
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
                checkpointA.rect.center, checkpointB.rect.center, progress);
        newPosition.z = gameCamera.transform.position.z;
        gameCamera.transform.position = newPosition;
        gameCamera.orthographicSize = Mathf.Lerp(
                checkpointA.orthographicSize, checkpointB.orthographicSize, progress);
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
        public float[] customSteps;
    }
}
