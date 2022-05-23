using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class batMove : BaseState
{
    enum e_Action
    {
        move,
        search,
        check,
    }

    [SerializeField] GameObject playerCC;
    [SerializeField] bool moveFlg;
    [SerializeField] float playerFromInterval;
    private NavMeshAgent agent;
    private float untilLaunch;
    private e_Action nowAction;
    private bool navmeshOnFlg;


    // Start is called before the first frame update
    public override void Start()
    {
        navmeshOnFlg = true;
        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        playerCC = GameObject.Find("player").gameObject;

        //超音波を初期化
        ChangeUltrasound(GetComponent<UltraSoundBeam>());

        CurrentState = (int)BatController.e_State.move;
        nowAction = e_Action.move;
        untilLaunch = 0;

        myController.OnNavMesh();
    }

    // Update is called once per frame
    public override void Update()
    {
        bool _navmeshFlg = navmeshOnFlg;

        untilLaunch += Time.deltaTime;

        //体を前に傾ける
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = myController.forwardAngle;
        transform.localEulerAngles = _localAngle;

        //高さを調整する
        myController.AdjustHeight();

        //超音波処理
        float _ultrasoundCoolTime = ultrasound.coolDown;
        if (ultrasound != null && untilLaunch - _ultrasoundCoolTime > 0)
        {
            ultrasound.Update();
            ultrasound.DrawLine();
        }

        switch (nowAction)
        {
            case e_Action.move:
                ActionMove();
                break;
            case e_Action.search:
                ActionSearch();
                break;
            case e_Action.check:
                ActionCheck();
                break;
        }

        //超音波を出し切った場合
        if (ultrasound.IsAlive == false)
        {
            //超音波処理を初期化
            ultrasound.Init();
            untilLaunch = 0;
        }

    }

    private void ActionMove()
    {
        playerAbnormalcondition abnormalcondition = playerCC.GetComponent<playerAbnormalcondition>();

        //主人公がハウリング状態の時
        if (abnormalcondition.IsHowling() == true)
        {
            //移動する場合
            if (moveFlg)
            {
                Vector3 _playerPos = playerCC.transform.position;
                Vector3 _myPos = transform.position;

                float _dis = Vector3.Distance(_playerPos, _myPos);

                if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                {
                    //離れている場合
                    agent.destination = _playerPos;
                }

                if (ultrasound.CheckHit() == true)
                {
                    //プレイヤーにハウリング状態を付加する
                    abnormalcondition.AddHowlingAbnormal();
                }
            }
        }
        else
        {
            ultrasound.Init();

            if (agent.velocity.magnitude <= 0.0f)
            {
                //アクション状態をサーチ状態に変化
                nowAction = e_Action.search;
                Animator animator = GetComponent<Animator>();
                animator.SetTrigger("ShakeHead");

                ChangeUltrasound(GetComponent<SmallUltrasound>());
            }
        }
    }

    private void ActionSearch()
    {
        if (ultrasound.CheckHit() == true)
        {
            playerAbnormalcondition abnormalcondition = playerCC.GetComponent<playerAbnormalcondition>();
            abnormalcondition.AddHowlingAbnormal();
        }
    }

    private void ActionCheck()
    {
        playerAbnormalcondition abnormalcondition = playerCC.GetComponent<playerAbnormalcondition>();

        if (ultrasound.CheckHit() == true || abnormalcondition.IsHowling() == true)
        {
            ultrasound.Init();
            untilLaunch = 0;

            //プレイヤーにハウリング状態を付加する
            abnormalcondition.AddHowlingAbnormal();
            nowAction = e_Action.move;
            ChangeUltrasound(GetComponent<UltraSoundBeam>());
            return;
        }

        //超音波を出し切った場合
        if (ultrasound.IsAlive == false)
        {
            ultrasound.Init();
            untilLaunch = 0;

            BatController batCon = gameObject.GetComponent<BatController>();
            batCon.ChangeState(GetComponent<WingFoldState>());
            //早期リターン
            return;
        }
    }

    public void SearchPlayerAction()
    {
        nowAction = e_Action.check;
        ChangeUltrasound(GetComponent<LargeUltrasound>());
    }

}
