using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour, IPausable
{
	public bool pingpongs = false;
	public float spinamount;
	private float spintimer;

    public float angleSize;

    private float maxRotation;
    private float minRotation;

    public bool rotatesother;

    public float rotationdegrees;
    public float progress;
    public float timespinning = 1.0f;
	public int currentdir = 1;

    private Quaternion rotation;
    private float startingZangle;

    public bool isPaused { get; set; }

    // Update is called once per frame
    private void Start()
    {
        startingZangle = transform.eulerAngles.z;
        rotation = this.transform.rotation;
    }
    void Update ()
    {
        if (!isPaused)
        {
            UnPausedUpdate();
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
        spintimer += Time.deltaTime;

        if (pingpongs == true)
        {
            float rZ = startingZangle + Mathf.Lerp(0, angleSize * 2, Mathf.PingPong(spintimer * spinamount, 1));
            transform.rotation = Quaternion.Euler(0, 0, rZ);
        }
        transform.Rotate(new Vector3(0, 0, spinamount));
    }

    public void OnDestroy()
    {
        PauseControl.TryRemovePausable(gameObject);
    }
}

