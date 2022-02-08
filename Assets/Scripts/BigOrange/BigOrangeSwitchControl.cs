using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BigOrangeSwitchControl : MonoBehaviour
{
    public Transform[] switches;
    public Tilemap grid;
    public Tile changetile;
    public Tile defaulttile;

    IEnumerator revertRoutine;

    void SpawnBlocks(int stompSpeed)
    {
        for (int i = 0; i < switches.Length; i++)
        {
            SetAdjacentTiles(switches[i], changetile);
        }

        if (revertRoutine != null)
        {
            StopCoroutine(revertRoutine);
        }
        revertRoutine = RevertTiles(switches, stompSpeed);
        StartCoroutine(revertRoutine);
    }

    public void SetAdjacentTiles(Transform current, Tile targetTile)
    {
        float sideSize = 0.72f;
        if(current != null)
        {
            Vector3 currentCellPos = current.transform.position;
            Vector3Int[] adjacentCells = new Vector3Int[]
            {
                grid.WorldToCell(currentCellPos + Vector3.left * sideSize),
                grid.WorldToCell(currentCellPos + Vector3.up * sideSize),
                grid.WorldToCell(currentCellPos + Vector3.right * sideSize),
                grid.WorldToCell(currentCellPos + Vector3.down * sideSize),
            };
            foreach (Vector3Int adjacentCell in adjacentCells)
            {
                grid.SetTile(adjacentCell, targetTile);
            }
        }
    }

    public IEnumerator RevertTiles(Transform current, int stompSpeed){

        int waittimer = stompSpeed;
        while (waittimer >= 0)
        {
            yield return 0;
            waittimer -= 1;
        }

        if(waittimer <= 0)
        {
            SetAdjacentTiles(current, null);
        }
    }

    public IEnumerator RevertTiles(Transform[] current, int stompSpeed)
    {
        int waittimer = stompSpeed;
        while(waittimer >= 0)
        {
            yield return 0;
            waittimer -= 1;
        }

        if(waittimer <= 0)
        {
            SetAdjacentTiles(current[0], null);
            SetAdjacentTiles(current[1], null);
            SetAdjacentTiles(current[2], null);
            SetAdjacentTiles(current[3], null);
        }
    }
}
