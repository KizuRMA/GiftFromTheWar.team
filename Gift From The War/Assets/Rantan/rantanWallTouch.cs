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
    /// アップデート
    /// </summary>
    void Update()
    {
        EraseInertia();

        //押し戻す処理
        if (returnFlg)
        {
            Return();
        }
    }

    private void EraseInertia() //慣性を消す
    {
        rd.velocity = Vector3.zero;
        rd.angularVelocity = Vector3.zero;
    }

    private void Return() //戻す処理
    {
        ReturnSpeedUp();

        CalculateForceVec();

        ForceVecCorrection();

        trans.localPosition += forceVec;
    }

    private void ReturnSpeedUp() //戻す速さを速くする
    {
        nowSpeed *= speedRaito;
    }

    private void CalculateForceVec() //forceVecの計算  
    {
        forceVec = firstPos - trans.localPosition;
    }

    private void ForceVecCorrection() //ぶるぶるしないための補正
    {
        bool froceVecSmallFlg = forceVec.magnitude > nowSpeed;  //forceVecが小さすぎないか見る

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
    /// 衝突
    /// </summary>
    /// <param name="collison"></param>
    private void OnCollisionEnter(Collision collison)
    {
        returnFlg = false;
        returnFinishFlg = false;
        nowSpeed = firstSpeed;
    }





    /// <summary>
    /// 衝突終了
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
