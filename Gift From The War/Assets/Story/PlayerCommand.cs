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

    private string objName;

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
        //　コマンドUIの表示・非表示の切り替え
        if (Input.GetKeyDown(KeyCode.Space)&&Ray(true))
        {
            //　コマンド
            if (!commandUI.activeSelf)
            {
               
            }
            else
            {
                ExitCommand();
            }
            //　コマンドUIのオン・オフ
            commandUI.SetActive(!commandUI.activeSelf);
            CursorManager.Instance.cursorLock = false;

        }
    }

    //レイ判定
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


    //　CommandScriptから呼び出すコマンド画面の終了
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Debug.Log("Call");
    }
}
