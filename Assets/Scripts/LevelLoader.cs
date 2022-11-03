using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	public int ID;

	public bool active = false;
    public bool isgateway = false;

	public bool completed = false;
	public bool golden = false;
    private bool caninput = false;

	public Sprite greensprite;
	public Sprite goldsprite;
    public Sprite completesprite;

	public Sprite UIdefault;
	public Sprite UIcomplete;
	public Sprite UIgolden;
	public Sprite UItimer;

	Animator m_Animator;
    private Pin pin;
    private int completerequired;

    public CanvasGroup canvas;
    public CanvasGroup SaveCanvas;
    public CanvasGroup LoadCanvas;

    public AudioClip LevelSelect;

    public MapManager mapmanager;

    public GamepadBackEnabler[] ButtonsEnabler;

    public OverworldCharacter character;

	void Start(){

		m_Animator = this.GetComponent<Animator> ();
        pin = this.GetComponent<Pin>();
        completerequired = pin.completerequired;
        //GameControl.onSingletonCheck.AddListener(PinVisualUpdate);
        character.onSelection.AddListener(DoThing);
    }

    private void DoThing()
    {
        if (active == true && caninput == true)
        {
            canvas.alpha = 255;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
            GameControl.control.levelID = ID;
            GameSoundManagement.instance.PlaySingle(LevelSelect);
            //mapmanager.checkLocked = true;

            if (SaveCanvas.alpha == 1)
            {
                SaveCanvas.alpha = 0;
                SaveCanvas.interactable = false;
                SaveCanvas.blocksRaycasts = false;
            }

            if (LoadCanvas.alpha == 1)
            {
                LoadCanvas.alpha = 0;
                LoadCanvas.interactable = false;
                SaveCanvas.blocksRaycasts = false;
            }
        }

        if (active == false)
        {
            caninput = false;
        }
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
        PinVisualUpdate();
    }

    void PinVisualUpdate()
    {

        if(ID > 100)
        {
            return;
        }

        bool complete = GameControl.control.completedlevels[ID];
        bool golden = GameControl.control.goldenpellets[ID];
        bool timer = GameControl.control.timerchallenge[ID];

        if (complete && timer && golden)
        {
            this.GetComponent<SpriteRenderer>().sprite = completesprite;
            m_Animator.SetBool("Rainbow", true);
            m_Animator.SetBool("Gold", false);
            m_Animator.SetBool("Green", false);
            m_Animator.SetBool("Red", false);
            return;
        }

        if (!complete)
        {
            m_Animator.SetBool("Red", true);
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = greensprite;
            m_Animator.SetBool("Green", true);
            m_Animator.SetBool("Red", false);
        }


        if (golden)
        {
            this.GetComponent<SpriteRenderer>().sprite = goldsprite;
            m_Animator.SetBool("Gold", true);
            m_Animator.SetBool("Green", false);
            m_Animator.SetBool("Red", false);
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
            canvas.blocksRaycasts = false;
        }
	}

    void CanvasEnable()
    {
        canvas.alpha = 255;
        canvas.blocksRaycasts = true;
    }

    private void LateUpdate()
    {
        if(canvas.alpha == 1)
        {
            foreach (GamepadBackEnabler button in ButtonsEnabler)
            {
                button.selectionLock = false;
            }
        }
        else
        {
            foreach (GamepadBackEnabler button in ButtonsEnabler)
            {
                button.selectionLock = true;
            }
        }
    }
    private void OnDisable()
    {
        GameControl.onSingletonCheck.RemoveListener(PinVisualUpdate);
        character.onSelection.RemoveListener(DoThing);
    }
}