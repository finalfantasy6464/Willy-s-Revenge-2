using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorState : MonoBehaviour
{
    GameObject Switch1;
    SwitchCollide switchcollide;
    public GameObject Pulley;
    Vector3 initialposition;
    public Transform targetposition;

    private Vector3 SlideVector;
    bool SlideUp = false;
    bool complete = false;


    public AudioClip SlidingClose;
    public AudioClip SlidingOpen;

    float lerpDuration = 0.25f;

    public PositionalSFX sfx;

    // Start is called before the first frame update
    void Start()
    {
        SlideVector = new Vector3(0f, 0.72f, 0f);
        initialposition = this.transform.position;
        Switch1 = GameObject.FindGameObjectWithTag("Switch");
        switchcollide = Switch1.GetComponent<SwitchCollide>();
        switchcollide.onSwitchToggle.AddListener(OnToggle);
    }

    IEnumerator SlideCoroutine(bool value)
    {
            float timeElapsed = 0;
        if (value)
        {
            while (timeElapsed < lerpDuration)
            {
                transform.position = Vector3.Lerp(initialposition, targetposition.position, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime * 1.5f;
                yield return null;
            }
            transform.position = targetposition.position;
        }
        else
        {
            while (timeElapsed < lerpDuration)
            {
                transform.position = Vector3.Lerp(targetposition.position, initialposition, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime * 1.5f;
                yield return null;
            }
            transform.position = initialposition;
        }
    }

    void OnToggle(bool value)
    {
        if (value == false){
            sfx.clip = SlidingClose;
            Pulley.GetComponent<Animator>().Play("PullRopeDown");
            StartCoroutine(SlideCoroutine(false));
        }
        else
        {
            sfx.clip = SlidingOpen;
            Pulley.GetComponent<Animator>().Play("PullRopeUp");
            StartCoroutine(SlideCoroutine(true));
        }
        sfx.PlayPositionalSound();
    }
}
