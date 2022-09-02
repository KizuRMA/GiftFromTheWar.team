using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    Scenario scenario;
    Command command;
    private GameObject pointerGameObject;

    private string[] scenarios;


    private void Start()
    {
        command = GameObject.Find("ScenarioManager/Command").GetComponent<Command>();
        scenario = GameObject.Find("ScenarioManager/Scenario").GetComponent<Scenario>();
    }

    //�}�E�X�I�[�o�[����Ă���I�u�W�F�N�g�̏����擾����
    public void OnPointerEnter(BaseEventData data)
    {
        pointerGameObject = (data as PointerEventData).pointerEnter;
    }

    //�I�����ɂ���ĕ\������e�L�X�g��ύX����
    public void StoryCommand()
    {
        if (scenario.talkCount == 1)
        {
            if (pointerGameObject.name == "Text0")
            {
                command.scenarios = ScenarioManager.Instance.UpdateLines("Scenario1");
                ScenarioManager.Instance.storyNum = 0;
            }
            else if (pointerGameObject.name == "Text1")
            {
                command.scenarios = ScenarioManager.Instance.UpdateLines("Scenario1");
                ScenarioManager.Instance.storyNum = 1;
            }
            else if (pointerGameObject.name == "Text2")
            {
                command.scenarios = ScenarioManager.Instance.UpdateLines("Scenario1");
                ScenarioManager.Instance.storyNum = 2;
            }
        }else if(scenario.talkCount==2)
        {
            if (pointerGameObject.name == "Text0")
            {
                command.scenarios = ScenarioManager.Instance.UpdateLines("Scenario2");
                ScenarioManager.Instance.storyNum = 0;
            }
            else if (pointerGameObject.name == "Text1")
            {
                command.scenarios = ScenarioManager.Instance.UpdateLines("Scenario2");
                ScenarioManager.Instance.storyNum = 1;
            }
            else if (pointerGameObject.name == "Text2")
            {
                command.scenarios = ScenarioManager.Instance.UpdateLines("Scenario2");
                ScenarioManager.Instance.storyNum = 2;
            }
        }
    }

    public void SelectCommand()
    {
        command.SelectCommand("Button");
    }
}
