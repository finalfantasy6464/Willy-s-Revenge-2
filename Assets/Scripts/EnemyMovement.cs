using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour, IPausable
{

	private float movespeed = 0.0f;
    private float movestep = 0.02f;
    private float moveinterval = 0.1f;
	public float multiplier = 1.0f;

	private Vector2 enemydir = Vector2.right;

    public bool lifespan = false;
    private bool levelstart = false;
	private bool justhit = false;

    private int hitcount;
	public int direction = 1;

    private float age = 0;
    public float death;

    [HideInInspector] public UnityEvent onWallHit;

    public bool isPaused { get; set; }

    void Awake()
    {
        onWallHit = new UnityEvent();
    }
    void Start()
    {
        StartCoroutine(LevelStarting());
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
                }

                if (coll.gameObject.tag != "Teleport" && coll.gameObject.tag != "Enemy")
                {
					direction = 2;
					justhit = true;
                    onWallHit.Invoke();
				}

				if (coll.gameObject.tag == "Teleport") {

					justhit = true;
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
                }

                if (coll.gameObject.tag != "Teleport" && coll.gameObject.tag != "Enemy") {

					direction = 1;
					justhit = true;
                    onWallHit.Invoke();
                }

				if (coll.gameObject.tag == "Teleport") {

					justhit = true;
				}

				}
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

    public void OnPause() { }


    public void OnUnpause() { }

    public void PausedUpdate() { }

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

            switch (direction)
            {

                case 2:
                    enemydir = Vector2.left / 8;
                    break;

                case 1:

                    enemydir = Vector2.right / 8;
                    break;
            }
        }
    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}

    