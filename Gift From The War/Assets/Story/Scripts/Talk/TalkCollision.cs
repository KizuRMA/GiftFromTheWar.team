using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCollision : MonoBehaviour
{
    [SerializeField] private Transform camTrans;
    //  レイ判定
    [SerializeField] private float handDis;

    //  レイが衝突しているかどうか
    private bool hitFlg;

    //  衝突したオブジェクトの情報を保持しておく
    private GameObject hitObj;

    PlayerTalk playerTalk;

    // Start is called before the first frame update
    void Start()
    {
        playerTalk = GetComponent<PlayerTalk>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray();
        
        if(hitFlg)
        {
            playerTalk.StartTalk();
            hitFlg = false;
        }
    }

    private void Ray()
    {
        if (playerTalk.talkFlg) return;

        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        //  レイ投射
        if (Physics.Raycast(ray, out hit, handDis))
        {
            //  衝突しているオブジェクトの各情報を取得
            hitObj = hit.collider.gameObject;

            //  衝突しているオブジェクトがネジ君だったら
            if (hitObj.CompareTag("Nezi"))
            {
                playerTalk.OnActiceIcon();
                playerTalk.SetTalkPartner(hitObj);

                // スペースキーが押されなかったら処理しない
                if (!Input.GetKeyDown(KeyCode.Space)) return;
                hitFlg = true;
            }
        }
        else
        {
            playerTalk.OffActiceIcon();
        }
    }
}
