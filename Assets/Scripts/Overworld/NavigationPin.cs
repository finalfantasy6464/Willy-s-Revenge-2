using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavigationPin : MonoBehaviour
{
    [Header("Navigation")]
	public BezierNode previousPath;
	public PathDirection previousDirection;
	public BezierNode nextPath;
	public PathDirection nextDirection;

    public bool IsPathAvailable(PathDirection direction, out BezierNode path)
    {
		if(previousPath != null && previousDirection == direction)
		{
			path = previousPath;
			return true;
		}
		else if(nextPath != null && nextDirection == direction)
		{
			path = nextPath;
			return true;
		}

		path = null;
		return false;        
    }

    BezierNode previousCache;
	BezierNode nextCache;
	[HideInInspector] public UnityEvent onCharacterEnter;
	[HideInInspector] public UnityEvent onCharacterExit;

	protected virtual void Awake()
	{
		onCharacterEnter = new UnityEvent();
		onCharacterExit = new UnityEvent();
	}
}
