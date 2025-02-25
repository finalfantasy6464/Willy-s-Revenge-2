using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovementFour : MonoBehaviour, IPausable
{
	int waypointIndex = 0;

	public int FollowerCount = 0;

	public int offsetFromLeader;
	public int followDelay;

	public int flipnumber = 0;
	float defaultmovespeed;
	public float movespeed = 2f;
	float corruptionmod = 0.5f;
	public float rotatespeed = 8f;
	public Transform[] waypoints;

	Collider2D m_Collider;
	public Sprite defaultsprite;
	SpriteRenderer sprite;

	private List<Vector3> PositionHistory = new List<Vector3>();
	private List<Quaternion> RotationHistory = new List<Quaternion>();
	private List<Sprite> SpriteHistory = new List<Sprite>();
	private List<bool> ColliderHistory = new List<bool>();
	private List<int> SortingHistory = new List<int>();

	public List<GameObject> FollowerList = new List<GameObject>();

	public bool isPaused { get; set; }

    void Start()
	{
		defaultmovespeed = movespeed;
		transform.position = waypoints[waypointIndex].transform.position;

		if (FollowerCount > 0)
		{
			foreach (GameObject follower in FollowerList)
			{
				follower.transform.position = transform.position;
			}
		}
		m_Collider = GetComponent<Collider2D>();
		sprite = GetComponent<SpriteRenderer>();
	}

     void Update(){
        if (!isPaused)
        {
			UnPausedUpdate();
        }
	}

	void Move() {
		transform.position = Vector3.MoveTowards (transform.position, waypoints [waypointIndex].transform.position, movespeed * Time.deltaTime);
		transform.rotation = Quaternion.Lerp (transform.rotation, waypoints [waypointIndex].transform.rotation, rotatespeed * Time.deltaTime);

		if (transform.position == waypoints [waypointIndex].transform.position) {
			waypointIndex += 1;
		}

		if (waypointIndex == waypoints.Length)
        {
			waypointIndex = 0;
		}

		if (waypointIndex == flipnumber && flipnumber != -1) {
			m_Collider.enabled = true;
			sprite.sprite = defaultsprite;
			sprite.sortingOrder = 3;
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		var hit = coll.gameObject;

		if (hit.CompareTag("Corruption"))
		{
			movespeed = movespeed * corruptionmod;
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		var hit = coll.gameObject;
		if (hit.CompareTag("Corruption"))
		{
			movespeed = defaultmovespeed;
		}
	}

	void UpdateHistoryList()
	{
		PositionHistory.Insert(0, transform.position);
		RotationHistory.Insert(0, transform.rotation);
		ColliderHistory.Insert(0, m_Collider.enabled ? true : false);
		SpriteHistory.Insert(0, sprite.sprite);
		SortingHistory.Insert(0, sprite.sortingOrder);

		if (PositionHistory.Count > 200)
		{
			PositionHistory.RemoveRange(200, PositionHistory.Count - 200);
		}

		if (RotationHistory.Count > 200)
		{
			RotationHistory.RemoveRange(200, RotationHistory.Count - 200);
		}

		if (ColliderHistory.Count > 200)
        {
			ColliderHistory.RemoveRange(200, ColliderHistory.Count - 200);
        }

		if (SpriteHistory.Count > 200)
        {
			SpriteHistory.RemoveRange(200, SpriteHistory.Count - 200);
        }

		if (SortingHistory.Count > 200)
		{
			SortingHistory.RemoveRange(200, SortingHistory.Count - 200);
		}
	}


	void UpdateFollowerStatus()
	{
		int delay;

		for (int i = 0; i < FollowerList.Count; i++)
		{
			if (!FollowerList[i].activeInHierarchy)
				break;

			delay = offsetFromLeader + (followDelay * i);

			if (PositionHistory.Count > delay)
			{
				FollowerList[i].GetComponent<Collider2D>().enabled = ColliderHistory[delay];
				FollowerList[i].GetComponent<SpriteRenderer>().sprite = SpriteHistory[delay];
				FollowerList[i].GetComponent<SpriteRenderer>().sortingOrder = SortingHistory[delay];
				FollowerList[i].transform.position = PositionHistory[delay];
				FollowerList[i].transform.rotation = RotationHistory[delay];
			}
		}
	}

	public void OnPause()
	{}

	public void OnUnpause()
	{}

    public void UnPausedUpdate()
    {
		Move();
		if(FollowerCount > 0)
        {
			UpdateFollowerStatus();
			UpdateHistoryList();
		}
	}

	public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}