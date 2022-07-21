using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

[RequireComponent(typeof(TextController))]
public class ScenarioManager : SingletonMonoBehaviour<ScenarioManager>
{
    public string loadFileName;

    //  �O�̃p�l���ɖ߂邩�ǂ���
    public bool backPanelFlg = false;

    //  ���͂�ǂݏI����Ă��邩�ǂ���
    public bool endFlg=false;

    //  �V�i���I���i�[����
    private string[] scenarios;
    //  ���݂̍s�ԍ�
    private int currentLine = 0;
   
    private bool isCallPreload = false;

    //
    private TextController textController;
    private CommandController commandController;

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
            Debug.LogError("�V�i���I�t�@�C����������܂���ł���");
            Debug.LogError("ScenarioManager�𖳌������܂�");
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

        UpdateLines(loadFileName);
        RequestNextLine();
    }

    // Update is called once per frame
    void Update()
    {
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
                    RequestNextLine();
                }
            }
            else
            {
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

        if(backPanelFlg)
        {
            currentLine = 0;
            backPanelFlg = false;
        }

    }
    //  ���͂��I�����Ă�����E�B���h�E���폜����
    private void ResetText()
    {
        //  ���N���b�N
        if (Input.GetMouseButtonDown(0))
        {
            endFlg = true;
            //  �s�ԍ������Z�b�g
            currentLine = 0;
            CursorManager.Instance.cursorLock = true;
        }
    }

    #endregion
}
