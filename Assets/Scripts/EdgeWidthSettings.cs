using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;
using UnityEngine.SceneManagement;

public class EdgeWidthSettings : MonoBehaviour
{
    static float edgeWidth = 0.4f;
    static string SHAPE_NAME = "CorruptionSpriteShape";
    
    [MenuItem("GameObject/CorruptionShape/Apply Edge Width")]
    static void ApplyEdgeWidth()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        List<Scene> allScenes = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
            allScenes.Add(SceneManager.GetSceneAt(i));

        foreach (Scene s in allScenes)
            ConvertInScene(s);
    }

    static void ConvertInScene(Scene s)
    {
        List<SpriteShapeController> shapes = new List<SpriteShapeController>();
        foreach (GameObject rootObject in s.GetRootGameObjects())
        {
            if(rootObject.name.Contains(SHAPE_NAME))
                shapes.Add(rootObject.GetComponent<SpriteShapeController>());
            
            foreach (Transform t in rootObject.transform)
            {
                if(rootObject.name.Contains(SHAPE_NAME))
                    shapes.Add(t.GetComponent<SpriteShapeController>());
            }
        }

        for (int i = 0; i < shapes.Count; i++)
        {
            Spline spline = shapes[i].spline;
            for (int j = 0; j < spline.GetPointCount(); j++)
            {
                spline.SetHeight(j, edgeWidth);
            }
        }
    }
}
