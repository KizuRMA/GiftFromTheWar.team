using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    Command command;
    private GameObject pointerGameObject;

    private string[] scenarios;


    private void Start()
    {
        command = GameObject.Find("ScenarioManager/Command").GetComponent<Command>();
    }

    //マウスオーバーされているオブジェクトの情報を取得する
    public void OnPointerEnter(BaseEventData data)
    {
        pointerGameObject = (data as PointerEventData).pointerEnter;
    }

    //選択肢によって表示するテキストを変更する
    public void StoryCommand()
    {
        if (pointerGameObject.name == "Text0")
        {
            command.scenarios = ScenarioManager.Instance.UpdateLines("Scenario1");
            ScenarioManager.Instance.storyNum = 0;
        }
        else if(pointerGameObject.name=="Text1")
        {
            command.scenarios = ScenarioManager.Instance.UpdateLines("Test1");
            ScenarioManager.Instance.storyNum = 1;
        }
        else if(pointerGameObject.name=="Text2")
        {
            command.scenarios = ScenarioManager.Instance.UpdateLines("Test2");
            ScenarioManager.Instance.storyNum = 2;
        }
    }

    public void SelectCommand()
    {
        command.SelectCommand("Button");
    }
}
