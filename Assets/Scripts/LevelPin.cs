using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelPin : NavigationPin
{
	[Header("Preview Data & Flags")]
	public string levelDisplayName;
	public Sprite levelPreviewSprite;
	public Sprite[] spriteState;
	public int parTime;
	public bool complete;
	public bool timeChallenge;
	public bool goldChallenge;
	public Animator animator;
	public bool allcleared => complete && timeChallenge && goldChallenge;

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

    void Start()
    {
		SetState();
    }

	public void SetState()
    {
		if(gameObject.activeInHierarchy)
        {
			if (allcleared)
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = spriteState[3];
				animator.Play("RainbowGlow");
				return;
			}


			if (goldChallenge)
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = spriteState[2];
				animator.Play("GoldenGlow");
				return;
			}

			if (complete)
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = spriteState[1];
				animator.Play("BeatenGlow");
				return;
			}

			else
			{
				animator.Play("UnbeatenGlow");
				return;
			}

		}
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
