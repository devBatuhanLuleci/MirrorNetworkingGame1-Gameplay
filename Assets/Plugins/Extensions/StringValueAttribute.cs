﻿using System;

public class StringValueAttribute : Attribute
{
    public string StringValue { get; protected set; }

	public StringValueAttribute(string value)
	{
		StringValue = value;
	}
}

