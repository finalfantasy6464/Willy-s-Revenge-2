using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingShooter : MonoBehaviour, IPausable
{

	private Vector3 Distance;
	private float DistanceFrom;
	public float fireprogress = 0.0f;
	public float firerate = 0.5f;
	private float nextfire = 0.0f;

	public Transform Spawnpoint;
	public Transform Spawnpoint2;
	public Transform Spawnpoint3;
	public List<Transform> spawnPoints;

	public GameObject ammo;
	public int shootertype = 1;
	public float shotspeed = 100.0f;

    public bool isPaused { get; set; }


	void Start()
	{
		spawnPoints = new List<Transform>();
		if(Spawnpoint != null) spawnPoints.Add(Spawnpoint);
		if(Spawnpoint2 != null) spawnPoints.Add(Spawnpoint2);
		if(Spawnpoint3 != null) spawnPoints.Add(Spawnpoint3);
	}

    void Update()
	{
        if (!isPaused)
			UnPausedUpdate();
	}

	void Attacking()
	{
		fireprogress += Time.deltaTime;
		if (fireprogress >= nextfire)
		{
			nextfire = firerate;
			fireprogress -= fireprogress;
			for (int i = 0; i < shootertype; i++)
			{
				GameObject newBullet = Instantiate(ammo, spawnPoints[i].position, spawnPoints[i].rotation);
				if(newBullet.TryGetComponent(out Rigidbody2D bulletBody))
				{
					bulletBody.AddForce(spawnPoints[i].right * shotspeed);
					newBullet.GetComponent<Bullet>().SetForce(spawnPoints[i].right * shotspeed);
				}
				PauseControl.TryAddPausable(newBullet);
			}
		}
	}

	public void OnPause() {}

	public void OnUnpause() {}

    public void UnPausedUpdate()
    {
		Attacking();
    }
	public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}