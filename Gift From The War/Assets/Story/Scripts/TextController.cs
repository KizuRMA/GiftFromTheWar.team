using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
	//	1文字の表示にかかる時間
	[SerializeField]
	[Range(0.05f, 0.15f)]
	public float intervalForCharacterDisplay = 0.05f;

	//	現在の文字列
	private string currentText;
	//	表示にかかる時間
	private float timeUntilDisplay = 0;
	//	文字列の表示を開始した時間
	private float timeElapsed = 1;
	//	表示中の文字数
	private int lastUpdateCharacter = -1;

	//	uiTextへの参照
	[SerializeField]
	private Text _uiText;

	Scenario scenario;

	//	文字の表示が完了しているかどうか
	public bool IsCompleteDisplayText
	{
		get { return Time.time > timeElapsed + timeUntilDisplay; }
	}

	// 強制的に全文表示する
	public void ForceCompleteDisplayText()
	{
		timeUntilDisplay = 0;
	}

	// 次に表示する文字列をセットする
	public void SetNextLine(string text)
	{
		currentText = text;

		//	想定時間と現在の時刻をキャッシュ
		timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;

		timeElapsed = Time.time;

		//	文字カウントを初期化
		lastUpdateCharacter = -1;
	}

    #region UNITY_CALLBACK	

    private void Awake()
    {
		//	シナリオクラス
		scenario = GameObject.Find("ScenarioManager").GetComponent<Scenario>();
	}
    
	void Update()
	{
		if (currentText != null)
		{
			//表示文字数が前回の表示文字数と異なるならテキストを更新する
			int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
			if (displayCharacterCount != lastUpdateCharacter)
			{
				_uiText.text = currentText.Substring(0, displayCharacterCount);

				lastUpdateCharacter = displayCharacterCount;
			}
		}

		// シナリオを読み始める時のテキストに変更する
		if(scenario.scenarioFlg==false)
        {
			_uiText.text ="左クリックでストーリーを読む";
        }
	}

	#endregion
}
