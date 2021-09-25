using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelPin : NavigationPin
{
	[Header("Preview Data & Flags")]
	public string levelDisplayName;
	public Sprite levelPreviewSprite;
	public int parTime;
	public bool complete;
	public bool timeChallenge;
	public bool goldChallenge;

	public PathArrowsDisplay pathArrows;

	protected override void Awake()
	{
		base.Awake();
		onCharacterEnter.AddListener(() => SetPathArrows(true));
		onCharacterEnter.AddListener(() => pathArrows.SetIsPlayerOnPin(true));
		
		onCharacterExit.AddListener(() => SetPathArrows(false));
		onCharacterExit.AddListener(() => pathArrows.SetIsPlayerOnPin(false));
	}

    void SetPathArrows(bool value)
    {
        if(previousDirection != PathDirection.None)
			pathArrows.SetArrow(previousDirection, value);

		if(nextDirection != PathDirection.None)
			pathArrows.SetArrow(nextDirection, value);
    }

	void OnDisable()
	{
		onCharacterEnter.RemoveListener(() => SetPathArrows(true));
		onCharacterEnter.AddListener(() => pathArrows.SetIsPlayerOnPin(true));
		
		onCharacterExit.RemoveListener(() => SetPathArrows(false));
		onCharacterExit.AddListener(() => pathArrows.SetIsPlayerOnPin(false));
	}
}

public enum PathDirection
{
	None, Left, Up, Right, Down
}
