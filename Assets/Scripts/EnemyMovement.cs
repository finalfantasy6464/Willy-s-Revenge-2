using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour, IPausable
{

	private float movespeed = 0.0f;
    private float defaultmovestep;
    private float movestep = 0.02f;
    private float moveinterval = 0.1f;
	public float multiplier = 1.0f;
    public float movemagnitude = 1.0f;
    float corruptionmod = 1.25f;

    bool finished = false; 
	private Vector2 enemydir = Vector2.right;

    Animator m_animator;

    public bool lifespan = false;
    private bool levelstart = false;
	private bool justhit = false;

    private int hitcount;
	public int direction = 1;

    private float age = 0;
    public float death;

    ParticleSystem particles;
    ParticleSystem.EmissionModule emissionModule;


    SpriteRenderer m_Renderer;

    [HideInInspector] public UnityEvent onWallHit;

    public bool isPaused { get; set; }

    void Awake()
    {
        onWallHit = new UnityEvent();
    }
    void Start()
    {
        defaultmovestep = movestep;
        particles = GetComponent<ParticleSystem>();
        emissionModule = particles.emission;
        m_Renderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(LevelStarting());
        if(direction == 2)
        {
            m_Renderer.flipX = false;
        }
        else
        {
            m_Renderer.flipX = true;
        }

        m_animator = GetComponent<Animator>();
    }

    IEnumerator LevelStarting()
    {
        if (levelstart == false)
        {
            yield return new WaitForSeconds(0.2f);
            levelstart = true;
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
			
		while (direction == 1) {
				
			if (justhit == true) {
				break;
			} else {

                if (coll.gameObject.tag == "Enemy")
                {
                    direction = 2;
                    hitcount += 1;
                    justhit = true;
                    m_Renderer.flipX = false;
                }

                if (coll.gameObject.tag != "Teleport" && coll.gameObject.tag != "Enemy")
                {
					direction = 2;
					justhit = true;
                    onWallHit.Invoke();
                    m_Renderer.flipX = false;
                }

				if (coll.gameObject.tag == "Teleport") {

					justhit = true;
				}

                if(coll.gameObject.tag == "Void")
                {
                    finished = true;
                    movespeed = 0;
                    movestep = 0;
                    m_animator.Play("EnemySuction");
                }

                if(coll.gameObject.tag == "Tail")
                {
                    coll.gameObject.SetActive(false);
                }
			}
		}

		while (direction == 2) {
				
			if (justhit == true) {
				break;
			} else {

                if (coll.gameObject.tag == "Enemy")
                {
                    direction = 1;
                    hitcount += 1;
                    justhit = true;
                    m_Renderer.flipX = true;
                }

                if (coll.gameObject.tag != "Teleport" && coll.gameObject.tag != "Enemy") {

					direction = 1;
					justhit = true;
                    onWallHit.Invoke();
                    m_Renderer.flipX = true;
                }

				if (coll.gameObject.tag == "Teleport") {

					justhit = true;
				}
                if (coll.gameObject.tag == "Void")
                {
                    finished = true;
                    movespeed = 0;
                    movestep = 0;
                    m_animator.Play("EnemySuction");
                }
            }
			}
		}

    void OnTriggerEnter2D(Collider2D coll)
    {
        var hit = coll.gameObject;

        if (hit.CompareTag("Corruption"))
        {
            movestep = movestep * corruptionmod;
            emissionModule.rateOverTime = 10f;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        var hit = coll.gameObject;
        if (hit.CompareTag("Corruption"))
        {
            movestep = defaultmovestep;
            emissionModule.rateOverTime = 0f;
        }
    }

    void Update()
    {
        if (!isPaused)
        {
            UnPausedUpdate();
        }
    }

        
	void LateUpdate(){
		justhit = false;
	}

	void Move() {

		Vector3 v = transform.position;

		transform.Translate (enemydir);
	}

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }


    public void OnPause() { }


    public void OnUnpause() { }

    public void UnPausedUpdate()
    {
            if (lifespan)
            {
                age += Time.smoothDeltaTime;
                if (age >= death)
                {
                    Destroy(gameObject);
                }
            }


            if (hitcount >= 10)
            {
                Destroy(gameObject);
            }

            if (levelstart == true)
            {
                this.movespeed += movestep * multiplier;
            }


            if (this.movespeed >= moveinterval)
            {
                Move();
                movespeed = movespeed - moveinterval;
            }

        if (!finished)
        {
            switch (direction)
            {

                case 2:
                    enemydir = (Vector2.left / 8) *  movemagnitude;
                    break;

                case 1:

                    enemydir = (Vector2.right / 8) * movemagnitude;
                    break;
            }
        }
           
        }
    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}

    