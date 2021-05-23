using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	public int ID;

	public bool active = false;
    public bool isgateway = false;

	public bool completed = false;
	public bool golden = false;
    private bool caninput = false;
    private bool changed = false;

	public Sprite greensprite;
	public Sprite goldsprite;

	public Sprite UIdefault;
	public Sprite UIcomplete;
	public Sprite UIgolden;
	public Sprite UItimer;

	Animator m_Animator;
    private Pin pin;
    private int completerequired;

    public CanvasGroup canvas;
    public GameObject SaveCanvas;
    public GameObject LoadCanvas;

    public AudioClip LevelSelect;

    public MapManager mapmanager;

	void Start(){

		m_Animator = this.GetComponent<Animator> ();
        pin = this.GetComponent<Pin>();
        completerequired = pin.completerequired;
    }

    [HideInInspector]public IEnumerator InputTimer()
    {
        if(caninput == false)
        {
            yield return 1;
            caninput = true;
        }
    }
    void Update()
    {

        if (GameControl.control.completedlevels[ID] == true)
        {
            this.GetComponent<SpriteRenderer>().sprite = greensprite;
            m_Animator.SetTrigger("Green");
        }

        if (GameControl.control.goldenpellets[ID] == true)
        {
            this.GetComponent<SpriteRenderer>().sprite = goldsprite;
            m_Animator.SetTrigger("Gold");
        }

        if (Input.GetKey(KeyCode.Space) & active == true & caninput == true) {
            canvas.alpha = 255;
            canvas.interactable = true;
            GameControl.control.levelID = ID;
            GameSoundManagement.instance.PlaySingle(LevelSelect);
            mapmanager.Checklocked = true;

            if (SaveCanvas.gameObject.activeSelf == true)
            {
                SaveCanvas.gameObject.SetActive(false);
            }

            if (LoadCanvas.gameObject.activeSelf == true)
            {
                LoadCanvas.gameObject.SetActive(false);
            }
        }

        if (active == false)
        {
            caninput = false;
        }
    }

	void OnTriggerEnter2D(Collider2D coll){

		if (coll.gameObject.tag == "Player") {
			active = true;
            StartCoroutine(InputTimer());

            GameObject completeEmblemObject = GameObject.Find ("CompleteEmblem");
			completeEmblemObject.GetComponent<Image>().sprite = UIdefault;
			GameObject timeEmblemObject = GameObject.Find ("TimeEmblem");
			timeEmblemObject.GetComponent<Image>().sprite = UIdefault;
			GameObject goldenEmblemObject = GameObject.Find ("GoldenEmblem");
            goldenEmblemObject.GetComponent<Image>().sprite = UIdefault;

			if (GameControl.control.completedlevels [ID] == true) {
				completeEmblemObject.GetComponent<Image> ().sprite = UIcomplete;
			}

			if (GameControl.control.timerchallenge [ID] == true) {
				timeEmblemObject.GetComponent<Image> ().sprite = UItimer;
			}

			if (GameControl.control.goldenpellets [ID] == true) {
				goldenEmblemObject.GetComponent<Image> ().sprite = UIgolden;

            if (isgateway == true)
                {
                    UIdefault = null;
                    completeEmblemObject.GetComponent<Image>().sprite = null;
                    timeEmblemObject.GetComponent<Image>().sprite = null;
                    goldenEmblemObject.GetComponent<Image>().sprite = null;
                }

            }
        }
	  } 

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			active = false;
            canvas.alpha = 0;
            canvas.interactable = false;
        }
	}

    void CanvasEnable()
    {
        canvas.alpha = 255;
    }
}

