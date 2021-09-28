using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShooter : MonoBehaviour, IPausable
{

	private Vector3 Distance;
	private float DistanceFrom;
	public float firerate = 0.5f;
	private float nextfire = 0.0f;

	public Transform Spawnpoint;
	public Transform Spawnpoint2;
	public Transform Spawnpoint3;

	public GameObject ammo;
	public int shootertype = 1;
	public float shotspeed = 100.0f;

    public bool isPaused { get; set; }

    void Update()
	{
        if (!isPaused)
        {
			UnPausedUpdate();
        }
	}

	void Attacking(){

		switch (shootertype){
		case 3:
			if (Time.time > nextfire) {
				nextfire = Time.time + firerate;

				GameObject Shoot = Instantiate (ammo, Spawnpoint.position, Spawnpoint.rotation) as GameObject;
				GameObject Shoot2 = Instantiate (ammo, Spawnpoint2.position, Spawnpoint2.rotation) as GameObject;
				GameObject Shoot3 = Instantiate (ammo, Spawnpoint3.position, Spawnpoint3.rotation) as GameObject;
				Shoot.GetComponent<Rigidbody2D> ().AddForce (Spawnpoint.right * shotspeed);
				Shoot2.GetComponent<Rigidbody2D> ().AddForce (Spawnpoint2.right * shotspeed);
				Shoot3.GetComponent<Rigidbody2D> ().AddForce (Spawnpoint3.right * shotspeed);
			}
			break;
		case 2:
			if (Time.time > nextfire) {
				nextfire = Time.time + firerate;

				GameObject Shoot = Instantiate (ammo, Spawnpoint.position, Spawnpoint.rotation) as GameObject;
				GameObject Shoot2 = Instantiate (ammo, Spawnpoint2.position, Spawnpoint2.rotation) as GameObject;
				Shoot.GetComponent<Rigidbody2D> ().AddForce (Spawnpoint.right * shotspeed);
				Shoot2.GetComponent<Rigidbody2D> ().AddForce (Spawnpoint2.right * shotspeed);

			}
			break;

		case 1:
			if (Time.time > nextfire) {
				nextfire = Time.time + firerate;

				GameObject Shoot = Instantiate (ammo, Spawnpoint.position, Spawnpoint.rotation) as GameObject;
				Shoot.GetComponent<Rigidbody2D> ().AddForce (Spawnpoint.right * shotspeed);
			}
			break;
	}
}

	public void OnPause()
	{ }

	public void OnUnpause()
	{ }

    public void PausedUpdate()
    {}

    public void UnPausedUpdate()
    {
		Attacking();
    }
}