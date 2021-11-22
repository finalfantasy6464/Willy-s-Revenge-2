using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject[] pickups;
    public GameObject timerObject;
    public GameObject enemyParent;

    public GameObject[] spawnTable;

    public GameObject SoundManager;
    MusicManagement music;

    public Vector3[] spawnPositions;

    public ArenaSetup arenaSetup;
    public PlayerController2021Arena player;

    public bool spawnLock;
    private bool levelStarted; 

    public float spawnTimer;
    public float spawnTimerCache;
    public float timerObjectChance;
    public float itemTime;

    public Vector3 playerStartPosition;
    float spawnCounter;
    float randomSpawnTimer;
    float itemCounter;
    float spawnPosx;
    float spawnposY;
    int spawnIndex;

    private void Start()
    {
        SoundManager = GameObject.Find("SoundManager");

        music = SoundManager.GetComponent<MusicManagement>();
        music.onLevelStart.Invoke();
        playerStartPosition = playerTransform.position;
    }

    public void Reset()
    {
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (go.tag.Contains("Enemy") || go.tag.Contains("Enemy2") || go.tag.Contains("Enemy3")) 
            {
                Destroy(go);
            }
            if (go.tag.Contains("Pickup"))
            {
                go.SetActive(false);
            }
        }
        itemCounter = 0f;
        spawnCounter = 0f;
    }

    public void ArenaStart()
    {
        playerTransform.position = playerStartPosition;
        SetSpawnPosition();
        SpawnPickup();
        SetSpawnIndex();
        spawnTimerCache = spawnTimer;
        SetSpawnTimer();
        levelStarted = true;
    }

    public void Update()
    {
        if(levelStarted == true)
        {
            SpawnEnemyTimer();
            ItemSpawnCheck();
        }
    }

    public void SetArenaState()
    {
        player.spriteRenderer.sprite = arenaSetup.skinSprites[arenaSetup.skinIndex];
        player.taillist[0].GetComponent<SpriteRenderer>().sprite = arenaSetup.tailSprites[arenaSetup.skinIndex];

        foreach (GameObject grid in arenaSetup.gridLayouts)
        {
            grid.SetActive(false);
        }

        foreach (GameObject bg in arenaSetup.backgrounds)
        {
            bg.SetActive(false);
        }

        arenaSetup.gridLayouts[arenaSetup.levelIndex].SetActive(true);
        arenaSetup.backgrounds[arenaSetup.levelIndex].SetActive(true);
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
