using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SaveLoadPanel : GUIWindow
{
    [Space]
    public SaveFileRow[] saveFileRows;
    public SaveFileRow highlightedSaveRow;
    public SaveFileRow selectedSaveRow;
    [Space]
    public Button saveCurrentButton;
    public Button loadCurrentButton;
    public Button deleteCurrentButton;

    public GUIWindow confirmationWindow;
    public TextMeshProUGUI confirmationLabel;
    public ScriptableGameState gameState;

    string confirmationString;
    bool initialized;

    void Awake()
    {
        OnShow += HighlightFirst;
   }

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    void Update()
    {
        if(!isShowing) return;

        if(GameInput.GetKeyDown("select"))
            ToggleSelection();

        if(GameInput.GetKeyDown("up"))
            MoveHighlightUp();
        else if(GameInput.GetKeyDown("down"))
            MoveHighlightDown();
    }

    void MoveHighlightDown()
    {
        Debug.Log("down");
        for (int i = 0; i < saveFileRows.Length; i++)
        {
            if(saveFileRows[i].isHighlighted)
            {
                saveFileRows[i].SetHighlighted(false);
                saveFileRows[i == saveFileRows.Length - 1 ? 0 : i + 1].SetHighlighted(true);
                break;
            }
        }
    }

    void MoveHighlightUp()
    {
        Debug.Log("up");
        for (int i = 0; i < saveFileRows.Length; i++)
        {
            if(saveFileRows[i].isHighlighted)
            {
                saveFileRows[i].SetHighlighted(false);
                saveFileRows[saveFileRows.Length == 0 ? saveFileRows.Length - 1 : i - 1].SetHighlighted(true);
                break;
            }
        }
    }

    IEnumerator DelayedStart()
    {
        yield return null;
        for (int i = 0; i < saveFileRows.Length; i++)
        {
            GameStatePreview preview = new GameStatePreview(i);
            if(preview.isEmpty)
                saveFileRows[i].SetEmpty();
            else
                saveFileRows[i].SetFromStatePreview(preview);
        }
    }

    void HighlightFirst()
    {
        saveFileRows[0].SetHighlighted(true);
        highlightedSaveRow = saveFileRows[0];
    }

    void ToggleSelection()
    {
        selectedSaveRow = null;
        foreach (SaveFileRow row in saveFileRows)
        {
            if(row.isSelected)
                row.isSelected = false;

            if(row.isHighlighted)
            {
                row.isHighlighted = false;
                row.isSelected = true;    
            }
        }
        
        SetButtons(selectedSaveRow != null,
                selectedSaveRow != null && !selectedSaveRow.isEmpty,
                selectedSaveRow != null && !selectedSaveRow.isEmpty);
    }

    void SaveCurrent()
    {
        GameControl.control.Save(selectedSaveRow.saveSlot);
        selectedSaveRow.SetFromControl(GameControl.control);
    }

    void LoadCurrent()
    {
        GameControl.control.Load(selectedSaveRow.saveSlot);
    }

    void DeleteCurrent()
    {
        GameControl.control.Delete(selectedSaveRow.saveSlot);
        selectedSaveRow.SetEmpty();
    }

    public void SetButtons(bool value)
    {
        SetButtons(value, value, value);
    }

    public void SetButtons(bool save, bool load, bool delete)
    {
        saveCurrentButton.interactable = save;
        loadCurrentButton.interactable = load;
        deleteCurrentButton.interactable = delete;
    }

    public void ShowConfirmation(string verb)
    {
        confirmationWindow.Show();
        confirmationLabel.SetText($"Are you sure you want to {verb} the selected file?");
        confirmationString = verb;
    }

    public void ConfirmationYesAction()
    {
        if (confirmationString == "save")
            SaveCurrent();
        else if (confirmationString == "load")
        {
            LoadCurrent();
            InspectorHide();
        }
        else if (confirmationString == "delete")
            DeleteCurrent();
    }
}
