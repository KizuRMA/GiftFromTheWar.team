using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class batMove : BaseState
{
    [SerializeField] GameObject playerCC;
    [SerializeField] bool moveFlg;
    [SerializeField] float playerFromInterval;
    [SerializeField] float ultrasoundCoolTime;
    private NavMeshAgent agent;
    private UltraSoundBeam ultrasound;
    private float untilLaunch;

    // Start is called before the first frame update
    public override void Start()
    {
        ultrasound = null;
        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        playerCC = GameObject.Find("player").gameObject;

        //超音波を初期化
        ultrasound = GetComponent<UltraSoundBeam>();
        ultrasound.Init();

        untilLaunch = Time.time; ;

        myController.OnNavMesh();
    }

    // Update is called once per frame
    public override void Update()
    {
        //体を前に傾ける
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = myController.forwardAngle;
        transform.localEulerAngles = _localAngle;

        //高さを調整する
        myController.AdjustHeight();

        //Debug.Log(transform.position);

        //移動する場合
        if (moveFlg)
        {
            Vector3 _playerPos = playerCC.transform.position;
            Vector3 _myPos = transform.position;

            //プレイヤーに近づきすぎない処理
            float dis = Vector3.Distance(_myPos, _playerPos);

            //プレイヤーとの距離を調べる
            if (dis <= playerFromInterval)
            {
                //近づきすぎている場合
                agent.destination = _myPos;
                BatController batCon = gameObject.GetComponent<BatController>();

                batCon.ChangeState(GetComponent<WingFoldState>());
                //早期リターン
                return;
            }
            else
            {
                if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                {
                    //離れている場合
                    agent.destination = _playerPos;
                }

            }

            //超音波処理
            if (ultrasound != null && untilLaunch + ultrasoundCoolTime <= Time.time)
            {
                ultrasound.Update();
            }

            //超音波を出し切った場合
            if (ultrasound.IsAlive() == false)
            {
                //超音波処理を初期化
                ultrasound.Init();
                untilLaunch = Time.time;
            }
        }
    }
}
