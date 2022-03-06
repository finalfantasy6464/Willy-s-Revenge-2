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
    public OverworldGUI mainGUI;

    public GamepadBackEnabler[] ButtonsEnabler;

	void Start(){

		m_Animator = this.GetComponent<Animator> ();
        pin = this.GetComponent<Pin>();
        completerequired = pin.completerequired;
        //GameControl.onSingletonCheck.AddListener(PinVisualUpdate);
    }

    public IEnumerator InputTimer()
    {
        if(!caninput)
        {
            yield return 1;
            caninput = true;
        }
    }

    void Update()
    {
        PinVisualUpdate();

        if (GameInput.GetKeyDown("select") && active && caninput && !mainGUI.levelPreview.isShowing)
        {
            SetCanvas(canvas, true);
            GameControl.control.levelID = ID;
            GameSoundManagement.instance.PlaySingle(LevelSelect);
            //mapmanager.checkLocked = true;

            if (SaveCanvas.alpha == 1f)
                SetCanvas(SaveCanvas, false);

            if (LoadCanvas.alpha == 1f)
                SetCanvas(LoadCanvas, false);
        }

        if (!active)
            caninput = false;
    }

    void PinVisualUpdate()
    {
        if(ID > 100)
            return;

        SpriteRenderer loaderRenderer = GetComponent<SpriteRenderer>();
        bool complete = GameControl.control.completedlevels[ID];
        bool golden = GameControl.control.goldenpellets[ID];
        bool timer = GameControl.control.timerchallenge[ID];

        if (complete && timer && golden)
        {
            loaderRenderer.sprite = completesprite;
            m_Animator.SetBool("Rainbow", true);
            m_Animator.SetBool("Gold", false);
            m_Animator.SetBool("Green", false);
            m_Animator.SetBool("Red", false);
            return;
        }

        if (!complete)
            m_Animator.SetBool("Red", true);
        else
        {
            loaderRenderer.sprite = greensprite;
            m_Animator.SetBool("Green", true);
            m_Animator.SetBool("Red", false);
        }

        if (golden)
        {
            loaderRenderer.sprite = goldsprite;
            m_Animator.SetBool("Gold", true);
            m_Animator.SetBool("Green", false);
            m_Animator.SetBool("Red", false);
        }
    }

	void OnTriggerEnter2D(Collider2D coll)
    {
		if (coll.gameObject.tag != "Player")
            return;
        
		active = true;
        StartCoroutine(InputTimer());

        Image completeRenderer = GameObject.Find("CompleteEmblem").GetComponent<Image>();
        Image timeRenderer = GameObject.Find("TimeEmblem").GetComponent<Image>();
        Image goldenRenderer = GameObject.Find("GoldenEmblem").GetComponent<Image>();

        completeRenderer.sprite = UIdefault;
        timeRenderer.sprite = UIdefault;
        goldenRenderer.sprite = UIdefault;

		if (GameControl.control.completedlevels[ID])
			completeRenderer.sprite = UIcomplete;

		if (GameControl.control.timerchallenge[ID])
			timeRenderer.sprite = UItimer;

		if (GameControl.control.goldenpellets[ID])
        {
			goldenRenderer.sprite = UIgolden;
            if (isgateway)
            {
                UIdefault = null;
                completeRenderer.sprite = null;
                timeRenderer.sprite = null;
                goldenRenderer.sprite = null;
            }
        }
	} 

	void OnTriggerExit2D(Collider2D coll)
    {
		if (coll.gameObject.tag == "Player")
        {
			active = false;
            SetCanvas(canvas, false);
        }
	}

    void CanvasEnable()
    {
        canvas.alpha = 255;
        canvas.blocksRaycasts = true;
    }

    void SetCanvas(CanvasGroup g, bool value)
    {
        g.alpha = value ? 1f : 0f;
        g.interactable = value;
        g.blocksRaycasts = value;
    }

    private void LateUpdate()
    {
        if(canvas.alpha == 1)
        {
            foreach (GamepadBackEnabler button in ButtonsEnabler)
                button.selectionLock = false;
        }
        else
        {
            foreach (GamepadBackEnabler button in ButtonsEnabler)
                button.selectionLock = true;
        }
    }

    private void OnDisable()
    {
        GameControl.onSingletonCheck.RemoveListener(PinVisualUpdate);
    }
}

