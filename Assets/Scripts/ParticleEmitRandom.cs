using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitRandom : MonoBehaviour, IPausable
{
	public Vector2 center;
	public Vector2 size;

	private float trigger;
	public float firerate;

    public float fireratemin;
    public float fireratemax;

	public GameObject Particle;

    public bool isPaused { get; set; }

    void Update(){

        if (!isPaused)
            UnPausedUpdate();
	}

	public void SpawnParticle()
    {
		Vector2 pos = center + new Vector2 (Random.Range(-size.x / 2, size.x / 2),Random.Range(-size.y / 2, size.y / 2));
		GameObject newParticle = Instantiate (Particle, pos, Quaternion.identity);
        PauseControl.TryAddPausable(newParticle);
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
            SpawnParticle();
            SetNewFireRate();
        }
    }

    private void SetNewFireRate()
    {
        firerate = Random.Range(fireratemin, fireratemax);
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}
