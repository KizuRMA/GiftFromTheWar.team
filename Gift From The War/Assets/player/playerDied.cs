using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerDied : MonoBehaviour
{
    //ゲームオブジェクトやスクリプト
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private Gravity gravity;
    [SerializeField] private GameObject rantan;
    private GameObject gun;
    private Rigidbody rantanRD;
    private Rigidbody gunRD;

    public bool diedFlg { get; set; }

    //移動
    [SerializeField] private float height;      //高さ
    [SerializeField] private float downSpeed;   //下がるスピード
    [SerializeField] private float downMax;     //下がる最大値
    private float nowGravity;                   //今の重力加速度
    private bool groundFlg = false;             //一度でも地面についたかどうか

    //回転
    [SerializeField] private float rotSpeed;    //回転スピード
    [SerializeField] private float rotMax;      //回転の最大値
    private float rotSum = 0;                   //回転の合計値
    [SerializeField] private float gunRotSpeed; //銃の回転スピード

    //目を閉じる
    [SerializeField] private GameObject eye;    //目の画像
    [SerializeField] private GameObject eye2;   //目の画像
    [SerializeField] private float eyeCoolTime; //目を閉じるまでの、クールタイム
    [SerializeField] private float sceneCoolTime; //目を閉じるまでの、クールタイム
    [SerializeField] private float moveEyeSpeed;//瞼の動く速さ
    private RectTransform eyeRec;               //目の画像情報
    private RectTransform eye2Rec;              //目の画像情報
    private bool eyeCloseFlg = false;           //目を閉じるフラグ
    private float eyeTime = 0;  //倒れて数秒後目を閉じる処理に使用する

    void Start()
    {
        //変数を初期化
        GunUseInfo _info = transform.GetComponent<GunUseInfo>();
        gun = _info.gunModel;

        CC = this.GetComponent<CharacterController>();
        trans = transform;
        rantanRD = rantan.GetComponent<Rigidbody>();
        gunRD = gun.GetComponent<Rigidbody>();
        diedFlg = false;
        nowGravity = gravity.GetGravity * Time.deltaTime;
        eyeRec = eye.GetComponent<RectTransform>();
        eye2Rec = eye2.GetComponent<RectTransform>();
        //eyeRec.localPosition = new Vector3(0 ,900, 0);
        //eye2Rec.localPosition = new Vector3(0, -900, 0);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    CC.GetComponent<playerAbnormalcondition>().life = 0;
        //}

        if (CC.GetComponent<playerAbnormalcondition>().life <= 0)   //HPが０になっていたら
        {
            diedFlg = true;
            playerHundLadder _ladder = transform.GetComponent<playerHundLadder>();
            _ladder.FinishLadder();

            CC.height = height;

            //親子関係削除
            rantan.transform.parent = null;
            gun.transform.parent = null;

            rantanRD.useGravity = true;
            gunRD.useGravity = true;

            //移動角度制限削除
            rantanRD.constraints = RigidbodyConstraints.None;
            gunRD.constraints = RigidbodyConstraints.None;

            //タイムアタック関係
            if (TimeAttackManager.Instance.timeAttackFlg)
            {
                TimeAttackManager.Instance.timerStopFlg = true;
                TimeAttackManager.Instance.timerStartFlg = false;
                TimeAttackManager.Instance.playerDiedFlg = true;
                TimeAttackManager.Instance.TimerHide();
            }
        }

        if (!diedFlg) return;

        DownKnees();

        Fall();

        if (!eyeCloseFlg) return;

        MoveEye();
    }

    private void DownKnees()    //膝をつく
    {
        if (groundFlg) return;  //地面についていたら通らない

        eyeTime += Time.deltaTime;
        //レイ判定で地面に着いたか確認する
        Ray ray = new Ray(trans.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, downMax) || eyeTime >= 2.0f)
        {
            groundFlg = true;
            StartCoroutine(EyeCoolTime());
            StartCoroutine(SceneCoolTime());
        }

        nowGravity += gravity.GetGravity * Time.deltaTime;
        CC.Move(new Vector3(0, nowGravity, 0) * Time.deltaTime);   //プレイヤーを移動
    }

    private void Fall() //倒れるときの回転
    {
        if (!groundFlg) return;
        if (rotSum > rotMax) return;

        rotSum += rotSpeed * Time.deltaTime;    //回転の合計を保存
        trans.rotation *= Quaternion.Euler(rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime);    //プレイヤーを回転
        gun.transform.rotation *= Quaternion.Euler(0, 0, gunRotSpeed * Time.deltaTime); //銃を回転
    }

    private IEnumerator EyeCoolTime()  //回復までのクールタイム
    {
        yield return new WaitForSeconds(eyeCoolTime);  //クールタイム分待つ

        eyeCloseFlg = true;
        eye.SetActive(true);
        eye2.SetActive(true);
    }

    private void MoveEye()  //目を動かす
    {
        if (eyeRec.localPosition.y > 0)
        {
            eyeRec.localPosition += new Vector3(0, -moveEyeSpeed * Time.deltaTime, 0);
        }

        if (eye2Rec.localPosition.y < 0)
        {
            eye2Rec.localPosition += new Vector3(0, moveEyeSpeed * Time.deltaTime, 0);
        }
    }

    private IEnumerator SceneCoolTime()  //回復までのクールタイム
    {
        yield return new WaitForSeconds(sceneCoolTime);  //クールタイム分待つ

        CursorManager.Instance.SetCursorLock(false);

        SceneManager.LoadScene("GameOverScene");
    }
}
