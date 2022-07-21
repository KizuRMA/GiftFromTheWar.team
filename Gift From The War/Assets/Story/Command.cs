using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Command : MonoBehaviour
{
    public enum CommandMode
    {
        CommandPanel,
        TextPanel,
        TextPanel1,
        TextPanel2,
    }


    private CommandMode currentCommand;
    //�@�v���C���[�R�}���h�X�N���v�g
    private PlayerCommand playerCommand;

    //�@�ŏ��ɑI������Button��Transform
    private GameObject firstSelectButton;
    private GameObject secondSelectButton;
    private GameObject thirdSelectButton;


    //�@�R�}���h�p�l��
    private GameObject commandPanel;
    //  �e�L�X�g�p�l��
    private GameObject textPanel;
    //  �e�L�X�g�p�l��
    private GameObject textPanel1;
    //  �e�L�X�g�p�l��
    private GameObject textPanel2;


    //�@�R�}���h�p�l����CanvasGroup
    private CanvasGroup commandPanelCanvasGroup;

    //�@�e�L�X�g�p�l����CanvasGroup
    private CanvasGroup textPanelCanvasGroup;

    private CanvasGroup textPanelCanvasGroup1;

    private CanvasGroup textPanelCanvasGroup2;


    //�@�Ō�ɑI�����Ă����Q�[���I�u�W�F�N�g���X�^�b�N
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    private void Awake()
    {
        //�@�R�}���h��ʂ��J�����������Ă���playerCommand���擾
        playerCommand = GameObject.FindWithTag("Player").GetComponent<PlayerCommand>();

        //�@���݂̃R�}���h��������
        currentCommand = CommandMode.CommandPanel;

        //�@�K�w��H���Ă��擾
        firstSelectButton = transform.Find("CommandPanel/TextButton").gameObject;
        secondSelectButton = transform.Find("CommandPanel/TextButton1").gameObject;
        thirdSelectButton = transform.Find("CommandPanel/TextButton2").gameObject;


        //�@�p�l���n
        commandPanel = transform.Find("CommandPanel").gameObject;
        textPanel = transform.Find("TextPanel").gameObject;
        textPanel1 = transform.Find("TextPanel1").gameObject;
        textPanel2= transform.Find("TextPanel2").gameObject;


        //�@CanvasGroup
        commandPanelCanvasGroup = commandPanel.GetComponent<CanvasGroup>();
        textPanelCanvasGroup = textPanel.GetComponent<CanvasGroup>();
        textPanelCanvasGroup1 = textPanel1.GetComponent<CanvasGroup>();
        textPanelCanvasGroup2 = textPanel2.GetComponent<CanvasGroup>();
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
                playerCommand.ExitCommand();
                gameObject.SetActive(false);
                //�@�X�e�[�^�X�L�����N�^�[�I���܂��̓X�e�[�^�X�\����
            }
            else if (currentCommand == CommandMode.TextPanel)
            {
                textPanelCanvasGroup.interactable = false;
                textPanel.SetActive(false);

                //�@�O�̃p�l���őI�����Ă����Q�[���I�u�W�F�N�g��I��
                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                commandPanelCanvasGroup.interactable = true;
                commandPanel.SetActive(true);
                ScenarioManager.Instance.backPanelFlg = true;

                currentCommand = CommandMode.CommandPanel;
            }
            else if(currentCommand==CommandMode.TextPanel1)
            {
                textPanelCanvasGroup1.interactable = false;
                textPanel1.SetActive(false);

                //�@�O�̃p�l���őI�����Ă����Q�[���I�u�W�F�N�g��I��
                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                commandPanelCanvasGroup.interactable = true;
                commandPanel.SetActive(true);
                ScenarioManager.Instance.backPanelFlg = true;

                currentCommand = CommandMode.CommandPanel;
            }
            else if (currentCommand == CommandMode.TextPanel2)
            {
                textPanelCanvasGroup2.interactable = false;
                textPanel2.SetActive(false);

                //�@�O�̃p�l���őI�����Ă����Q�[���I�u�W�F�N�g��I��
                EventSystem.current.SetSelectedGameObject(selectedGameObjectStack.Pop());
                commandPanelCanvasGroup.interactable = true;
                commandPanel.SetActive(true);
                ScenarioManager.Instance.backPanelFlg = true;

                currentCommand = CommandMode.CommandPanel;
            }

        }
    }


    private void OnEnable()
    {
        //�@���݂̃R�}���h�̏�����
        currentCommand = CommandMode.CommandPanel;
        commandPanel.SetActive(true);

        //�@�R�}���h���j���[�\�����ɑ��̃p�l���͔�\���ɂ���
        textPanel.SetActive(false);
        textPanel1.SetActive(false);
        textPanel2.SetActive(false);

        selectedGameObjectStack.Clear();

        commandPanelCanvasGroup.interactable = true;
        textPanelCanvasGroup.interactable = false;
        textPanelCanvasGroup1.interactable = false;
        textPanelCanvasGroup2.interactable = false;
    }

    //�@�I�������R�}���h�ŏ�������
    public void SelectCommand(string command)
    {
        if (command == "Button")
        {
            currentCommand = CommandMode.TextPanel;
            //�@UI�̃I���E�I�t��I���A�C�R���̐ݒ�
            commandPanel.SetActive(false);

            commandPanelCanvasGroup.interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            //�@�K�w����ԍŌ�ɕ��בւ�
            textPanel.transform.SetAsLastSibling();
            textPanel.SetActive(true);
            textPanelCanvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(textPanel.transform.GetChild(0).gameObject);
        }
        else if(command=="Button1")
        {
            currentCommand = CommandMode.TextPanel1;
            //�@UI�̃I���E�I�t��I���A�C�R���̐ݒ�
            commandPanel.SetActive(false);

            commandPanelCanvasGroup.interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            //�@�K�w����ԍŌ�ɕ��בւ�
            textPanel1.transform.SetAsLastSibling();
            textPanel1.SetActive(true);
            textPanelCanvasGroup1.interactable = true;
            EventSystem.current.SetSelectedGameObject(textPanel1.transform.GetChild(0).gameObject);
        }
        else if (command == "Button2")
        {
            currentCommand = CommandMode.TextPanel2;
            //�@UI�̃I���E�I�t��I���A�C�R���̐ݒ�
            commandPanel.SetActive(false);

            commandPanelCanvasGroup.interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            //�@�K�w����ԍŌ�ɕ��בւ�
            textPanel2.transform.SetAsLastSibling();
            textPanel2.SetActive(true);
            textPanelCanvasGroup2.interactable = true;
            EventSystem.current.SetSelectedGameObject(textPanel2.transform.GetChild(0).gameObject);
        }

    }
}
