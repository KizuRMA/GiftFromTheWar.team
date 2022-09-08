using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    //�I�u�W�F�N�g��X�N���v�g���Ƃ��Ă���
    [SerializeField] private Transform camTrans;

    //  ���C����
    [SerializeField] private float handDis;

    //  ���C���Փ˂��Ă��邩�ǂ���
    public bool hitFlg;

    //  �Փ˂����I�u�W�F�N�g�̏���ێ����Ă���
    private string objName;

    public GameObject neziKun;

    // �b�������鎞�̃A�C�R���p
    [SerializeField] public GameObject talkIcon;

    Scenario scenario;

    // Start is called before the first frame update
    void Start()
    {
        scenario = GameObject.Find("ScenarioManager").GetComponent<Scenario>();
        talkIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray();

        if(hitFlg==true)
        {
            scenario.OpenCommand();
            hitFlg = false;
            talkIcon.SetActive(false);
        }
    }

    //���C����
    void Ray()
    {
        if (scenario.scenarioFlg) return;

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
                // �X�y�[�X�L�[�ŉ�b�E�B���h�E�\��
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    hitFlg = true;
                }

                talkIcon.SetActive(true);
            }
        }
        else
        {
            talkIcon.SetActive(false);
        }
    }
}
