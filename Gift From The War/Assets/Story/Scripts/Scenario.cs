using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Scenario : MonoBehaviour
{
    //�I�u�W�F�N�g��X�N���v�g���Ƃ��Ă���
    [SerializeField] private Transform camTrans;

    //  ���C����
    [SerializeField] private float handDis;

    //  ���C���Փ˂��Ă��邩�ǂ���
    private bool hitFlg;

    //  �Փ˂����I�u�W�F�N�g�̏���ێ����Ă���
    private string objName;

    //  ��b���鑊��
    private GameObject neziKun = null;

    //  �R�}���h�I�u�W�F�N�g�Q�Ɨp
    [SerializeField] GameObject image;

    // �b�������鎞�̃A�C�R���p
    [SerializeField] GameObject talkIcon;

    // �b����������
    public int talkCount=0;

    public bool scenarioFlg=false;

    private void Awake()
    {
     
    }

    // Start is called before the first frame update
    void Start()
    { 
        //  �R�}���h��ʂ���Ă���
        image.SetActive(false);
        talkIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //  ���C����
        Ray();

        if (hitFlg==true)
        {
            //  �R�}���h��ʂ�\��
            OpenCommand();

            hitFlg = false;
        }
        //  �R�}���h��ʏI��
        EndCommand();
    }

    //���C����
    private void Ray()
    {
        //  �X�y�[�X�L�[���������Ƃ�
       // if (!Input.GetKeyDown(KeyCode.Space)) return;

        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        //  ���C����
        if (Physics.Raycast(ray, out hit, handDis))
        {
            //  �Փ˂��Ă���I�u�W�F�N�g�̊e�����擾
            objName = hit.collider.gameObject.tag;
            neziKun = hit.collider.gameObject;

            //  �Փ˂��Ă���I�u�W�F�N�g���l�W�N��������
            if (objName == "Nezi")
            {
                hitFlg = true;
                talkCount++;

                // ��b�\�A�C�R���\��
                talkIcon.SetActive(true);

            }


        }
    }

    //  ���͂�ǂݏI�������R�}���h��ʂ��I��������
    void EndCommand()
    {
        if (!ScenarioManager.Instance.endFlg) return;

        image.SetActive(false);
        neziKun.SetActive(false);

        ScenarioManager.Instance.endFlg = false;
        CursorManager.Instance.cursorLock = true;

        scenarioFlg = false;
       
    }

    //�@CommandScript����Ăяo���R�}���h��ʂ̏I��
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        CursorManager.Instance.cursorLock = true;

        scenarioFlg = false;
        talkCount = 0;
    }

    //  �R�}���h��ʕ\��
    private void OpenCommand()
    {
        image.SetActive(true);
        talkIcon.SetActive(false);
        CursorManager.Instance.cursorLock = false;
        scenarioFlg = true;
    }
}
