using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Scenario : MonoBehaviour
{
    //  コマンドオブジェクト参照用
    [SerializeField] GameObject image;

    // 話しかける時のアイコン用
    [SerializeField] GameObject talkIcon;

    // 話しかけた回数
    public int talkCount = 0;

    public bool scenarioFlg = false;

    StoryManager storyManager;

    private void Awake()
    {
     
    }

    // Start is called before the first frame update
    void Start()
    {
        storyManager = GameObject.Find("NeziKunGroup").GetComponent<StoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
       // Ray();

        if (storyManager.hitFlg == true)
        {
            //  コマンド画面を表示
            OpenCommand();

            storyManager.hitFlg = false;
        }

        //  コマンド画面終了
        EndCommand();
    }


    //  文章を読み終わったらコマンド画面を終了させる
    void EndCommand()
    {
        if (!ScenarioManager.Instance.endFlg) return;

        image.SetActive(false);
        storyManager.neziKun.SetActive(false);

        ScenarioManager.Instance.talkCount++;


        CursorManager.Instance.cursorLock = true;

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
        talkIcon.SetActive(false);
        CursorManager.Instance.cursorLock = false;
        scenarioFlg = true;
    }
}
