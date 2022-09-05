using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Scenario : MonoBehaviour
{
    //  �R�}���h�I�u�W�F�N�g�Q�Ɨp
    [SerializeField] GameObject image;

    // �b�������鎞�̃A�C�R���p
    [SerializeField] GameObject talkIcon;

    // �b����������
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
            //  �R�}���h��ʂ�\��
            OpenCommand();

            storyManager.hitFlg = false;
        }

        //  �R�}���h��ʏI��
        EndCommand();
    }


    //  ���͂�ǂݏI�������R�}���h��ʂ��I��������
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

    //�@CommandScript����Ăяo���R�}���h��ʂ̏I��
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        CursorManager.Instance.cursorLock = true;

        scenarioFlg = false;

    }

    //  �R�}���h��ʕ\��
    public void OpenCommand()
    {
        image.SetActive(true);
        talkIcon.SetActive(false);
        CursorManager.Instance.cursorLock = false;
        scenarioFlg = true;
    }
}
