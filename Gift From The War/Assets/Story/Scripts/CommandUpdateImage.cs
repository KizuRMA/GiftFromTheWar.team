using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateImage : ICommand
{
	public string Tag
	{
		get { return "img"; }
	}

	public void Command(Dictionary<string, string> command)
	{ 
		var obj = GameObject.Find("ScenarioManager").GetComponent<Layer>();
		obj.ShowPanel();
	}
}
