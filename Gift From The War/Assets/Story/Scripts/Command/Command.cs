using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Command : MonoBehaviour
{
    public enum CommandMode
    {
        CommandPanel,
    }

    private CommandMode currentCommand;
    //  �e�L�X�g�p�l��
    private GameObject textPanel;
    //�@�e�L�X�g�p�l����CanvasGroup
    private CanvasGroup textPanelCanvasGroup;
    //�@�Ō�ɑI�����Ă����Q�[���I�u�W�F�N�g���X�^�b�N
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    private void Awake()
    {
        //�@���݂̃R�}���h��������
        currentCommand = CommandMode.CommandPanel;

        //�@�p�l���n
        textPanel = transform.Find("TextPanel").gameObject;

        //�@CanvasGroup
        textPanelCanvasGroup = textPanel.GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //�@�L�����Z���{�^�������������̏���
        if (Input.GetButtonDown("Cancel"))
        {
            //�@�R�}���h�I����ʎ�
            if (currentCommand == CommandMode.CommandPanel)
            {
               // scenario.ExitCommand();
                gameObject.SetActive(false);
                ScenarioManager.Instance.currentLine = 0;
                //�@�X�e�[�^�X�L�����N�^�[�I���܂��̓X�e�[�^�X�\����
            }
        }

        // �l�W�N�ɘb���������񐔂ɂ���ăe�L�X�g�̓��e��ύX����
        if(ScenarioManager.Instance!=null)
            switch (ScenarioManager.Instance.talkCount)
            {
                case 1:
                    ScenarioManager.Instance.UpdateLines("Scenario2");
                    break;
                case 2:
                    ScenarioManager.Instance.UpdateLines("Scenario3");
                    break;
                case 3:
                    ScenarioManager.Instance.UpdateLines("Scenario4");
                    break;
                case 4:
                    ScenarioManager.Instance.UpdateLines("Scenario5");
                    break;
                case 5:
                    ScenarioManager.Instance.UpdateLines("Scenario6");
                    break;
                case 6:
                    ScenarioManager.Instance.UpdateLines("Scenario7");
                    break;
                case 7:
                    ScenarioManager.Instance.UpdateLines("Scenario8");
                    break;
                case 8:
                    ScenarioManager.Instance.UpdateLines("Scenario9");
                    break;
            }

    }


    private void OnEnable()
    {
        //�@���݂̃R�}���h�̏�����
        currentCommand = CommandMode.CommandPanel;

        textPanel.SetActive(true);

        selectedGameObjectStack.Clear();
        textPanelCanvasGroup.interactable = true;
    }
}
