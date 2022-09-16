using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Scenario : MonoBehaviour
{
    //  �R�}���h�I�u�W�F�N�g�Q�Ɨp
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
        if (Time.timeScale <= 0) return;
        //  �R�}���h��ʏI��
        EndCommand();
    }


    //  ���͂�ǂݏI�������R�}���h��ʂ��I��������
    public void EndCommand()
    {
        if (!ScenarioManager.Instance.endFlg) return;

        image.SetActive(false);

        ScenarioManager.Instance.talkCount++;

        CursorManager.Instance.SetCursorLock(true);
        ScenarioData.Instance.WriteFile();

        scenarioFlg = false;
        ScenarioManager.Instance.endFlg = false;
    }

    //�@CommandScript����Ăяo���R�}���h��ʂ̏I��
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        CursorManager.Instance.SetCursorLock(true);

        scenarioFlg = false;

    }

    //  �R�}���h��ʕ\��
    public void OpenCommand()
    {
        image.SetActive(true);
        CursorManager.Instance.SetCursorLock(false);
        scenarioFlg = true;
    }
}
