using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SaveLoadPanel : GUIWindow
{
    public WindowState windowState;
    public SaveFileRow[] saveFileRows;
    public SaveFileRow selectedSaveRow;
    public Selectable UISelected;
    public Button backButton;
    public Button saveCurrentButton;
    public Button loadCurrentButton;
    public Button deleteCurrentButton;
    [Space]
    public GUIWindow confirmationWindow;
    public TextMeshProUGUI confirmationLabel;
    public ScriptableGameState gameState;

    string confirmationString;
    bool initialized;

    public void Start()
    {
        SetWindowState(WindowState.Selection);
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

    public void UpdateSelection(bool value)
    {
        foreach (SaveFileRow row in saveFileRows)
        {
            if(row.toggle.isOn)
                selectedSaveRow = row;
        }
        
        SetButtons(selectedSaveRow != null,
                selectedSaveRow != null && !selectedSaveRow.isEmpty,
                selectedSaveRow != null && !selectedSaveRow.isEmpty);

        if(!value) return;

        SetUISelected(selectedSaveRow.isEmpty ? saveCurrentButton : loadCurrentButton);
        SetWindowState(WindowState.Operation);
    }

    void SetUISelected(Selectable selectable)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectable.gameObject);
        UISelected = selectable;
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

    public void ProcessCloseAll()
    {
        SetWindowState(WindowState.Selection);
        SetUISelected(saveFileRows[0].toggle);
    }

    public void ShowConfirmation(string verb)
    {
        confirmationWindow.Show();
        confirmationLabel.SetText($"Are you sure you want to {verb} the selected file?");
        confirmationString = verb;

        SetUISelected(confirmationWindow.firstSelected);
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

    public void ProcessCancelInput()
    {
        if(windowState == WindowState.Operation)
        {
            SetWindowState(WindowState.Selection);
            SetUISelected(selectedSaveRow.toggle);
        } else
            Close();
    }

    public void SetWindowState(WindowState state)
    {
        windowState = state;
        Navigation backNavigation = backButton.navigation;
        
        if(state == WindowState.Selection)
        {
            backNavigation.selectOnLeft = null;
            backNavigation.selectOnUp = saveFileRows[2].toggle;
            backNavigation.selectOnRight = null;
            backNavigation.selectOnDown = saveFileRows[0].toggle;

            foreach (SaveFileRow row in saveFileRows)
                row.toggle.interactable = true;
            
            if(selectedSaveRow != null)
                selectedSaveRow.toggle.isOn = false;
            
            SetButtons(false, false, false);
        }
        else if(state == WindowState.Operation)
        {
            foreach (SaveFileRow row in saveFileRows)
                row.toggle.interactable = false;

            backNavigation.selectOnLeft = deleteCurrentButton;
            backNavigation.selectOnUp = null;
            backNavigation.selectOnRight = loadCurrentButton;
            backNavigation.selectOnDown = null;
            
            Navigation saveNavigation = saveCurrentButton.navigation;
            if(loadCurrentButton.interactable)
            {
                saveNavigation.selectOnLeft = loadCurrentButton;
                saveNavigation.selectOnRight = deleteCurrentButton;
            }
            else
            {
                saveNavigation.selectOnLeft = backButton;
                saveNavigation.selectOnRight = backButton;
                backNavigation.selectOnLeft = saveCurrentButton;
                backNavigation.selectOnRight = saveCurrentButton;
            }
            saveCurrentButton.navigation = saveNavigation;
        }

        backButton.navigation = backNavigation;
    }

    public enum WindowState
    {
        Selection,
        Operation
    }
}
