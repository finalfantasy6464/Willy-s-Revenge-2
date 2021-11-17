using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public GameObject[] pickups;
    public GameObject timerObject;
    public GameObject enemyParent;

    public GameObject[] spawnTable;

    public Vector3[] spawnPositions;

    public bool spawnLock;
    public float spawnTimer;
    public float spawnTimerCache;
    public float timerObjectChance;
    public float itemTime;

    float spawnCounter;
    float randomSpawnTimer;
    float itemCounter;
    float spawnPosx;
    float spawnposY;
    int spawnIndex;

    private void Start()
    {

        SetSpawnPosition();
        SpawnPickup();
        SetSpawnIndex();
        spawnTimerCache = spawnTimer;
        SetSpawnTimer();
    }

    public void Update()
    {
        SpawnEnemyTimer();
        ItemSpawnCheck();
    }

    void ItemSpawnCheck()
    {
        itemCounter += Time.deltaTime;
        if(itemCounter >= itemTime)
        {
            SpawnAny();
            itemCounter = 0f;
        }
    }

    public void SpawnAny()
    {
        if (timerObjectChance >= Random.Range(0f, 1f) && !timerObject.activeInHierarchy)
        {
            SetSpawnPosition();
            timerObject.transform.position = new Vector3(spawnPosx, spawnposY, -1);
            timerObject.SetActive(true);
        }
        else
        {
            SetSpawnPosition();
            SpawnPickup();
        }
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
        bool isSpaceValid;
        do
        {
            isSpaceValid = true;
            spawnPosx = Random.Range(-3.25f, 4.69f);
            spawnposY = Random.Range(0.35f, -8.29f);

            foreach (GameObject pickup in pickups)
            {
                if(pickup.activeInHierarchy && Vector2.Distance(
                            new Vector2(spawnPosx, spawnposY), pickup.transform.position) < 0.5f)
                {
                    isSpaceValid = false;
                    break;
                }
            }
        } while(!isSpaceValid);
    }

    public void SpawnPickup()
    {
        foreach (GameObject pickup in pickups)
        {
            if(!pickup.activeInHierarchy)
            {
                pickup.transform.position = new Vector3(spawnPosx, spawnposY, -1);
                pickup.SetActive(true);
                return;
            }
        }
    }
}
