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
    //　プレイヤーコマンドスクリプト
    private PlayerCommand playerCommand;

    //　最初に選択するButtonのTransform
    private GameObject firstSelectButton;
    private GameObject secondSelectButton;
    private GameObject thirdSelectButton;


    //　コマンドパネル
    private GameObject commandPanel;
    //  テキストパネル
    private GameObject textPanel;
    //  テキストパネル
    private GameObject textPanel1;
    //  テキストパネル
    private GameObject textPanel2;


    //　コマンドパネルのCanvasGroup
    private CanvasGroup commandPanelCanvasGroup;

    //　テキストパネルのCanvasGroup
    private CanvasGroup textPanelCanvasGroup;

    private CanvasGroup textPanelCanvasGroup1;

    private CanvasGroup textPanelCanvasGroup2;


    //　最後に選択していたゲームオブジェクトをスタック
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    private void Awake()
    {
        //　コマンド画面を開く処理をしているplayerCommandを取得
        playerCommand = GameObject.FindWithTag("Player").GetComponent<PlayerCommand>();

        //　現在のコマンドを初期化
        currentCommand = CommandMode.CommandPanel;

        //　階層を辿ってを取得
        firstSelectButton = transform.Find("CommandPanel/TextButton").gameObject;
        secondSelectButton = transform.Find("CommandPanel/TextButton1").gameObject;
        thirdSelectButton = transform.Find("CommandPanel/TextButton2").gameObject;


        //　パネル系
        commandPanel = transform.Find("CommandPanel").gameObject;
        textPanel = transform.Find("TextPanel").gameObject;
        textPanel1 = transform.Find("TextPanel1").gameObject;
        textPanel2= transform.Find("TextPanel2").gameObject;


        //　CanvasGroup
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
        //　キャンセルボタンを押した時の処理
        if (Input.GetButtonDown("Cancel"))
        {
            //　コマンド選択画面時
            if (currentCommand == CommandMode.CommandPanel)
            {
                playerCommand.ExitCommand();
                gameObject.SetActive(false);
                //　ステータスキャラクター選択またはステータス表示時
            }
            else if (currentCommand == CommandMode.TextPanel)
            {
                textPanelCanvasGroup.interactable = false;
                textPanel.SetActive(false);

                //　前のパネルで選択していたゲームオブジェクトを選択
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

                //　前のパネルで選択していたゲームオブジェクトを選択
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

                //　前のパネルで選択していたゲームオブジェクトを選択
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
        //　現在のコマンドの初期化
        currentCommand = CommandMode.CommandPanel;
        commandPanel.SetActive(true);

        //　コマンドメニュー表示時に他のパネルは非表示にする
        textPanel.SetActive(false);
        textPanel1.SetActive(false);
        textPanel2.SetActive(false);

        selectedGameObjectStack.Clear();

        commandPanelCanvasGroup.interactable = true;
        textPanelCanvasGroup.interactable = false;
        textPanelCanvasGroup1.interactable = false;
        textPanelCanvasGroup2.interactable = false;
    }

    //　選択したコマンドで処理分け
    public void SelectCommand(string command)
    {
        if (command == "Button")
        {
            currentCommand = CommandMode.TextPanel;
            //　UIのオン・オフや選択アイコンの設定
            commandPanel.SetActive(false);

            commandPanelCanvasGroup.interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            //　階層を一番最後に並べ替え
            textPanel.transform.SetAsLastSibling();
            textPanel.SetActive(true);
            textPanelCanvasGroup.interactable = true;
            EventSystem.current.SetSelectedGameObject(textPanel.transform.GetChild(0).gameObject);
        }
        else if(command=="Button1")
        {
            currentCommand = CommandMode.TextPanel1;
            //　UIのオン・オフや選択アイコンの設定
            commandPanel.SetActive(false);

            commandPanelCanvasGroup.interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            //　階層を一番最後に並べ替え
            textPanel1.transform.SetAsLastSibling();
            textPanel1.SetActive(true);
            textPanelCanvasGroup1.interactable = true;
            EventSystem.current.SetSelectedGameObject(textPanel1.transform.GetChild(0).gameObject);
        }
        else if (command == "Button2")
        {
            currentCommand = CommandMode.TextPanel2;
            //　UIのオン・オフや選択アイコンの設定
            commandPanel.SetActive(false);

            commandPanelCanvasGroup.interactable = false;
            selectedGameObjectStack.Push(EventSystem.current.currentSelectedGameObject);

            //　階層を一番最後に並べ替え
            textPanel2.transform.SetAsLastSibling();
            textPanel2.SetActive(true);
            textPanelCanvasGroup2.interactable = true;
            EventSystem.current.SetSelectedGameObject(textPanel2.transform.GetChild(0).gameObject);
        }

    }
}
