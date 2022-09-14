using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Scenario : MonoBehaviour
{
    //  コマンドオブジェクト参照用
    [SerializeField] GameObject image;

    public bool scenarioFlg = false;

    private void Awake()
    {
     
    }

    // Start is called before the first frame update
    void Start()
    {
        image.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //  コマンド画面終了
        EndCommand();
    }


    //  文章を読み終わったらコマンド画面を終了させる
    public void EndCommand()
    {
        if (!ScenarioManager.Instance.endFlg) return;

        image.SetActive(false);

        ScenarioManager.Instance.talkCount++;

        CursorManager.Instance.cursorLock = true;
        ScenarioData.Instance.WriteFile();

        scenarioFlg = false;
        ScenarioManager.Instance.endFlg = false;
    }

    //　CommandScriptから呼び出すコマンド画面の終了
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        CursorManager.Instance.cursorLock = true;

        scenarioFlg = false;

    }

    //  コマンド画面表示
    public void OpenCommand()
    {
        image.SetActive(true);
        CursorManager.Instance.cursorLock = false;
        scenarioFlg = true;
    }
}
