using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadPanel : GUIWindow
{
    public SaveFileRow[] saveFileRows;
    public SaveFileRow selected;
    public Button saveCurrentButton;
    public Button loadCurrentButton;
    public Button deleteCurrentButton;

    public GUIWindow confirmationWindow;
    public TextMeshProUGUI confirmationLabel;
    public ScriptableGameState gameState;

    string confirmationString;
    bool initialized;

    public void Start()
    {
        StartCoroutine(DelayedStart());
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

    public void UpdateSelection()
    {
        selected = null;
        foreach (SaveFileRow row in saveFileRows)
        {
            if(row.toggle.isOn)
                selected = row;
        }
        
        SetButtons(selected != null,
                selected != null && !selected.isEmpty,
                selected != null && !selected.isEmpty);
    }

    void SaveCurrent()
    {
        GameControl.control.Save(selected.saveSlot);
        selected.SetFromControl(GameControl.control);
    }

    void LoadCurrent()
    {
        GameControl.control.Load(selected.saveSlot);
    }

    void DeleteCurrent()
    {
        GameControl.control.Delete(selected.saveSlot);
        selected.SetEmpty();
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
