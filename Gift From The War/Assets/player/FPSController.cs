using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    //ゲームオブジェクトやスクリプトをとってくる

    [SerializeField] private GameObject ladderHund;
    [SerializeField] private GameObject cam;

    private PlayerStartDown playerStartDown;
    private magnet magnet;
    private magnetChain magnetChain;
    private playerHundLadder ladder;
    private playerDied died;
    private CharacterController CC;
    private MoveWindGun moveWind;

    //プレイヤーのXZ移動
    private Transform trans;
    [SerializeField] private float walkSpeed;       //歩行速度
    [SerializeField] private float dashSpeedRaito;  //走る速さの倍率
    private float nowMoveSpeed;                     //今の移動速度
    public bool moveFlg { get; set; }
    public bool dashFlg { get; set; }

    //プレイヤー移動全般
    private Vector3 moveVelocity; // キャラの移動速度情報
    private Vector3 moveVec; // 合成用

    //プレイヤーの回転
    Quaternion cameraRot, characterRot;
    [SerializeField] private float sensityvity;    //振り向く感度
    [SerializeField] private float turnSpeed; //振り向く速さ
    private float turnRaito = 180; //振り向く段階
    [SerializeField] private float minX = -45f, maxX = 45f; //角度の制限用
    private float nowSensityvity;  //今の振りむき感度

    [SerializeField] bool isDebug;
    [SerializeField] bool useUI;

    private void Awake()
    {
        if (isDebug)
        {
            SaveManager.Instance.nowSaveData.getGunFlg = true;
            SaveManager.Instance.nowSaveData.getWindFlg = true;
            SaveManager.Instance.nowSaveData.getMagnetFlg = true;
            SaveManager.Instance.nowSaveData.getFireFlg = true;
        }
    }

    void Start()
    {
        //変数を初期化
        GunUseInfo _info = transform.GetComponent<GunUseInfo>();

        magnet = _info.muzzlePos.GetComponent<magnet>();
        magnetChain = _info.muzzlePos.GetComponent<magnetChain>();
        ladder = transform.GetComponent<playerHundLadder>();
        moveWind = transform.GetComponent<MoveWindGun>();
        died = transform.GetComponent<playerDied>();
        CC = transform.GetComponent<CharacterController>();
        playerStartDown = transform.GetComponent<PlayerStartDown>();

        trans = transform;
        nowMoveSpeed = walkSpeed;
        moveFlg = false;
        dashFlg = false;
        CursorManager.Instance.cursorLock = true;

        if (isDebug == false)
        {
            CC.enabled = false;
            trans.position = SaveManager.Instance.nowSaveData.dataSpotPos;
            CC.transform.position = SaveManager.Instance.nowSaveData.dataSpotPos;
            CC.enabled = true;
        }
    }

    void Update()
    {
        if (Time.timeScale <= 0f)return;
        if (playerStartDown != null && playerStartDown.isAuto == true) return;

        UpdateCursorLock();

        if (ladder.touchLadderFlg || died.diedFlg || magnetChain.metalFlg)  //プレイヤーの移動無効化
        {
            moveFlg = false;
            return;
        }

        AssignTmpRot();

        MiddleClick();

        if (turnRaito >= 180) //180ターンを使っていなかったら
        {
            CameraMove();
        }

        cameraRot = ClampRotation(cameraRot);

        AssignFinalRot();

        MoveKey();

        DashJudge();

        Dash();

        Move();

        CC.Move(moveVec * Time.deltaTime);

    }

    //--------------------------------------------------------------------
    //自作関数
    //--------------------------------------------------------------------
    private void UpdateCursorLock()  //カーソル表示切り替え
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    CursorManager.Instance.cursorLock = false;
        //}
        //else if (Input.GetMouseButton(0))
        //{
        //    CursorManager.Instance.cursorLock = true;
        //}
    }

    private void AssignTmpRot()    //計算するために回転量を保持する
    {
        cameraRot = cam.transform.localRotation;
        characterRot = trans.localRotation;
    }

    private void AssignFinalRot()   //計算した回転量を代入
    {
        cam.transform.localRotation = cameraRot;
        trans.localRotation = characterRot;
    }

    private void MiddleClick()
    {
        if (moveWind.effectFlg || magnet.metal != null) return; //一部例外あり

        if (Input.GetMouseButtonDown(2))
        {
            turnRaito = 0;
        }

        if (turnRaito < 180)
        {
            //Y軸回転だけにする
            cameraRot[0] = 0;
            cameraRot[2] = 0;

            turnRaito += turnSpeed * Time.deltaTime;    //指定した角度になるまで回転する

            characterRot *= Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0); //回転を保持
        }
    }

    private void MoveKey()
    {
        //移動方向をとってくる
        moveVelocity.x = Input.GetAxis("Horizontal");
        moveVelocity.z = Input.GetAxis("Vertical");

        moveFlg = !(moveVelocity.x == 0 && moveVelocity.z == 0);
    }

    private void Move()
    {
        if (moveFlg) AudioManager.Instance.PlaySE("walk");
        else AudioManager.Instance.StopSE("walk");

        //移動方向を計算
        moveVec = cam.transform.forward * moveVelocity.z + cam.transform.right * moveVelocity.x;
        moveVec.y = 0;

        //移動量を計算
        moveVec.Normalize();
        moveVec *= nowMoveSpeed;
    }

    private void CameraMove()
    {
        CalSensityvity();

        //マウスから角度を計算
        float xRot = Input.GetAxis("Mouse X") * nowSensityvity;
        float yRot = Input.GetAxis("Mouse Y") * nowSensityvity;

        //カメラとキャラクターの回転を代入
        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);
    }

    private void CalSensityvity()   //カメラの感度の算出
    {
        if (magnet.metal != null)    //磁石の能力を使っていたら、遅くする
        {
            nowSensityvity = magnet.sensityvity;
        }
        else
        {
            nowSensityvity = sensityvity;
        }
    }

    private void DashJudge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) == true)
        {
            dashFlg = !dashFlg;
        }
    }

    private void Dash()
    {
        nowMoveSpeed = dashFlg ? walkSpeed * dashSpeedRaito : walkSpeed;
    }

    private Quaternion ClampRotation(Quaternion q)  //角度制限
    {
        //q = x,y,z,w (x,y,zはベクトル（量と向き）：wはスカラー（座標とは無関係の量）)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }
}
