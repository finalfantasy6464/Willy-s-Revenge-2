using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public GameObject[] pickups;
    public GameObject enemyParent;

    public GameObject[] spawnTable;

    public Vector3[] spawnPositions;

    public bool spawnLock;

    float spawnPosx;
    float spawnposY;

    int spawnIndex;

    public float spawnTimer;
    public float spawnTimerCache;

    float spawnCounter;

    float randomSpawnTimer;


    private void Start()
    {
        SetSpawnPosition();
        SetSpawnIndex();
        spawnTimerCache = spawnTimer;
        SetSpawnTimer();
    }

    public void Update()
    {
        SpawnEnemyTimer();
    }

    public void SetSpawnIndex()
    {
        spawnIndex = Random.Range(0, spawnTable.Length);
    }

    public void SetSpawnTimer()
    {
        spawnTimer = Random.Range(spawnTimerCache, spawnTimerCache * 2.5f);
    }
    public void SpawnEnemyTimer()
    {
        if (!spawnLock)
        {
            spawnCounter += Time.deltaTime;

            if (spawnCounter >= spawnTimer)
            {
                spawnCounter = 0;
                SetSpawnTimer();
                SpawnEnemy();
            }
        }
    }
    private void SpawnEnemy()
    {
        Instantiate(spawnTable[spawnIndex]);
        spawnTable[spawnIndex].transform.position = spawnPositions[spawnIndex];
        
        SetSpawnIndex();
    }

    public void SetSpawnPosition()
    {
        spawnPosx = Random.Range(-3.25f, 4.69f);
        spawnposY = Random.Range(0.35f, -8.29f);
        SpawnPickup();
    }

    public void SpawnPickup()
    {
        pickups[0].transform.position = new Vector3(spawnPosx, spawnposY, -1);
        pickups[0].SetActive(true);
    }
}
