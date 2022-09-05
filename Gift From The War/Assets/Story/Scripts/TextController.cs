using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
	//	1�����̕\���ɂ����鎞��
	[SerializeField]
	[Range(0.05f, 0.15f)]
	public float intervalForCharacterDisplay = 0.05f;

	//	���݂̕�����
	private string currentText;
	//	�\���ɂ����鎞��
	private float timeUntilDisplay = 0;
	//	������̕\�����J�n��������
	private float timeElapsed = 1;
	//	�\�����̕�����
	private int lastUpdateCharacter = -1;

	//	uiText�ւ̎Q��
	[SerializeField]
	private Text _uiText;

	Scenario scenario;

	//	�����̕\�����������Ă��邩�ǂ���
	public bool IsCompleteDisplayText
	{
		get { return Time.time > timeElapsed + timeUntilDisplay; }
	}

	// �����I�ɑS���\������
	public void ForceCompleteDisplayText()
	{
		timeUntilDisplay = 0;
	}

	// ���ɕ\�����镶������Z�b�g����
	public void SetNextLine(string text)
	{
		currentText = text;

		//	�z�莞�Ԃƌ��݂̎������L���b�V��
		timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;

		timeElapsed = Time.time;

		//	�����J�E���g��������
		lastUpdateCharacter = -1;
	}

    #region UNITY_CALLBACK	

    private void Awake()
    {
		//	�V�i���I�N���X
		scenario = GameObject.Find("ScenarioManager").GetComponent<Scenario>();
	}
    
	void Update()
	{
		if (currentText != null)
		{
			//�\�����������O��̕\���������ƈقȂ�Ȃ�e�L�X�g���X�V����
			int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
			if (displayCharacterCount != lastUpdateCharacter)
			{
				_uiText.text = currentText.Substring(0, displayCharacterCount);

				lastUpdateCharacter = displayCharacterCount;
			}
		}

		// �V�i���I��ǂݎn�߂鎞�̃e�L�X�g�ɕύX����
		if(scenario.scenarioFlg==false)
        {
			_uiText.text ="���N���b�N�ŃX�g�[���[��ǂ�";
        }
	}

	#endregion
}
