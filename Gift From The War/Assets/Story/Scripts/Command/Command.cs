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
    //  テキストパネル
    private GameObject textPanel;
    //　テキストパネルのCanvasGroup
    private CanvasGroup textPanelCanvasGroup;
    //　最後に選択していたゲームオブジェクトをスタック
    private Stack<GameObject> selectedGameObjectStack = new Stack<GameObject>();

    private void Awake()
    {
        //　現在のコマンドを初期化
        currentCommand = CommandMode.CommandPanel;

        //　パネル系
        textPanel = transform.Find("TextPanel").gameObject;

        //　CanvasGroup
        textPanelCanvasGroup = textPanel.GetComponent<CanvasGroup>();
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
               // scenario.ExitCommand();
                gameObject.SetActive(false);
                ScenarioManager.Instance.currentLine = 0;
                //　ステータスキャラクター選択またはステータス表示時
            }
        }

        // ネジ君に話しかけた回数によってテキストの内容を変更する
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
        //　現在のコマンドの初期化
        currentCommand = CommandMode.CommandPanel;

        textPanel.SetActive(true);

        selectedGameObjectStack.Clear();
        textPanelCanvasGroup.interactable = true;
    }
}
