using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[RequireComponent(typeof(TextController))]
public class ScenarioManager : SingletonMonoBehaviour<ScenarioManager>
{
    public int storyNum=0;
    public string[] loadFileName=null;

    //  �O�̃p�l���ɖ߂邩�ǂ���
    public bool backPanelFlg = false;

    //  ���͂�ǂݏI����Ă��邩�ǂ���
    public bool endFlg=false;

    //  �V�i���I���i�[����
    private string[] scenarios;

    private string[] scenarios1;

    private string[] scenarios2;


    //  ���݂̍s�ԍ�
    private int currentLine = 0;

    private bool isCallPreload = false;

    //
    private TextController textController;
    private CommandController commandController;
    private Scenario scenario;

    public void RequestNextLine(string fileName,string[] _scenarios)
    {
        fileName = _scenarios[currentLine];

        textController.SetNextLine(CommandProcess(fileName));
        currentLine++;

        isCallPreload = false;
    }

    public string[] UpdateLines(string fileName)
    {
        var scenarioText = Resources.Load<TextAsset>("Scenario/" + fileName);

        if (scenarioText == null)
        {
            Debug.LogError("�V�i���I�t�@�C����������܂���ł���");
            Debug.LogError("ScenarioManager�𖳌������܂�");
            enabled = false;
            return null;
        }

        if (fileName == "Scenario1")
        {
            scenarios = scenarioText.text.Split(new string[] { "@br" }, System.StringSplitOptions.None);
            return scenarios;
        }
        else if(fileName=="Test1")
        {
            scenarios1 = scenarioText.text.Split(new string[] { "@br" }, System.StringSplitOptions.None);
            return scenarios1;
        }
        else if (fileName == "Test2")
        {
            scenarios2 = scenarioText.text.Split(new string[] { "@br" }, System.StringSplitOptions.None);
            return scenarios2;
        }

        currentLine = 0;

        Resources.UnloadAsset(scenarioText);
        return null;
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
        scenario = GameObject.Find("ScenarioManager/Scenario").GetComponent<Scenario>();

        for (int i = 0; i < loadFileName.Length; i++)
        {
            UpdateLines(loadFileName[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  �O�̃p�l���ɖ߂�
        if (backPanelFlg)
        {
            //  �s�ԍ������Z�b�g
            currentLine = 0;
            backPanelFlg = false;
        }
    }
    //  ���͂�ǂݏI����Ă�����E�B���h�E���폜����
    private void ResetText()
    {
        //  ���N���b�N
        if (Input.GetMouseButtonDown(0))
        {
            endFlg = true;
            //  �s�ԍ������Z�b�g
            currentLine = 0;
        }
    }

    public void TextUpdate(string[] scenarios)
    {
        if (!scenario.scenarioFlg) return;

        // �����̕\�����������Ă�Ȃ�N���b�N���Ɏ��̍s��\������
        if (textController.IsCompleteDisplayText)
        {
            if (currentLine < scenarios.Length)
            {
                if (!isCallPreload)
                {
                    commandController.PreloadCommand(scenarios[currentLine]);
                    isCallPreload = true;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    RequestNextLine(loadFileName[storyNum], scenarios);
                }
            }
            else
            {
                //  ���͂�ǂݏI�����
                ResetText();
            }


        }
        else
        {
            // �������ĂȂ��Ȃ當�������ׂĕ\������
            if (Input.GetMouseButtonDown(0))
            {
                textController.ForceCompleteDisplayText();
            }
        }
    }

    #endregion
}
