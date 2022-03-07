using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ConvertCorruptionShapes : Editor
{
    const string PREFAB_PATH = "ResourcePrefabs/CorruptionSpriteShape";

    [MenuItem("GameObject/Convert CorruptionShape into CorruptionSpriteShape")]
    static void ConvertShapes()
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
        List<GameObject> corruptObjects = new List<GameObject>();
        foreach (GameObject rootObject in s.GetRootGameObjects())
        {
            if(rootObject.name.Contains("CorruptionShape"))
                corruptObjects.Add(rootObject);
            
            foreach (Transform t in rootObject.transform)
            {
                if(rootObject.name.Contains("CorruptionShape"))
                    corruptObjects.Add(t.gameObject);
            }
        }

        GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
        for (int i = 0; i < corruptObjects.Count; i++)
        {
            GameObject newShape = PrefabUtility.InstantiatePrefab(prefab, null) as GameObject;
            newShape.name = "CorruptionSpriteShape_" + i.ToString("00");
            ConvertToSpriteShape(corruptObjects[i].GetComponent<PolygonCollider2D>(),
                    newShape.GetComponent<SpriteShapeRenderer>(),
                    newShape.GetComponent<SpriteShapeController>(),
                    newShape.GetComponent<PolygonCollider2D>());
        }
    }

    static void ConvertToSpriteShape(PolygonCollider2D originalPolygon,
            SpriteShapeRenderer shapeRenderer,
            SpriteShapeController shapeController,
            PolygonCollider2D shapePolygon)
    {
        shapeRenderer.transform.position = originalPolygon.transform.position;
        shapeRenderer.transform.eulerAngles = originalPolygon.transform.eulerAngles;
        shapePolygon.pathCount = originalPolygon.pathCount;
        
        for (int i = 0; i < originalPolygon.pathCount; i++)
            shapePolygon.SetPath(i, originalPolygon.GetPath(i));

        Spline spline = shapeController.spline;
        spline.Clear();
        int index = 0;
        foreach (Vector2 point in originalPolygon.points)
        {
            spline.InsertPointAt(index, point);
            index++;
        }

        shapeController.RefreshSpriteShape();
    }
}
