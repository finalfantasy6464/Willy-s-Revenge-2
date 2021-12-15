using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorStrike : MonoBehaviour, IPausable
{
	public Vector2 center;
	public Vector2 size;

	private float trigger;
	public float firerate;

    public int firearcmin = 0;
    public int firearcmax = 360;

	public GameObject Meteor;

    public bool isPaused { get; set; }

    void Update(){

        if (!isPaused)
            UnPausedUpdate();
	}

	public void SpawnMeteor()
    {
		Vector2 pos = center + new Vector2 (Random.Range(-size.x / 2, size.x / 2),Random.Range(-size.y / 2, size.y / 2));
		GameObject newMeteor = Instantiate (Meteor, pos, Quaternion.Euler(0,0,Random.Range(firearcmin,firearcmax)));
        PauseControl.TryAddPausable(newMeteor);
    }

	void OnDrawGizmosSelected()
    {
		Gizmos.color = new Color (0.5f, 0.0f, 0.5f, 0.25f);
		Gizmos.DrawCube (center, size);
	}

    public void OnPause()
    { }

    public void OnUnpause()
    { }

    public void UnPausedUpdate()
    {
        trigger += Time.deltaTime;

        if (trigger >= firerate)
        {
            trigger = 0.0f;
            SpawnMeteor();
        }
    }
    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
