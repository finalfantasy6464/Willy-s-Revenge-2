using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;
using UnityEngine.SceneManagement;

public class EdgeWidthSettings : MonoBehaviour
{
    public static float edgeWidth = 0.6f;
    const string SHAPE_NAME = "CorruptionSpriteShape";
    const string MATERIAL_PATH = "CorruptionShape/CorruptionEdgeMaterial";
    const string MATERIAL_INVERTED_PATH = "CorruptionShape/CorruptionEdgeMaterialInverted";
    
    /*[MenuItem("GameObject/CorruptionShape/Preview Edge Width")]*/

    public static void ApplyEdgeWidth()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        List<Scene> allScenes = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
            allScenes.Add(SceneManager.GetSceneAt(i));

        foreach (Scene s in allScenes)
            SetSceneShapesWidth(s);
    }

    /*[MenuItem("GameObject/CorruptionShape/Update Edge Direction")]*/

    static void UpdateEdgeDirection()
    {
        SpriteShapeController[] shapes = GetShapeControllers();
        SpriteShapeRenderer[] renderers = GetShapeRenderers();
        Material defaultMaterial = Resources.Load<Material>(MATERIAL_PATH);
        Material invertedMaterial = Resources.Load<Material>(MATERIAL_INVERTED_PATH);

        for (int i = 0; i < shapes.Length; i++)
        {
            bool isClockwise = shapes[i].spline.GetPosition(0).x
                    <= shapes[i].spline.GetPosition(1).x;

            // Edge Material
            renderers[i].sharedMaterials[1] = isClockwise ? defaultMaterial : invertedMaterial;
        }
    }

    static void SetSceneShapesWidth(Scene s)
    {
        SpriteShapeController[] shapes = GetShapeControllers();
        for (int i = 0; i < shapes.Length; i++)
        {
            Spline spline = shapes[i].spline;
            for (int j = 0; j < spline.GetPointCount(); j++)
            {
                spline.SetHeight(j, edgeWidth);
            }
            shapes[i].UpdateSpriteShapeParameters();
            shapes[i].BakeMesh();
        }
    }

    static SpriteShapeController[] GetShapeControllers()
    {
        List<SpriteShapeController> shapes = new List<SpriteShapeController>();
        foreach (GameObject rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if(rootObject.name.Contains(SHAPE_NAME))
                shapes.Add(rootObject.GetComponent<SpriteShapeController>());
            
            foreach (Transform t in rootObject.transform)
            {
                if(rootObject.name.Contains(SHAPE_NAME))
                    shapes.Add(t.GetComponent<SpriteShapeController>());
            }
        }

        return shapes.ToArray();
    }

    static SpriteShapeRenderer[] GetShapeRenderers()
    {
        List<SpriteShapeRenderer> shapes = new List<SpriteShapeRenderer>();
        foreach (GameObject rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if(rootObject.name.Contains(SHAPE_NAME))
                shapes.Add(rootObject.GetComponent<SpriteShapeRenderer>());
            
            foreach (Transform t in rootObject.transform)
            {
                if(rootObject.name.Contains(SHAPE_NAME))
                    shapes.Add(t.GetComponent<SpriteShapeRenderer>());
            }
        }

        return shapes.ToArray();
    }
}
