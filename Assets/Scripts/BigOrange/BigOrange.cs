using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using WillysRevenge2.BigOrangeMoves;
using UnityEngine.Experimental.Rendering.Universal;

public class BigOrange : MonoBehaviour, IPausable
{
    public float MaxHP = 100000;
    public float HP = 100000;

    [Header("VFX")]
    public LineRenderer lightning;
    public Light2D forearmLight;
    public Light2D bicepLight;
    public ParticleSystem[] smoke;

    [Header("Body Parts")]
    public Transform leftShoulder;
    public Transform leftArm;
    public Transform leftFoot;
    public Transform leftHand;
    public Transform rightShoulder;
    public Transform rightArm;
    public Transform rightHand;
    public Transform rightFoot;
    public Transform [] partTransforms => new Transform[]
    {
        leftShoulder, leftArm, leftFoot, leftHand,
        rightShoulder, rightArm, rightHand, rightFoot
    };

    [Header("Moves")]
    public BigOrangeMove currentMove;
    public int stompspeedindex = 0;
    [Space]
    public BigOrangeMove chargeMove;
    public BigOrangeMove clapMove;
    public BigOrangeMove jumpMove;
    public BigOrangeMove punchMove;
    public BigOrangeMove slamMove;
    public BigOrangeMove stompMove;
    public BigOrangeMove hopMove;
    public bool isPaused { get; set; }
    [Space]
    public string previousstate;

    [Header("Dependencies")]
    public CameraShaker cameraShaker;    
    public Animator m_animator;
    public BigOrangeSwitchControl switchControl;
    public BigOrangeEnemySpawner enemySpawner;
    public BigOrangeSound sound;
    public FinalBossActivation activator;
    [HideInInspector]
    public int[] speeds;

    public event Action OnTakeDamage;
    bool justlooped = false;
    int rng;
    int rng2;
    int loopcount = 0;
    float HPpercentage;

    void Start()
    {
        OnTakeDamage += () => ShakeCamera(1f);      
        rng2 = 0;
        HPpercentage = Mathf.Round(HP / MaxHP * 100) / 100;
        speeds = new int[]
        {
            150, 100, 50
        };
        StartCoroutine(WaitForEntranceRoutine());
    }

    /// Reduces health and returns whether B.O. died as a result of it
    public bool TakeDamage(int amount)
    {
        HP = Mathf.Max(0, HP - amount);
        GameSoundManagement.instance.efxSource.Stop();

        if(amount > 0)
            OnTakeDamage?.Invoke();
        
        if(currentMove is Punch)
        {
            ((Punch)currentMove).ForceFinish();
            SetBoolAnimationParameters(false);
        }

        m_animator.Play("Damage");
        return HP == 0;
    }

    public void ShakeCameraHorizontal(float shakeAmount)
    {
        cameraShaker.Shake(shakeAmount, 0f);
    }

    public void ShakeCameraVertical(float shakeAmount)
    {
        cameraShaker.Shake(0f, shakeAmount);
    }

    public void ShakeCamera(float shakeAmount)
    {
        cameraShaker.Shake(shakeAmount, shakeAmount);
    }

    private IEnumerator WaitForEntranceRoutine()
    {
        
        while(m_animator.GetCurrentAnimatorStateInfo(0).IsName("Entrance"))
                yield return null;

        // Fade Out
        
        while(m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0f)
        {
            if(m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                break;
            yield return null;
        }

        // Fade In
        activator.BattleActivated();
        IdleLoops();
    }

    private void Update()
    {
        if (HP <= 0)
        {
            foreach(ParticleSystem smokeparticle in smoke)
            {
                var emission = smokeparticle.emission;
                emission.rateOverTime = 0;
            }
            m_animator.SetFloat("Speed", 1);
            m_animator.Play("Death");
        }

        if(HP/MaxHP <= 0.9f)
        {
            smoke[0].gameObject.SetActive(true);
            smoke[0].Play();
        }
        if(HP/MaxHP <= 0.7f)
        {
            smoke[1].gameObject.SetActive(true);
            smoke[1].Play();
        }
        if(HP/MaxHP <= 0.3f)
        {
            smoke[2].gameObject.SetActive(true);
            smoke[2].Play();
        }
    }

    void rngGenerate()
    {
        rng = UnityEngine.Random.Range(1, 225);
        rng2 = UnityEngine.Random.Range(1, 100);

        ChooseMove();
    }

    void ChooseMove()
    {
        PlayerController2021remake player = FindObjectOfType<PlayerController2021remake>();
        //((Punch)punchMove).Execute(player, this, UnityEngine.Random.Range(0f, 1f) > 0.5f ? "Right" : "Left");
        //    currentMove = punchMove;
        //    stompspeedindex = 0;
   
        if (rng <= 35)
        {
            ((Punch)punchMove).Execute(player, this, UnityEngine.Random.Range(0f, 1f) > 0.5f ? "Right" : "Left");
            currentMove = punchMove;
            stompspeedindex = 0;
        }
        if (rng > 35 && rng <= 40)
        {
            ((Jump)jumpMove).Execute(this);
            currentMove = jumpMove;
            stompspeedindex = 0;
        }
        if (rng > 40 && rng <= 140)
        {
            ((Slam)slamMove).Execute(this, UnityEngine.Random.Range(0f, 1f) > 0.5f ? "Right" : "Left");
            currentMove = slamMove;
            stompspeedindex = 0;
        }
        if (rng > 140 && rng <= 160)
        {
            currentMove = stompMove;
            stompspeedindex = Mathf.Min(3, stompspeedindex + 1);
            ((Stomp)stompMove).Execute(this);
        }
        if (rng > 160 && rng <= 180)
        {
            ((Charge)chargeMove).Execute(m_animator);
            currentMove = chargeMove;
            stompspeedindex = 0;
        }
        if (rng > 180 && rng <= 200)
        {
            ((Clap)clapMove).Execute(m_animator);
            currentMove = clapMove;
            stompspeedindex = 0;
        }
        if(rng > 200)
        {
            ((Hop)hopMove).Execute(m_animator);
            currentMove = hopMove;
            stompspeedindex = 0;
        }
    }

    void SetBoolAnimationParameters(bool value)
    {
        foreach (AnimatorControllerParameter parameter in m_animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                m_animator.SetBool(parameter.name, value);
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

    public (Vector3[], Quaternion[]) GetCurrentPartsState()
    {
        List<Vector3> positions = new List<Vector3>();
        List<Quaternion> rotations = new List<Quaternion>();
        
        foreach (Transform child in transform)
        {
            positions.Add(child.position);
            rotations.Add(child.rotation);
        }

        return (positions.ToArray(), rotations.ToArray());

    }

    public void SetCurrentPartsState(Vector3[] positions, Vector3[] eulerAngles)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(0).position = positions[i];
            transform.GetChild(0).eulerAngles = eulerAngles[i];
        }
    }
}