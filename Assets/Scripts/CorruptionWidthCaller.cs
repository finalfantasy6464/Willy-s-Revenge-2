using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CorruptionWidthCaller : MonoBehaviour
{
    void Start()
    {
        SpriteShapeController shape = GetComponent<SpriteShapeController>();
        Spline spline = shape.spline;
        for (int j = 0; j < spline.GetPointCount(); j++)
        {
            spline.SetHeight(j, EdgeWidthSettings.edgeWidth);
        }
        shape.UpdateSpriteShapeParameters();
        Destroy(this);
    }
}
