using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public CanvasGroup SaveCanvas;
    public CanvasGroup LoadCanvas;
    public CanvasGroup LevelCanvas;

    public Button backButton;
    public Button playButton;

    public AudioClip backsound;
    public AudioClip playsound;

    private GameSoundManagement sound;

    public GamepadBackEnabler[] ButtonsEnabler;

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

        if (GameInput.GetKeyUp("up"))
        {
            character.TrySetDirection(Direction.Up);
        }
        else if (GameInput.GetKeyUp("down"))
        {
            character.TrySetDirection(Direction.Down);
        }
        else if (GameInput.GetKeyUp("left"))
        {
            character.TrySetDirection(Direction.Left);
        }
        else if (GameInput.GetKeyUp("right"))
        {
            character.TrySetDirection(Direction.Right);
        }
        else if (GameInput.GetKeyUp("pause"))
        {
            menuCanvas.alpha = 1;
            menuCanvas.blocksRaycasts = true;
            menuCanvas.interactable = true;
            Checklocked = true;
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

    private void LateUpdate()
    {
        if (menuCanvas.alpha == 1 ^ SaveCanvas.alpha == 1 ^ LoadCanvas.alpha == 1)
        {
            foreach (GamepadBackEnabler button in ButtonsEnabler)
            {
                button.selectionLock = false;
            }
        }
    }

    public void OnDisable()
    {
        backButton.onClick.RemoveListener(() => sound.PlaySingle(backsound));
        playButton.onClick.RemoveListener(() => sound.PlaySingle(playsound));
    }
}
