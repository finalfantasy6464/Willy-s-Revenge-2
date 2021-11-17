using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
public class CameraProgressPreview : MonoBehaviour
{
    public OverworldCamera cameraPath;
    [Range(0, 20)]public int checkpointAIndex;
    [Range(0, 20)]public int checkpointBIndex;
    [Range(0f, 1f)] public float progress;
    OverworldCamera.Checkpoint preview;
    OverworldCamera.Checkpoint a;
    OverworldCamera.Checkpoint b;
    
    void OnDrawGizmos()
    {
        if(preview == null) return;
        Handles.Label(preview.bottomLeft, $"Preview (t = {progress})");
        Handles.DrawDottedLine(preview.xCenterLeft, preview.xCenterRight, 0.5f);
        Handles.DrawDottedLine(preview.yCenterBottom, preview.yCenterTop, 0.5f);
        Gizmos.DrawWireCube(preview.rect.center, preview.rect.size);
    }

    void OnValidate()
    {
        TryUpdatePreview();
    }

    public void TryUpdatePreview()
    {
        if(preview == null || cameraPath == null || checkpointAIndex > cameraPath.checkpoints.Length - 1
                || checkpointBIndex > cameraPath.checkpoints.Length - 1)
            return;

        a = cameraPath.checkpoints[checkpointAIndex];
        b = cameraPath.checkpoints[checkpointBIndex];

        preview.bottomLeft = Vector2.Lerp(a.bottomLeft, b.bottomLeft, progress);
        preview.size = Mathf.Lerp(a.size, b.size, progress);
    }
}
