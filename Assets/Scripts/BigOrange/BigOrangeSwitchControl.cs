using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BigOrangeSwitchControl : MonoBehaviour
{
    public Transform[] switches;
    public List<GameObject> TileWall;
    public GameObject wall;
    float offset = 0.72f;

    List<Vector2> spawnPositions;

    IEnumerator revertRoutine;

    public void Start()
    {
        spawnPositions = new List<Vector2>();

        for (int i = 0; i < switches.Length; i++)
        {
            spawnPositions.Add(new Vector2(switches[i].transform.position.x - offset, switches[i].transform.position.y));
            spawnPositions.Add(new Vector2(switches[i].transform.position.x + offset, switches[i].transform.position.y));
            spawnPositions.Add(new Vector2(switches[i].transform.position.x, switches[i].transform.position.y + offset));
            spawnPositions.Add(new Vector2(switches[i].transform.position.x, switches[i].transform.position.y - offset));
        }
    }

    public void SpawnBlocks(int stompSpeed)
    {
        TileWall.Clear();
        for (int i = 0; i < switches.Length; i++)
        {
            TileWall.Add(wall);
            TileWall.Add(wall);
            TileWall.Add(wall);
            TileWall.Add(wall);

            for (int k = 0; k < 4; k++)
            {
                Instantiate(TileWall[k + (i * 4)]);
                TileWall[k + (i * 4)].transform.position = spawnPositions[k + (i * 4)];
            }
        }

        if (revertRoutine != null)
        {
            StopCoroutine(revertRoutine);
        }
        revertRoutine = RevertTiles(stompSpeed);
        StartCoroutine(revertRoutine);
    }

    public IEnumerator RevertTiles(int stompSpeed)
    {
        int waittimer = stompSpeed;
        while(waittimer >= 0)
        {
            yield return 0;
            waittimer -= 1;
        }

        if(waittimer <= 0)
        {
            for (int i = 0; i < TileWall.Count; i++)
            {
                    var walls = GameObject.FindGameObjectsWithTag("switchWall");

                foreach (GameObject wall in walls)
                    {
                        Destroy(wall.gameObject);
                    }
            }
        }         
    }
}
