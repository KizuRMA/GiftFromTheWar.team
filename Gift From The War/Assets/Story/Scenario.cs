using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Scenario : MonoBehaviour
{
    //オブジェクトやスクリプトをとってくる
    [SerializeField] private Transform camTrans;

    //  プレイヤー関係
    private FPSController fpsController;
    private MoveWindGun moveWindGun;

    //  レイ判定
    [SerializeField] private float handDis;

    //  レイが衝突しているかどうか
    private bool hitFlg;

    //  衝突したオブジェクトの情報を保持しておく
    private string objName;

    //  会話する相手
    private GameObject neziKun = null;

    //  コマンドオブジェクト参照用
    [SerializeField] GameObject image;

    public int count=0;

    private void Awake()
    {
        fpsController = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>();
        moveWindGun = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveWindGun>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //  コマンド画面を閉じておく
        image.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //  レイ判定
        Ray();

        if (hitFlg==true)
        {
            //  コマンド画面を表示
            OpenCommand();
           
            //  コマンド画面表示中はプレイヤーの動作をオフにする
            fpsController.enabled = false;
            moveWindGun.enabled = false;

            count++;

            hitFlg = false;
        }

        //  コマンド画面終了
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
            //  衝突しているオブジェクトの各情報を取得
            objName = hit.collider.gameObject.tag;
            neziKun = hit.collider.gameObject;

            //  衝突しているオブジェクトがネジ君だったら
            if (objName == "Nezi")
            {
                hitFlg = true;
            }
        }
    }

    //  文章を読み終わったらコマンド画面を終了させる
    void EndCommand()
    {
        if (!ScenarioManager.Instance.endFlg) return;

        image.SetActive(false);
        neziKun.SetActive(false);

        ScenarioManager.Instance.endFlg = false;
        CursorManager.Instance.cursorLock = true;


        StartCoroutine(Resume());
    }

    //　CommandScriptから呼び出すコマンド画面の終了
    public void ExitCommand()
    {
        EventSystem.current.SetSelectedGameObject(null);
        CursorManager.Instance.cursorLock = true;
        fpsController.enabled = true;
        moveWindGun.enabled = true;
    }

    //  コマンド画面表示
    private void OpenCommand()
    {
        image.SetActive(true);
        CursorManager.Instance.cursorLock = false;
    }

    IEnumerator Resume()
    {
        //1秒停止
        yield return new WaitForSeconds(0.5f);

        fpsController.enabled = true;
        moveWindGun.enabled = true;

    }
}
