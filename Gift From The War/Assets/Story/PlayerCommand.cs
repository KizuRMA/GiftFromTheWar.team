using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerCommand : MonoBehaviour
{
    //�I�u�W�F�N�g��X�N���v�g���Ƃ��Ă���
    [SerializeField] private Transform camTrans;

    //���C����
    [SerializeField] private float handDis;

    private bool hitFlg;

    private string objName;

    private GameObject neziKun=null;

    //�@�R�}���h�pUI
    [SerializeField]
    private GameObject commandUI = null;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        //  ���C����
        Ray();

        //�@�R�}���hUI�̕\��
        if(hitFlg)
        {
            //�@�R�}���hUI�̃I��
            commandUI.SetActive(true);
            CursorManager.Instance.cursorLock = false;
            hitFlg = false;
        }

        EndCommand();
    }

    //���C����
    private void Ray()
    {
        //  �X�y�[�X�L�[���������Ƃ�
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        //  ���C����
        if (Physics.Raycast(ray, out hit, handDis))
        {
            //  �Փ˂��Ă���I�u�W�F�N�g�̃^�O�����擾
            objName = hit.collider.gameObject.tag;
            neziKun = hit.collider.gameObject;

            //  �Փ˂��Ă���I�u�W�F�N�g���l�W�N��������
            if(objName=="Nezi")
            {
                hitFlg = true;
            }
        }     
    }

    //  �R�}���h�̏I����
    void EndCommand()
    {
        if (ScenarioManager.Instance.endFlg)
        {
            neziKun.SetActive(false);
            commandUI.SetActive(false);
            ScenarioManager.Instance.endFlg = false;
        }
    }


    //�@CommandScript����Ăяo���R�}���h��ʂ̏I��
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        CursorManager.Instance.cursorLock = true;
    }
}
