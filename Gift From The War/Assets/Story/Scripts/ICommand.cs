using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �R�}���h
public interface ICommand
{
	string Tag { get; }
	void Command(Dictionary<string, string> command);
}

// ���O�ɌĂ΂��R�}���h
public interface IPreCommand
{
	string Tag { get; }
	void PreCommand(Dictionary<string, string> command);
}

