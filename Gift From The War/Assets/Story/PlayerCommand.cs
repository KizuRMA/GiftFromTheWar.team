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

    private string objName;

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
        //�@�R�}���hUI�̕\���E��\���̐؂�ւ�
        if (Input.GetKeyDown(KeyCode.Space)&&Ray(true))
        {
            //�@�R�}���h
            if (!commandUI.activeSelf)
            {
               
            }
            else
            {
                ExitCommand();
            }
            //�@�R�}���hUI�̃I���E�I�t
            commandUI.SetActive(!commandUI.activeSelf);
            CursorManager.Instance.cursorLock = false;

        }
    }

    //���C����
    private bool Ray(bool value)
    {
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, handDis))
        {
            objName = hit.collider.gameObject.name;

            if(objName=="NeziKun")
            {
                return true;
            }
        }
            return false;
    }


    //�@CommandScript����Ăяo���R�}���h��ʂ̏I��
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Debug.Log("Call");
    }
}
