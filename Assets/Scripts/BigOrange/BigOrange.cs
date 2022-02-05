using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections;
using WillysRevenge2.BigOrangeMoves;

public class BigOrange : MonoBehaviour, IPausable
{
    public LineRenderer lightning;
    public EdgeCollider2D lightningCollider;
    public Animator m_animator;
    public Text hpText;

    public ParticleSystem[] smoke;

    public string previousstate;

    public Slider slider;

    public FinalBossActivation activator;

    int rng;
    int rng2;
    int loopcount = 0;

    public float MaxHP = 100000;
    public float HP = 100000;

    private int[] speeds;

    float HPpercentage;

    public Tilemap grid;
    public Tile changetile;
    public Tile defaulttile;

    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public Transform spawn4;
    public Transform leftShoulder;
    public Transform leftArm;
    public Transform leftHand;
    public Transform rightShoulder;
    public Transform rightArm;
    public Transform rightHand;

    public Transform[] switches;

    public GameObject BossActivator;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Enemy4;
    public GameObject Enemy5;
    public GameObject Enemy6;

    public BigOrangeMove chargeMove;
    public BigOrangeMove clapMove;
    public BigOrangeMove jumpMove;
    public BigOrangeMove leftSlamMove;
    public BigOrangeMove punchMove;
    public BigOrangeMove rightSlamMove;
    public BigOrangeMove stompMove;

    private IEnumerator RevertRoutine;

    public int stompspeedindex = 0;

    bool justlooped = false;

    public bool isPaused { get; set; }

    public BigOrangeMove currentMove;

    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        rng2 = 0;
        //IdleLoops();

        HPpercentage = Mathf.Round(HP / MaxHP * 100) / 100;
        activator = BossActivator.GetComponent<FinalBossActivation>();

        speeds = new int[]
        {
            150, 100, 50
        };

