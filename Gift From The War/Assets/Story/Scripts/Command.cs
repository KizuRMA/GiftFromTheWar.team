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
        TextPanel,
        TextPanel1,
        TextPanel2,
    }


    private CommandMode currentCommand;
    //�@�V�i���I�X�N���v�g
    private Scenario scenario;

    //�@�R�}���h�p�l��
    private GameObject commandPanel;
    //  �e�L�X�g�p�l��
    private GameObject textPanel;

    //�@�R�}���h�p�l����CanvasGroup
    private CanvasGroup commandPanelCanvasGroup;

    //�@�e�L�X�g�p�l����CanvasGroup
    private CanvasGroup textPanelCanvasGroup;

    //�@�L�����N�^�[�I���̃{�^���̃v���n�u
    [SerializeField]
    private GameObject textButtonPrefab = null;

    //�@�p�[�e�B�[�X�e�[�^�X
    [SerializeField]
    private PartyStatus partyStatus = null;

    [SerializeField]
    private ButtonStatus[] buttonStatus = null;

    //�@�Ō�ɑI�����Ă����Q�[���I�u�W�F�N�g���X�^�b�N
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    public string[] scenarios;
    private void Awake()
    {
        //�@�R�}���h��ʂ��J�����������Ă���Scenario���擾
        scenario = GameObject.FindGameObjectWithTag("Scenario").GetComponent<Scenario>();

        //�@���݂̃R�}���h��������
        currentCommand = CommandMode.CommandPanel;

        //�@�p�l���n
        commandPanel = transform.Find("CommandPanel").gameObject;
        textPanel = transform.Find("TextPanel").gameObject;

        //�@CanvasGroup
        commandPanelCanvasGroup = commandPanel.GetComponent<CanvasGroup>();
        textPanelCanvasGroup = textPanel.GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //�{�^���̃v���n�u����C���X�^���X����
        GameObject[] textButtonIns=new GameObject[3];

        for(int i=0;i<partyStatus.GetButtonStatus().Count;i++)
        {
            textButtonIns[i] = Instantiate<GameObject>(textButtonPrefab, commandPanel.transform);
            textButtonIns[i].GetComponentInChildren<Text>().text = buttonStatus[i].GetButtonName();
            textButtonIns[i].GetComponentInChildren<Text>().name = "Text" + i;
            textButtonIns[i].name = "Button"+i;
        }
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
                scenario.ExitCommand();
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
        }

        // �l�W�N�ɘb���������񐔂ɂ���ăe�L�X�g�̓��e��ύX����
        if (scenario.talkCount == 0)
        {
            scenarios = ScenarioManager.Instance.UpdateLines("Scenario1");
            ScenarioManager.Instance.storyNum = 0;
        }
        else if (scenario.talkCount == 1)
        {
            scenarios = ScenarioManager.Instance.UpdateLines("Scenario2");
            ScenarioManager.Instance.storyNum = 1;
        }
        // �e�L�X�g���X�V����
        ScenarioManager.Instance.TextUpdate(scenarios);
    }


    private void OnEnable()
    {
        //�@���݂̃R�}���h�̏�����
        currentCommand = CommandMode.CommandPanel;
        //commandPanel.SetActive(true);
        textPanel.SetActive(true);


        //�@�R�}���h���j���[�\�����ɑ��̃p�l���͔�\���ɂ���
       //textPanel.SetActive(false);

        selectedGameObjectStack.Clear();

       //commandPanelCanvasGroup.interactable = true;
       //textPanelCanvasGroup.interactable = false;
       textPanelCanvasGroup.interactable = true;

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
        }

        //�@�K�w����ԍŌ�ɕ��בւ�
        textPanel.transform.SetAsLastSibling();
        textPanel.SetActive(true);
        textPanelCanvasGroup.interactable = true;
        EventSystem.current.SetSelectedGameObject(textPanel.transform.GetChild(0).gameObject);
    }
}
