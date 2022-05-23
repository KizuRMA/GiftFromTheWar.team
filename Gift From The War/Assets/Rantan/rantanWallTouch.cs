using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanWallTouch : MonoBehaviour
{
    Transform trans;
    [SerializeField] float force;
    Vector3 firstPos;
    Rigidbody rd;
    bool returnFlg = true;
    bool returnFinishFlg = true;
    [SerializeField] float firstSpeed;
    [SerializeField] float speedRaito;
    float nowSpeed;
    Vector3 forceVec;

    // Start is called before the first frame update
    void Start()
    {
        trans = transform;
        rd = this.GetComponent<Rigidbody>();
        firstPos = transform.localPosition;
    }




    /// <summary>
    /// �A�b�v�f�[�g
    /// </summary>
    void Update()
    {
        EraseInertia();

        //�����߂�����
        if (returnFlg)
        {
            Return();
        }
    }

    private void EraseInertia() //����������
    {
        rd.velocity = Vector3.zero;
        rd.angularVelocity = Vector3.zero;
    }

    private void Return() //�߂�����
    {
        ReturnSpeedUp();

        CalculateForceVec();

        ForceVecCorrection();

        trans.localPosition += forceVec;
    }

    private void ReturnSpeedUp() //�߂������𑬂�����
    {
        nowSpeed *= speedRaito;
    }

    private void CalculateForceVec() //forceVec�̌v�Z  
    {
        forceVec = firstPos - trans.localPosition;
    }

    private void ForceVecCorrection() //�Ԃ�Ԃ邵�Ȃ����߂̕␳
    {
        bool froceVecSmallFlg = forceVec.magnitude > nowSpeed;  //forceVec�����������Ȃ�������

        if (froceVecSmallFlg)
        {
            forceVec = forceVec.normalized * nowSpeed;
        }
        else
        {
            nowSpeed = firstSpeed;
            returnFlg = false;
            returnFinishFlg = true;
        }
    }




    /// <summary>
    /// �Փ�
    /// </summary>
    /// <param name="collison"></param>
    private void OnCollisionEnter(Collision collison)
    {
        returnFlg = false;
        returnFinishFlg = false;
        nowSpeed = firstSpeed;
    }





    /// <summary>
    /// �ՓˏI��
    /// </summary>
    /// <param name="collison"></param>
    private void OnCollisionExit(Collision collison)
    {
        returnFlg = true;
    }


    public bool GetReturnFinishFlg()
    {
        return returnFinishFlg;
    }
}