        StartCoroutine(WaitForEntranceRoutine());
    }

    private IEnumerator WaitForEntranceRoutine()
    {
        while(m_animator.GetCurrentAnimatorStateInfo(0).IsName("Entrance"))
                yield return null;

        rngGenerate();
    }

    private void Update()
    {
        slider.maxValue = MaxHP;
        slider.minValue = 0;
        slider.value = HP;

        hpText.text = HP.ToString() + " / " + MaxHP.ToString();

        if (HP <= 0)
        {
            foreach(ParticleSystem smokeparticle in smoke)
            {
                var emission = smokeparticle.emission;
                emission.rateOverTime = 0;
            }
            m_animator.Play("Death");
            hpText.text = "0" + " / " + MaxHP.ToString();
        }

        if(HP/MaxHP <= 0.9f)
        {
            smoke[0].Play();
        }
        if(HP/MaxHP <= 0.7f)
        {
            smoke[1].Play();
        }
        if(HP/MaxHP <= 0.3f)
        {
            smoke[2].Play();
        }
    }

    void rngGenerate()
    {
        rng = Random.Range(1, 100);
        rng2 = Random.Range(1, 100);

        ChooseMove();
    }

    void ChooseMove()
    {
        PlayerController2021remake player = FindObjectOfType<PlayerController2021remake>();
        if (rng > 1 && rng <= 50)
        {
            ((Punch)punchMove).Execute(player, this, "Left");
        }
        if (rng > 50 && rng <= 99)
        {
            ((Punch)punchMove).Execute(player, this, "Right");
        }
        /*
        if (rng <= 20)
        {
            m_animator.SetBool("Jump", true);
            stompspeedindex = 0;
        }
        if (rng > 20 && rng <= 50)
        {
            m_animator.SetBool("LeftSlam", true);
            stompspeedindex = 0;
        }
        if (rng > 50 && rng <= 80)
        {
            m_animator.SetBool("RightSlam", true);
            stompspeedindex = 0;
        }
        if (rng > 80 && rng <= 100)
        {
            m_animator.SetBool("Stomp", true);
            if(stompspeedindex < 2)
                stompspeedindex ++;
        }
        if (rng > 100 && rng <= 120)
        {
            ((Punch)punchMove).Execute(FindObjectOfType<PlayerController2021remake>(), this, leftShoulder, leftArm, leftHand);
        }
        if (rng > 120 && rng <= 140)
        {
            ((Punch)punchMove).Execute(FindObjectOfType<PlayerController2021remake>(), this, rightShoulder, rightArm, rightHand);
        }
        if (rng > 140 && rng <= 160)
        {

        }
        if (rng > 160 && rng <= 180)
        {

        }*/
    }

    void forceidle()
    {
        foreach (AnimatorControllerParameter parameter in m_animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                m_animator.SetBool(parameter.name, false);
            }
        }
    }

    void IdleLoops()
    {
        justlooped = false;

        foreach (AnimatorControllerParameter parameter in m_animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                m_animator.SetBool(parameter.name, false);
        }

        HPpercentage = Mathf.Round(HP / MaxHP * 100) / 100;

        m_animator.SetBool("Idle", true);
        m_animator.SetFloat("Speed", 1 + (3 - (3 * HPpercentage)));

        if (loopcount == 0 && justlooped == false)
        {
            if (rng2 >= 50)
            {
                rngGenerate();
                loopcount = 0;
                m_animator.SetBool("Idle", false);
            }
            loopcount += 1;
            justlooped = true;
        }
        if (loopcount == 1 && justlooped == false)
        {
            if (rng2 >= 75)
            {
                rngGenerate();
                loopcount = 0;
                m_animator.SetBool("Idle", false);
            }
            loopcount += 1;
            justlooped = true;
        }

        if (loopcount == 2 && justlooped == false)
        {
            rngGenerate();
            loopcount = 0;
            m_animator.SetBool("Idle", false);
            justlooped = true;
        }
    }

    void SpawnEnemy1()
    {
        CreateEnemy<EnemyMovement>(Enemy1, spawn1, Vector2.zero);
        CreateEnemy<EnemyMovement>(Enemy1, spawn1, new Vector2(-0.72f, 0));
        CreateEnemy<EnemyMovement>(Enemy1, spawn1, new Vector2(0.72f, 0));

        CreateEnemy<EnemyMovementTwo>(Enemy5, spawn3, Vector2.zero);
        CreateEnemy<EnemyMovementTwo>(Enemy5, spawn3, new Vector2(0, -0.72f));
        CreateEnemy<EnemyMovementTwo>(Enemy5, spawn3, new Vector2(0, 0.72f));
    }

    void SpawnEnemy2()
    {
        CreateEnemy<EnemyMovement>(Enemy2, spawn2, Vector2.zero);
        CreateEnemy<EnemyMovement>(Enemy2, spawn2, new Vector2(-0.72f, 0));
        CreateEnemy<EnemyMovement>(Enemy2, spawn2, new Vector2(0.72f, 0));

        CreateEnemy<EnemyMovementTwo>(Enemy6, spawn4, Vector2.zero);
        CreateEnemy<EnemyMovementTwo>(Enemy6, spawn4, new Vector2(0, -0.72f));
        CreateEnemy<EnemyMovementTwo>(Enemy6, spawn4, new Vector2(0, 0.72f));
    }

    void SpawnEnemy3()
    {
        GameObject newenemy = Instantiate(Enemy3, spawn1.transform.position, Quaternion.identity);
        PauseControl.TryAddPausable(newenemy.GetComponentInChildren<EnemyMovementThreeVariant>().gameObject);
        GameObject newenemy2 = Instantiate(Enemy4, spawn2.transform.position, Quaternion.identity);
        PauseControl.TryAddPausable(newenemy2.GetComponentInChildren<EnemyMovementThreeVariant>().gameObject);
    }

    void CreateEnemy<T>(GameObject prefab, Transform spawn, Vector2 positionOffset)
    {
        GameObject newEnemy = Instantiate(prefab);
        PauseControl.TryAddPausable(newEnemy);
        T moveComponent = newEnemy.GetComponent<T>();
        newEnemy.transform.position = spawn.position + (Vector3)positionOffset;
        
        if(moveComponent is EnemyMovement moveOne)
            moveOne.multiplier = 3;
        if(moveComponent is EnemyMovementTwo moveTwo)
            moveTwo.multiplier = 3;
    }

    void SpawnBlocks()
    {
        for (int i = 0; i < switches.Length; i++)
        {
            SetAdjacentTiles(switches[i], changetile);
        }

        if (RevertRoutine != null)
        {
            StopCoroutine(RevertRoutine);
        }
        RevertRoutine = RevertTiles(switches);
        StartCoroutine(RevertRoutine);
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

    public IEnumerator RevertTiles(Transform current){

        int waittimer = speeds[stompspeedindex];
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

    public IEnumerator RevertTiles(Transform[] current)
    {
        int waittimer = speeds[stompspeedindex];
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

    public void BattleHasEnded()
    {
        activator.BattleEnd();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }

    public void OnPause()
    {
        m_animator.SetFloat("PausedSpeed", 0);
        m_animator.SetFloat("EntranceSpeed", 0);
        m_animator.SetFloat("Speed", 0);
    }

    public void OnUnpause()
    {
        m_animator.SetFloat("PausedSpeed", 1);
        m_animator.SetFloat("EntranceSpeed", 1);
        m_animator.SetFloat("Speed", 1 + (3 - (3 * HPpercentage)));
    }

    public void UnPausedUpdate()
    { }
}