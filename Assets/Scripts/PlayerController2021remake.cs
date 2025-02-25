using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController2021remake : MonoBehaviour, IPausable
{
    public PlayerCollision playerCollision;
    //Otherobjects
    public bool shieldactive = false;
    public GameObject Shield;
    public GameObject PlayerSprite;
    bool Switch1Pressed = false;
    bool Switch2Pressed = false;
    private Vector3 doormove = new Vector3(0, 0.72f, 0);

    public bool dirlock = false;
    public bool gotgold = false;
    public bool delaylock = false;
    public bool isHeadJumping = false;

    public Sprite[] skinSprites;
    public Sprite[] tailSprites;
 
    public int enabledSegmentAmount;
    public Transform lastEnabledSegment => taillist[enabledSegmentAmount - 1].transform;
    private float pelletgap;
    Rigidbody2D rb;

    SpriteRenderer spriteRenderer;

    //Segments
    public List<GameObject> taillist = new List<GameObject>();
    public List<SpriteRenderer> tailListRenderers = new List<SpriteRenderer>();
    public List<CircleCollider2D> tailListColliders = new List<CircleCollider2D>();
    private List<Vector3> PositionHistory = new List<Vector3>();
    public List<float> AlphaHistory = new List<float>();
    public List<Vector3> slowdownPositionCache = new List<Vector3>();
    public int followDelay;
    public int speedIncrease;
    public bool debugShowFromStart;
    public Sprite[] SegmentSprites;
    public int offsetFromHead;
    public int LatestSegmentDelay;
    public int SegmentDelaySleep = 5;
    public int SegmentDelaySleepcache;
    public int SegmentDelaySleepCounter;
    public int SegmentHistorySkipCounter;
    public int SegmentHistorySkipTimer;
    public bool ate;
    public bool isSegmentJumping;
    public Vector3 LastLandPosition;

    // movement
    public int presentdir;
    public Vector3 direction = Vector3.zero;
    public Vector3 finalMovementVector = Vector3.zero;
    public Vector3 shiftVector = Vector3.zero;
    public Vector3 corruptionDirectionCache = Vector3.zero;

    private bool moving;
    private float rotationAngle;
    public bool canmove = true;
    public bool directionreset = false;

    public float basespeed = 1.8f;
    public float speedMultiplier = 1f;
    public float finalspeed;

    private float pelletmagnitude = 0.08f;
    private float horizontal;
    private float vertical;
    private float dpadX;
    private float dpadY;

    public float pelletno = 0.0f;

    private int Scorecount;

    private int numberofpickups;

    LevelCanvas canvas;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI speedText;
    public Image GoldenPellet;

    [HideInInspector] public UnityEvent onEatPellet;
    [HideInInspector] public UnityEvent onGoldenPellet;
    [HideInInspector] public UnityEvent onSlidingDoorOpen;
    [HideInInspector] public UnityEvent onSlidingDoorClose;
    [HideInInspector] public UnityEvent onCollectShield;

    public bool isPaused { get ; set ; }

    void Awake()
    {
        onEatPellet = new UnityEvent();
        onGoldenPellet = new UnityEvent();
        onSlidingDoorOpen = new UnityEvent();
        onSlidingDoorClose = new UnityEvent();
        onCollectShield = new UnityEvent();

        for (int i = 0; i < 80; i++) 
        {
            slowdownPositionCache.Add(new Vector3(99,99,99));
        }

        SegmentDelaySleepcache = SegmentDelaySleep;
    }

    void Start()
    {
        canvas = GameObject.FindObjectOfType<LevelCanvas>();
        countText = canvas.countText;
        GoldenPellet = canvas.goldenImage;
        speedText = canvas.speedText;


        basespeed = basespeed * speedMultiplier;
        Scorecount = 0;
        dirlock = false;
        SetCountMax();
        SetCountText();
        SetSpeedText();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = PlayerSprite.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = skinSprites[GameControl.control.currentCharacterSprite];
        SetTailPieceSprite();

        for (int i = 0; i < taillist.Count; i++)
        {
            if (taillist[i].activeInHierarchy)
                enabledSegmentAmount++;
            else
                break;
        }

        if (debugShowFromStart == true)
        {
            foreach (GameObject segment in taillist)
            {
                segment.SetActive(true);
            }
        }
    }

    void Update()
    {
        if(!isPaused)
            UnPausedUpdate();
    }

    void UpdateHistoryLists()
    {
        PositionHistory.Insert(0, transform.position);
        AlphaHistory.Insert(0, spriteRenderer.color.a);

        if(PositionHistory.Count > 1000)
        {
            PositionHistory.RemoveRange(1000, PositionHistory.Count - 1000);
        }

        if (AlphaHistory.Count > 1000)
        {
            AlphaHistory.RemoveRange(1000, AlphaHistory.Count - 1000);
        }

    }

    void Move()
    {
        if (dirlock == false)
        {
            if (GameInput.GetKeyDown("right"))
            {
                direction.x = 1;
                direction.y = 0;
                rotationAngle = 0;
            }
            else if (GameInput.GetKeyDown("left"))
            {
                direction.x = -1;
                direction.y = 0;
                rotationAngle = 180;
            }
            else if (GameInput.GetKeyDown("up"))
            {
                direction.y = 1;
                direction.x = 0;
                rotationAngle = 90;
            }
            else if (GameInput.GetKeyDown("down"))
            {
                direction.y = -1;
                direction.x = 0;
                rotationAngle = 270;
            }
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }

        finalspeed = basespeed + (pelletmagnitude * (pelletno * speedMultiplier));

        if (canmove)
        {
            if(corruptionDirectionCache != Vector3.zero)
                direction = corruptionDirectionCache;
            finalMovementVector = shiftVector + (direction * finalspeed) * Time.deltaTime;
            transform.position += finalMovementVector;
        }
        else
        {
            direction.x = 0;
            direction.y = 0;
            finalMovementVector = Vector3.zero;
            transform.Translate(finalMovementVector);
        }
    }

    void UpdateSegmentSprite(Sprite segmentsprite)
    {
        SpriteRenderer currentrenderer;
        SpriteRenderer previousrenderer;

        for (int i = taillist.Count - 1; i > 0; i--)
        {
            if (!taillist[i].activeInHierarchy)
            {
                continue;
            }
            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            previousrenderer = taillist[i - 1].GetComponent<SpriteRenderer>();
            currentrenderer.sprite = previousrenderer.sprite;
        }

        taillist[0].GetComponent<SpriteRenderer>().sprite = segmentsprite;
    }

    void SetTailPieceSprite()
    {
        SegmentSprites[0] = tailSprites[GameControl.control.currentCharacterSprite];
        foreach(GameObject tail in taillist)
        {
            tail.GetComponent<SpriteRenderer>().sprite = SegmentSprites[0];
        }
    }

    void UpdateSegmentTint()
    {
        SpriteRenderer currentrenderer;
        
        for (int i = 0; i < taillist.Count; i++)
        {
            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            currentrenderer.color = new Color(1f - (i * 0.007f), 1f - (i * 0.007f), 1f - (i * 0.007f), 1f);
           
            if (!taillist[i].activeInHierarchy)
            {
                break;
            }
        }
    }

    void UpdateSegmentAlpha()
    {
        SpriteRenderer currentrenderer;

        for (int i = 0; i < taillist.Count; i++)
        {
            if (!taillist[i].activeInHierarchy)
            {
                break;
            }

            currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
            if (AlphaHistory.Count > offsetFromHead + (followDelay * i))
            {
                currentrenderer.color = new Color(currentrenderer.color.r, currentrenderer.color.b, currentrenderer.color.g, AlphaHistory[offsetFromHead + (followDelay * i)]);
            }
        }
    }

    void ShowNextSegment(Sprite segmentsprite, int speedIncrease)
    {
        SegmentHistorySkipCounter = offsetFromHead;

        if (ate)
        {
            LatestSegmentDelay = 1;
            SegmentDelaySleepCounter = SegmentDelaySleep;
        }
        ate = true;
        LatestSegmentDelay = offsetFromHead;
        SegmentDelaySleep = SegmentDelaySleepcache;

        for (int i = 0; i < taillist.Count; i++)
        {
            if (!taillist[i].activeInHierarchy)
            {
                taillist[i].SetActive(true);
                enabledSegmentAmount++;
                break;
            }
        }
        UpdateSegmentSprite(segmentsprite);
        UpdateSegmentTint();
    }

    void UpdateSegmentsPosition()
    {
        int delay;

        for (int i = 0; i < taillist.Count; i++)
        {
            if (!taillist[i].activeInHierarchy)
                break;

            delay = offsetFromHead + (followDelay * i);

            taillist[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            if (PositionHistory.Count > delay)
            {
                taillist[i].transform.position = PositionHistory[delay];
            }

            if (i == taillist.Count - 1 && taillist[i].transform.position == LastLandPosition)
            {
                isSegmentJumping = false;
            }
        }
    }

    void UpdateSegmentDelay()
    {
        if (!ate)
          return;

        if (SegmentDelaySleepCounter < SegmentDelaySleep)
        {
            SegmentDelaySleepCounter++;
        }
        else
        {
            LatestSegmentDelay--;
            if (LatestSegmentDelay == 0)
            {
                ate = false;
                LatestSegmentDelay = offsetFromHead;
                SegmentDelaySleep = SegmentDelaySleepcache;
            }
            else
            {
                SegmentDelaySleep = Mathf.Max(0, SegmentDelaySleep - 1);
            }
            SegmentDelaySleepCounter = 0;
        }
    }

    void UpdateSegmentOrder()
    {
        SpriteRenderer currentrenderer;

        for (int i = 0; i < taillist.Count; i++)
        {
             currentrenderer = taillist[i].GetComponent<SpriteRenderer>();
             currentrenderer.sortingOrder = -i;

            if (isHeadJumping || isSegmentJumping)
            {
             currentrenderer.sortingOrder += (int)(taillist[i].transform.localScale.x * 10.0f);
            }

            if (!taillist[i].activeInHierarchy)
            {
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletgap = 0;
            pelletno += 1;
            Scorecount = Scorecount + 1;
            
            SetCountText();
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[0], 1);
            }
        }

        if (other.gameObject.CompareTag("GoldenPickup"))
        {
            onGoldenPellet.Invoke();
            Destroy(other.gameObject);
            GoldenPellet.enabled = true;
            gotgold = true;
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[1], 0);
            }
        }

        if (other.gameObject.tag == "SuperPickup")
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletno += 40;
            if (!debugShowFromStart)
            {
               ShowNextSegment(SegmentSprites[4], 40);
            }
        }

        if (other.gameObject.CompareTag("AntiPickup"))
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletno = Mathf.Max(0, pelletno - 40);

            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[2], -40);
            }
        }

        if (other.gameObject.CompareTag("SlowPickup"))
        {
            onEatPellet.Invoke();
            Destroy(other.gameObject);
            pelletno = Mathf.Max(0, pelletno - 5);
            if (!debugShowFromStart)
            {
                ShowNextSegment(SegmentSprites[3], -5);
            }
        }


        if (other.gameObject.CompareTag("Switch"))
        {
            if (Switch1Pressed == false)
            {
                Switch1Pressed = true;

                Switch2Pressed = false;
            }
        }
        if (other.gameObject.CompareTag("Switch2"))
        {
            if (Switch2Pressed == false)
            {
                Switch1Pressed = false;

                Switch2Pressed = true;
            }
        }

        if (other.gameObject.CompareTag("Shield"))
        {
            GameObject Shieldnew = new GameObject();

            if(shieldactive != true)
            {
                onCollectShield.Invoke();
                Vector3 s = transform.position;
                Destroy(other.gameObject);
                Shieldnew = Instantiate(Shield, s, Quaternion.identity);
                PauseControl.TryAddPausable(Shieldnew);
                shieldactive = true;
            }
            else
            {
                Shieldnew = GameObject.FindGameObjectWithTag("ActiveShield");
                Shieldnew.GetComponent<Shield>().shieldtimer += 5.0f;
                Destroy(other.gameObject);
            }
        }
}

    void SetCountText()
    {
        countText.text = "Pickups: " + Scorecount.ToString() + " / " + numberofpickups.ToString();
    }

    void SetSpeedText()
    {
        if(speedText != null)
        {
            if (direction == Vector3.zero)
            {
                speedText.text = "Speed: " + 0 + " Mph";
            }
            else
            {
                speedText.text = "Speed: " + finalspeed.ToString() + " MPH";
            }
        }
    }

    void SetCountMax()
    {
        GameObject[] totalpickups = GameObject.FindGameObjectsWithTag("Pickup");
        numberofpickups = totalpickups.Length;
    }

    void DebugOptionsCheck()
    {
        if (Input.GetKey(KeyCode.N))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Input.GetKey(KeyCode.B))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    void SceneReloader()
    {
        if (GameInput.InputMapPressedDown["reset"]())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnPause()
    {
        isPaused = true;
    }

    public void OnUnpause()
    {
        isPaused = false;
    }

    public void UnPausedUpdate()
    {
        if(!playerCollision.dyingByCorruption)
        {
            Move();
            UpdateSegmentAlpha();
        }
        
        SetSpeedText();
        UpdateSegmentsPosition();
        UpdateSegmentDelay();
        UpdateSegmentOrder();
        //DebugOptionsCheck();
        SceneReloader();
        UpdateHistoryLists();
        this.pelletgap += Time.deltaTime;
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}