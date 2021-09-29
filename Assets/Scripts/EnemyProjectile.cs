using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour, IPausable
{

	public GameObject bullet;

	public float fireRate;
	private float nextFire;

    public bool isPaused { get; set; }

    // Start is called before the first frame update
    void Start()
    {
		nextFire = Time.time;
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
		if (Time.time > nextFire){
			Instantiate (bullet, transform.position, Quaternion.identity);
			nextFire = Time.time + fireRate;
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