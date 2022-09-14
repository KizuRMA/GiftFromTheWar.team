using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[RequireComponent(typeof(TextController))]
public class ScenarioManager : SingletonMonoBehaviour<ScenarioManager>
{
    public string[] loadFileName = null;

    //  文章を読み終わっているかどうか
    public bool endFlg=false;

    //  シナリオを格納する
    private string[] scenarios;

    //  現在の行番号
    public int currentLine = 0;

    private bool isCallPreload = false;

    //
    private TextController textController;
    private CommandController commandController;
    private Layer layer;

    private PlayerTalk scenario;

    public int talkCount;

    public void RequestNextLine()
    {
        var currentText = scenarios[currentLine];

        textController.SetNextLine(CommandProcess(currentText));
        currentLine++;
        isCallPreload = false;
    }

    public string[] UpdateLines(string fileName)
    {
        var scenarioText = Resources.Load<TextAsset>("Scenario/" + fileName);

        if (scenarioText == null)
        {
            Debug.LogError("シナリオファイルが見つかりませんでした");
            Debug.LogError("ScenarioManagerを無効化します");
            enabled = false;
            return null;
        }

        scenarios = scenarioText.text.Split(new string[] { "@br" }, System.StringSplitOptions.None);
        return scenarios;  
    }

    private string CommandProcess(string line)
    {
        var lineReader = new StringReader(line);
        var lineBuilder = new StringBuilder();
        var text = string.Empty;
        while ((text = lineReader.ReadLine()) != null)
        {
            var commentCharacterCount = text.IndexOf("//");
            if (commentCharacterCount != -1)
            {
                text = text.Substring(0, commentCharacterCount);
            }

            if (!string.IsNullOrEmpty(text))
            {
                if (text[0] == '@' && commandController.LoadCommand(text))
                    continue;
                lineBuilder.AppendLine(text);
            }
        }

        return lineBuilder.ToString();
    }

    #region UNITY_CALLBACK

    // Start is called before the first frame update
    void Start()
    {
       // DontDestroyOnLoad(this);

        textController = GetComponent<TextController>();
        commandController = GetComponent<CommandController>();
        scenario = GetComponent<PlayerTalk>();
        layer = GetComponent<Layer>();

        talkCount = ScenarioData.Instance.saveData.talkCount;

        UpdateLines(loadFileName[0]);
        RequestNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        TextUpdate();
    }
    //  文章を読み終わっていたらウィンドウを削除する
    private void ResetText()
    {
        //  左クリック
        if (Input.GetMouseButtonDown(0))
        {
            endFlg = true;
            //  行番号をリセット
            currentLine = 0;
        }

        if(endFlg)
        {
            scenario.EndTalk();
            talkCount++;
            ScenarioData.Instance.saveData.talkCount = talkCount;
            ScenarioData.Instance.WriteFile();
            endFlg = false;
        }
    }

    public void TextUpdate()
    {
        if (!scenario.iconFlg) return;

        // 文字の表示が完了してるならクリック時に次の行を表示する
        if (textController.IsCompleteDisplayText)
        {
            if (currentLine < scenarios.Length)
            {
                if (!isCallPreload)
                {
                    commandController.PreloadCommand(scenarios[currentLine]);
                    isCallPreload = true;
                }

                if (Input.GetMouseButtonDown(0)&&layer.showFlg==false)
                {
                    RequestNextLine();
                }
            }
            else
            {
                //  文章を読み終わった
                ResetText();
            }


        }
        else
        {
            // 完了してないなら文字をすべて表示する
            if (Input.GetMouseButtonDown(0))
            {
                textController.ForceCompleteDisplayText();
            }
        }
    }

    #endregion
}
