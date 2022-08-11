using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpinnerType
{
    WithText
}

public enum SpinnerPosition
{
	LeftTop,
	RightTop,
	Center,
	LeftBottom,
	RightBottom,		
}
public class Spinner : MonoBehaviour
{
	[SerializeField]
	private SpinnerType _type;

	public SpinnerType Type
	{
		get { return _type; }
		set { TypeChanged(value); }
	}

	private void TypeChanged(SpinnerType value)
	{
		Debug.LogWarning("Not Implemented");
	}

	

	void Start()
    {
        
    }

  
}
