using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerCommand : MonoBehaviour
{
    //オブジェクトやスクリプトをとってくる
    [SerializeField] private Transform camTrans;

    //レイ判定
    [SerializeField] private float handDis;

    private bool hitFlg;

    private string objName;

    private GameObject neziKun=null;

    //　コマンド用UI
    [SerializeField]
    private GameObject commandUI = null;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        //  レイ判定
        Ray();

        //　コマンドUIの表示
        if(hitFlg)
        {
            //　コマンドUIのオン
            commandUI.SetActive(true);
            CursorManager.Instance.cursorLock = false;
            hitFlg = false;
        }

        EndCommand();
    }

    //レイ判定
    private void Ray()
    {
        //  スペースキーを押したとき
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        //  レイ投射
        if (Physics.Raycast(ray, out hit, handDis))
        {
            //  衝突しているオブジェクトのタグ名を取得
            objName = hit.collider.gameObject.tag;
            neziKun = hit.collider.gameObject;

            //  衝突しているオブジェクトがネジ君だったら
            if(objName=="Nezi")
            {
                hitFlg = true;
            }
        }     
    }

    //  コマンドの終了時
    void EndCommand()
    {
        if (ScenarioManager.Instance.endFlg)
        {
            neziKun.SetActive(false);
            commandUI.SetActive(false);
            ScenarioManager.Instance.endFlg = false;
        }
    }


    //　CommandScriptから呼び出すコマンド画面の終了
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        CursorManager.Instance.cursorLock = true;
    }
}
