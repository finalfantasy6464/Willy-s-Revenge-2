using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour, IPausable
{

    public GameObject bullet;

    public float fireRate;
    private float nextFire;

    public float bulletscale = 1.0f;
    public float shotspeed = 1.0f;
    private float truespeed;

    private bool firing;

    Transform player;
    Vector2 firingangle;
 
    public bool isPaused { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        truespeed = shotspeed * 10;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            UnPausedUpdate();
        }
    }

	void CheckIfTimeToFire()
	{
        nextFire += Time.deltaTime;

		if (nextFire >= fireRate)
        {
            nextFire -= nextFire;
            Fire();
        }
    }

    void Fire()
    {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.transform.localScale = newBullet.transform.localScale * bulletscale;
            PauseControl.TryAddPausable(newBullet);

            if (newBullet.TryGetComponent(out Rigidbody2D bulletBody))
            {
                if(player != null)
            {
                firingangle = player.position - transform.position;
            }
                bulletBody.AddForce((firingangle.normalized) * truespeed);
                newBullet.GetComponent<Aimedbullet>().SetForce((firingangle.normalized) * (truespeed / 2));
            }
    }

    public void OnPause()
    { }

    public void OnUnpause()
    { }

    public void PausedUpdate()
    { }

    public void UnPausedUpdate()
    {
        CheckIfTimeToFire();
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}