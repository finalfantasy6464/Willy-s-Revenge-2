using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColourController : MonoBehaviour
{
    public Camera mainCamera;
    public Gradient changeGradient;

    private float changeProgress = 0f;

    public int progressIndex;

    public OverworldCharacter character;

    public Vector3 checkStart;
    public Vector3 checkEnd;

    private void Start()
    {
        TrySetProgress(0.0f);
    }

    private void Update()
    {
        SetProgressFromIndex(progressIndex);
    }

    public float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    public void SetVectorTransforms(Transform start, Transform end)
    {
        checkStart = start.position;
        checkEnd = end.position;
    }

    public void TrySetProgress(float changeProgress)
    {
        float progress = (changeProgress / 3f) + progressIndex * 0.33f;

        if(this.changeProgress < progress)
        {
            mainCamera.backgroundColor = changeGradient.Evaluate(progress);
            this.changeProgress = progress; 
        }
    }

    public void SetProgressFromIndex(int index)
    {
        TrySetProgress(Mathf.Clamp01(InverseLerp(checkStart, checkEnd, character.transform.position)));
    }
}
