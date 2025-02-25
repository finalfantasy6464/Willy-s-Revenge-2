using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementThree : MonoBehaviour, IPausable
{
	public int waypointIndex = 0;
	public int FollowerCount = 0;

	public int offsetFromLeader;
	public int followDelay;

    public bool usesParticles = false;

	public float movespeed = 2f;
	public Transform[] waypoints;

	public int emissionrate = 500;

	public bool stopsonRoute = false;
	public float stopcounter;
	public float stoptime = 0.5f;
	ParticleSystem particles;
	ParticleSystem.EmissionModule emissionModule;

	private List<Vector3> PositionHistory = new List<Vector3>();

	public List<GameObject> FollowerList = new List<GameObject>();

	public bool isPaused { get; set; }

    void Start(){
		transform.position = waypoints[waypointIndex].transform.position;

		if(FollowerCount > 0)
        {
			foreach (GameObject follower in FollowerList)
			{
				follower.transform.position = transform.position;
			}
		}
        if (usesParticles)
        {
			StartCoroutine(startParticleRoutine());
        }
	}

	IEnumerator startParticleRoutine()
    {
		yield return 2;
		particles = GetComponent<ParticleSystem>();
		emissionModule = particles.emission;
		emissionModule.rateOverTime = emissionrate;
		yield break;
	}

     void Update(){
        if (!isPaused)
        {
			UnPausedUpdate();
        }
	}

	void Move() {
        if (!stopsonRoute)
        {
			transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, movespeed * Time.smoothDeltaTime);

			if (transform.position == waypoints[waypointIndex].transform.position)
			{
				waypointIndex += 1;
			}

			if (waypointIndex >= waypoints.Length)
				waypointIndex = 0;
		}

		if(stopsonRoute)
		{
			stopcounter += Time.deltaTime;

			if(stopcounter >= stoptime)
			transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, movespeed * Time.smoothDeltaTime);

			if (transform.position == waypoints[waypointIndex].transform.position)
			{
				waypointIndex += 1;
				stopcounter = 0;
			}

			if (waypointIndex >= waypoints.Length)
				waypointIndex = 0;
		}
	}


	void UpdateHistoryList()
	{
		PositionHistory.Insert(0, transform.position);

		if (PositionHistory.Count > 1000)
		{
			PositionHistory.RemoveRange(1000, PositionHistory.Count - 1000);
		}
	}


	void UpdateFollowerPosition()
	{
		int delay;

		for (int i = 0; i < FollowerList.Count; i++)
		{
			if (!FollowerList[i].activeInHierarchy)
				break;

			delay = offsetFromLeader + (followDelay * i);

			FollowerList[i].transform.rotation = Quaternion.Euler(0, 0, 0);
			if (PositionHistory.Count > delay)
			{
				FollowerList[i].transform.position = PositionHistory[delay];
			}
		}
	}

	public void OnPause()
    {
		if(particles != null)
        {
			particles.Pause();
        }
	}

    public void OnUnpause()
    {
		if (particles != null)
		{
			particles.Play();
		}
	}

    public void UnPausedUpdate()
    {
		Move();
		if(FollowerCount > 0)
        {
			UpdateFollowerPosition();
			UpdateHistoryList();
		}
	}

	public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}