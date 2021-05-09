using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
	public Character Character;

	public Pin StartPin;
	public Pin TargetPin;
    public Pin PreviousPin;

	public Text SelectedLevelText;
    public Text SelectedLevelParTime;
    public Image SelectedLevelPreviewImage;

	private GameObject[] allObjects;
	private GameObject pinObject;

    public CanvasGroup menuCanvas;
    public GameObject SaveCanvas;
    public GameObject LoadCanvas;
    public GameObject LevelCanvas;

	/// <summary>
	/// Use this for initialization
	/// </summary>
	private void Start ()
	{
		if (GameControl.control.levelID != 0) {
			pinObject = GameObject.Find ("LP" + GameControl.control.levelID.ToString ());
			StartPin = pinObject.GetComponent<Pin> ();
        }

		// Pass a ref and default the player Starting Pin
		Character.Initialise (this, StartPin);
	}

	/// <summary>
	/// This runs once a frame
	/// </summary>
	private void Update()
	{
        // Only check input when character is stopped
        if (Character.IsMoving) return;
		
		// First thing to do is try get the player input
		CheckForInput();
	}

	
	/// <summary>
	/// Check if the player has pressed a button
	/// </summary>
	private void CheckForInput()
	{
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Character.TrySetDirection(Direction.Up);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Character.TrySetDirection(Direction.Down);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Character.TrySetDirection(Direction.Left);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Character.TrySetDirection(Direction.Right);
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
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

	
	/// <summary>
	/// Update the GUI text
	/// </summary>
	public void UpdateGui()
	{
		SelectedLevelText.text = string.Format("{0}", Character.CurrentPin.SceneToLoad);
        SelectedLevelParTime.text = string.Format("{0}", Character.CurrentPin.ParTime);
        SelectedLevelPreviewImage.sprite = Character.CurrentPin.previewimage;
    }
}
