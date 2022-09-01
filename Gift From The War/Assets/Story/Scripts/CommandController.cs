using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : SingletonMonoBehaviour<CommandController>
{
	// ��������͂��Ȃ���Ăяo���R�}���h
	private readonly List<ICommand> m_commandList = new List<ICommand>()
	{
		//new CommandUpdateImage(),	// name=�I�u�W�F�N�g�� image=�C���[�W�� 
		//new CommandJumpNextScenario(),	// fileName=�V�i���I��
	};

	// �����̕\�������������^�C�~���O�ŌĂ΂�鏈��
	private List<IPreCommand> m_preCommandList = new List<IPreCommand>();

	public void PreloadCommand(string line)
	{
		var dic = CommandAnalytics(line);
		foreach (var command in m_preCommandList)
			if (command.Tag == dic["tag"])
				command.PreCommand(dic);
	}

	public bool LoadCommand(string line)
	{
		var dic = CommandAnalytics(line);
		foreach (var command in m_commandList)
		{
			if (command.Tag == dic["tag"])
			{
				command.Command(dic);
				return true;
			}
		}
		return false;
	}

	// �R�}���h�����
	private Dictionary<string, string> CommandAnalytics(string line)
	{
		Dictionary<string, string> command = new Dictionary<string, string>();
		// �R�}���h�����擾
		var tag = Regex.Match(line, "@(\\S+)\\s");
		command.Add("tag", tag.Groups[1].ToString());

		// �R�}���h�̃p�����[�^���擾
		Regex regex = new Regex("(\\S+)=(\\S+)");
		var matches = regex.Matches(line);
		foreach (Match match in matches)
		{
			command.Add(match.Groups[1].ToString(), match.Groups[2].ToString());
		}

		return command;
	}

	#region UNITY_CALLBACK

	new void Awake()
	{
		base.Awake();

		// PreCommand���擾
		foreach (var command in m_commandList)
		{
			if (command is IPreCommand)
			{
				m_preCommandList.Add((IPreCommand)command);
			}
		}
	}

	#endregion
}
