using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OverworldLevelPin : MonoBehaviour
{
	public MapManager mapManager;
	public OverworldLevelPinView view;
	public OverworldFollowCamera overworldCamera;
	[Header("Preview Data & Flags")]
	public int levelNumber;
	public int parTime;
	public int worldIndex;
	private Vector3 playerOffset = new Vector3(0, -2f,  0f);
	private Vector3 cameraOffset = new Vector3(0, -2f, -10f);
	public string levelDisplayName;
	public Sprite levelPreviewSprite;

	private void SelectLevel()
    {
        mapManager.LoadLevelFromSceneIndex(levelNumber);
    }

	public void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out OverworldPlayer player))
		{
			player.currentPin = this;
			player.OnSelect += SelectLevel;
			overworldCamera.SetCameraMode(OverworldFollowCamera.CameraMode.LevelPreview);
			GameControl.control.savedOverworldPlayerPosition = transform.position + playerOffset;
			GameControl.control.savedCameraPosition = transform.position + cameraOffset;
			GameControl.control.savedPinID = levelNumber;
			overworldCamera.SetTarget(transform);
			LevelPreviewWindow previewWindow = mapManager.overworldGUI.levelPreview as LevelPreviewWindow;
			previewWindow.UpdatePreviewData(this);
			previewWindow.Show();
		}
	}

    public void OnTriggerExit(Collider other)
	{
		if(other.TryGetComponent(out OverworldPlayer player))
		{
			player.currentPin = null;
			player.OnSelect -= SelectLevel;
			overworldCamera.SetCameraMode(OverworldFollowCamera.CameraMode.FreeRoam);
			overworldCamera.SetTarget(player.transform);
			mapManager.overworldGUI.levelPreview.Hide();
		}
	}

	public void SetWorldData(int levelNumber, int parTime, string levelDisplayName, Sprite levelPreviewSprite)
	{
		this.levelNumber = levelNumber;
		this.parTime = parTime;
		this.levelDisplayName = levelDisplayName;
		this.levelPreviewSprite = levelPreviewSprite;
	}
}