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
    //　シナリオスクリプト
    private Scenario scenario;

    //　コマンドパネル
    private GameObject commandPanel;
    //  テキストパネル
    private GameObject textPanel;

    //　コマンドパネルのCanvasGroup
    private CanvasGroup commandPanelCanvasGroup;

    //　テキストパネルのCanvasGroup
    private CanvasGroup textPanelCanvasGroup;

    //　キャラクター選択のボタンのプレハブ
    [SerializeField]
    private GameObject textButtonPrefab = null;

    //　パーティーステータス
    [SerializeField]
    private PartyStatus partyStatus = null;

    [SerializeField]
    private ButtonStatus[] buttonStatus = null;

    //　最後に選択していたゲームオブジェクトをスタック
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    public string[] scenarios;
    private void Awake()
    {
        //　コマンド画面を開く処理をしているScenarioを取得
        scenario = GameObject.FindGameObjectWithTag("Scenario").GetComponent<Scenario>();

        //　現在のコマンドを初期化
        currentCommand = CommandMode.CommandPanel;

        //　パネル系
        commandPanel = transform.Find("CommandPanel").gameObject;
        textPanel = transform.Find("TextPanel").gameObject;

        //　CanvasGroup
        commandPanelCanvasGroup = commandPanel.GetComponent<CanvasGroup>();
        textPanelCanvasGroup = textPanel.GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //ボタンのプレハブからインスタンス生成
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
        //　キャンセルボタンを押した時の処理
        if (Input.GetButtonDown("Cancel"))
        {
            //　コマンド選択画面時
            if (currentCommand == CommandMode.CommandPanel)
            {
                scenario.ExitCommand();
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
        }

        // ネジ君に話しかけた回数によってテキストの内容を変更する
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
        // テキストを更新する
        ScenarioManager.Instance.TextUpdate(scenarios);
    }


    private void OnEnable()
    {
        //　現在のコマンドの初期化
        currentCommand = CommandMode.CommandPanel;
        //commandPanel.SetActive(true);
        textPanel.SetActive(true);


        //　コマンドメニュー表示時に他のパネルは非表示にする
       //textPanel.SetActive(false);

        selectedGameObjectStack.Clear();

       //commandPanelCanvasGroup.interactable = true;
       //textPanelCanvasGroup.interactable = false;
       textPanelCanvasGroup.interactable = true;

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
        }

        //　階層を一番最後に並べ替え
        textPanel.transform.SetAsLastSibling();
        textPanel.SetActive(true);
        textPanelCanvasGroup.interactable = true;
        EventSystem.current.SetSelectedGameObject(textPanel.transform.GetChild(0).gameObject);
    }
}
