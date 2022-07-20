using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[RequireComponent(typeof(TextController))]
public class ScenarioManager : SingletonMonoBehaviour<ScenarioManager>
{
    public string loadFileName;

    //  シナリオを格納する
    private string[] scenarios;
    //  現在の行番号
    private int currentLine = 0;
    //  最大の行番号
    private int maxLine;

    private bool isCallPreload = false;

    //
    private TextController textController;
    private CommandController commandController;
  
    private GameObject neziKun;

    void RequestNextLine()
    {
        var currentText = scenarios[currentLine];

        textController.SetNextLine(CommandProcess(currentText));
        currentLine++;
   
        isCallPreload = false;
    }

    public void UpdateLines(string fileName)
    {
        var scenarioText = Resources.Load<TextAsset>("Scenario/" + fileName);

        if (scenarioText == null)
        {
            Debug.LogError("シナリオファイルが見つかりませんでした");
            Debug.LogError("ScenarioManagerを無効化します");
            enabled = false;
            return;
        }
        scenarios = scenarioText.text.Split(new string[] { "@br" }, System.StringSplitOptions.None);
        currentLine = 0;

        Resources.UnloadAsset(scenarioText);
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
        DontDestroyOnLoad(this);

        textController = GetComponent<TextController>();
        commandController = GetComponent<CommandController>();

        neziKun = GameObject.Find("NeziKun");

        maxLine = 3;

        UpdateLines(loadFileName);
        RequestNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        // 文字の表示が完了してるならクリック時に次の行を表示する
        if (textController.IsCompleteDisplayText)
        {
            if (currentLine >= maxLine)
            {
                currentLine = 0;
                neziKun.SetActive(false);
            }

            if (currentLine < scenarios.Length)
            {
                if (!isCallPreload)
                {
                    commandController.PreloadCommand(scenarios[currentLine]);
                    isCallPreload = true;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    RequestNextLine();
                }
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

    void CheckEndText()
    {
        Debug.Log("End");
    }

    #endregion
}
