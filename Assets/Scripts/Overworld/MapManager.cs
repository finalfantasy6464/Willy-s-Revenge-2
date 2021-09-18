using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MapManager : MonoBehaviour
{
	public Character character;

	public Pin StartPin;
	public Pin TargetPin;
    public Pin PreviousPin;

	public Text SelectedLevelText;
    public Text SelectedLevelParTime;
    public Image SelectedLevelPreviewImage;

    public bool Checklocked = false;

	private GameObject[] allObjects;
	private GameObject pinObject;

    public CanvasGroup menuCanvas;
    public GameObject SaveCanvas;
    public GameObject LoadCanvas;
    public GameObject LevelCanvas;

    public Button backButton;
    public Button playButton;

    public AudioClip backsound;
    public AudioClip playsound;

    private GameSoundManagement sound;

    public List<GatePin> worldgates = new List<GatePin>();

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start ()
	{
        character = GameObject.Find("Character").GetComponent<Character>();

        if(sound == null)
        {
            sound = GameObject.Find("SoundManager").GetComponent<GameSoundManagement>();
        }

		if (GameControl.control.levelID != 0) {
			pinObject = GameObject.Find ("LP" + GameControl.control.levelID.ToString ());
			StartPin = pinObject.GetComponent<Pin> ();
            character.transform.position = StartPin.transform.position;
        }

        sound.GetComponent<MusicManagement>().onOverworld.Invoke();

        backButton.onClick.AddListener(()=> sound.PlaySingle(backsound));
        playButton.onClick.AddListener(()=> sound.PlaySingle(playsound));


        // Pass a ref and default the player Starting Pin
        character.Initialise (this, StartPin);

        GameControl.control.lockedgates.Clear();
        GameControl.control.destroyedgates.Clear();

        if (GameControl.control.lockedgatescache.Count == 0)
        {
            for (int i = 0; i < worldgates.Count; i++)
            {
                GameControl.control.lockedgates.Add(true);
                GameControl.control.destroyedgates.Add(false);
            }
        }
        else
        {
            for (int i = 0; i < GameControl.control.lockedgatescache.Count; i++)
            {
                GameControl.control.lockedgates.Add(false);
                GameControl.control.destroyedgates.Add(false);
                GameControl.control.lockedgates[i] = GameControl.control.lockedgatescache[i];
                GameControl.control.destroyedgates[i] = GameControl.control.destroyedgatescache[i];
            }
        }
    }

	/// <summary>
	/// This runs once a frame
	/// </summary>
	private void Update()
	{
        // Only check input when character is stopped
        if (character.IsMoving) return;

        // First thing to do is try get the player input
        if (!Checklocked)
        {
            CheckForInput();
        }
	}

    public void CheckUnlocker()
    {
        Checklocked = false;
    }

	/// <summary>
	/// Check if the player has pressed a button
	/// </summary>
	private void CheckForInput()
	{
        //var gamepad = Gamepad.current;
        //if(gamepad == null)
        //{
        //    return;
        //}

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) 
                || Input.GetAxisRaw("Vertical") == 1 /*|| gamepad.dpad.up.isPressed*/)
        {
            character.TrySetDirection(Direction.Up);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S) 
                || Input.GetAxisRaw("Vertical") == -1 /*|| gamepad.dpad.down.isPressed*/)
        {
            character.TrySetDirection(Direction.Down);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) 
                || Input.GetAxisRaw("Horizontal") == -1 /*|| gamepad.dpad.left.isPressed*/)
        {
            character.TrySetDirection(Direction.Left);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) 
                || Input.GetAxisRaw("Horizontal") == 1 /*|| gamepad.dpad.right.isPressed*/)
        {
            character.TrySetDirection(Direction.Right);
        }
        else if (Input.GetKeyUp(KeyCode.Escape) /*|| gamepad.startButton.isPressed*/)
        {
            menuCanvas.alpha = 1;
            menuCanvas.interactable = true;

            if(SaveCanvas.gameObject.activeSelf == true)
            {
                SaveCanvas.gameObject.SetActive(false);
            }

            if(LoadCanvas.gameObject.activeSelf == true)
            {
                LoadCanvas.gameObject.SetActive(false);
            }

            if(LevelCanvas.gameObject.activeSelf == true)
            {
                LevelCanvas.gameObject.SetActive(false);
            }
        }
	}

    public void SetWorldGateData(List<bool> lockedgates, List<bool> destroyedgates)
    {
        for (int i = 0; i < worldgates.Count; i++)
        {
            worldgates[i].locked = lockedgates[i];
            worldgates[i].destroyed = destroyedgates[i];
            worldgates[i].OnLevelLoaded.Invoke();
        }
    }

    /// <summary>
    /// Update the GUI text
    /// </summary>
    public void UpdateGui()
	{
        SelectedLevelText.text = string.Format("{0}", character.CurrentPin.SceneToLoad);
        SelectedLevelParTime.text = string.Format("{0}", character.CurrentPin.ParTime);
        SelectedLevelPreviewImage.sprite = character.CurrentPin.previewimage;
    }

    public void OnDisable()
    {
        backButton.onClick.RemoveListener(() => sound.PlaySingle(backsound));
        playButton.onClick.RemoveListener(() => sound.PlaySingle(playsound));
    }
}
