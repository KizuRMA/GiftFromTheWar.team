using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WingFoldState : BaseState
{
    enum e_Action
    {
        none,
        search,
        move,
        sticking,
        leave,
    }

    [SerializeField] float ultrasoundCoolTime;
    [SerializeField] float ascendingSpeed;
    private Vector3 targetPos;
    private RaycastHit hit;
    private CharacterController playerCC;
    private NavMeshAgent agent;
    private UltraSound ultrasound;
    private GameObject childGameObject;
    private bool nextAnime;
    private bool navmeshOnFlg;
    private float frame;
    private float distance;
    private float defaltHight;
    private e_Action nowAction;
    private float rotateY;
    private float untilLaunch;
    private float amountChangeAngX;
    private float amountChangeDis;

    // Start is called before the first frame update
    public override void Start()
    {
        rotateY = transform.eulerAngles.y;
        nextAnime = false;
        navmeshOnFlg = true;
        nowAction = e_Action.search;
        untilLaunch = 0;
        distance = 0;
        defaltHight = 0;
        frame = 20;

        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        playerCC = GameObject.Find("player").GetComponent<CharacterController>();
        childGameObject = transform.Find("Capsule").gameObject;
        childGameObject.GetComponent<CapsuleCollider>().enabled = true;
        childGameObject.GetComponent<BatCapsuleScript>().Start();

        ultrasound = GetComponent<UltraSound>();
        ultrasound.Init();

        CurrentState = (int)BatController.e_State.wingFold;
    }

    // Update is called once per frame
    public override void Update()
    {
        bool _navmeshFlg = navmeshOnFlg;

        if (agent.isStopped == true)
        {
            //体を回転させる処理
            if (myController.forwardAngle >= 90)
            {
                transform.eulerAngles = new Vector3(180.0f - myController.forwardAngle, rotateY + 180.0f, 180.0f);
            }
            else
            {
                transform.eulerAngles = new Vector3(myController.forwardAngle, rotateY, 0);
            }
        }
        else
        {
            myController.SimpleAdjustHeight();
        }

        //現在のアクション状態毎に関数を実行する
        switch (nowAction)
        {
            //張り付いた状態
            case e_Action.none:
                ActionNone();
                break;
            //張り付く場所を探す
            case e_Action.search:
                ActionSearch();
                break;
            case e_Action.move:
                ActionMove();
                break;
            //張り付きにいく状態の時
            case e_Action.sticking:
                ActionSticking();
                break;
            //離れる状態の時
            case e_Action.leave:
                ActionLeave();
                break;
        }

        if (agent.isStopped == true)
        {
            //コウモリの座標からワールド座標で下に向いているレイを作る
            int layerMask = 1 << 9;
            Ray _ray = new Ray(transform.position, Vector3.down);
            RaycastHit _raycastHit;

            bool _rayHit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, layerMask);

            Debug.Log(myController.hight);

            //レイがオブジェクトに当たっている場合
            if (_rayHit == true)
            {
                //現在の高さを記録しておく
                myController.hight = _raycastHit.distance;
            }
        }

        //ナビメッシュに変更が加えられている場合
        if (_navmeshFlg != navmeshOnFlg)
        {
            if (navmeshOnFlg == true)
            {
                myController.OnNavMesh();
            }
            else
            {
                myController.OffNavMesh();
            }
        }
    }

    private void ActionSticking()
    {
        Animator animator = GetComponent<Animator>();

        //ターゲットとしている座標までの距離を調べる
        float _targetDis = Vector3.Distance(transform.position, targetPos);

        //天井との距離が移動量よりも大きい、または逆さまになっていない場合
        if (_targetDis >= ascendingSpeed || myController.forwardAngle < 180.0f)
        {
            //上に移動する処理
            transform.position = Vector3.MoveTowards(transform.position, targetPos, ascendingSpeed * Time.deltaTime);
            _targetDis = Vector3.Distance(transform.position, targetPos);
        }
        else
        {
            //アクション状態を変更する
            nowAction = e_Action.none;
            untilLaunch = 0;
            nextAnime = false;
            return;
        }

        //天井との高さが近い場合
        if (_targetDis <= 0.5f)
        {
            GameObject.Find("CollisionDetector").GetComponent<BoxCollider>().enabled = false;

            //コウモリが180度回転していない場合
            if (myController.forwardAngle < 180.0f)
            {
                myController.forwardAngle += 1.0f;

                if (myController.forwardAngle >= 180.0f)
                {
                    myController.forwardAngle = 180.0f;
                }
            }

            if (nextAnime == false)
            {
                //羽を閉じるアニメーションに切り替える
                animator.SetInteger("trans", 1);
                nextAnime = true;
            }
        }
    }

    private void ActionNone()
    {
        untilLaunch += Time.deltaTime;

        //超音波のクールタイムが終了している場合
        if (untilLaunch - ultrasoundCoolTime > 0)
        {
            //超音波を更新
            bool _hit = ultrasound.Update();

            //超音波がプレイヤーに当たっている場合
            if (_hit == true)
            {
                //アクション状態を天井から離れる状態に変化
                nowAction = e_Action.leave;
                targetPos += Vector3.down * 0.8f;
                amountChangeDis = Vector3.Distance(targetPos, transform.position);
                amountChangeAngX = myController.forwardAngle - 20.0f;

                //プレイヤーをハウリング状態にする
                playerCC.GetComponent<playerAbnormalcondition>().AddHowlingAbnormal();

                //アニメーションを切り替える
                Animator animator = GetComponent<Animator>();
                animator.SetInteger("trans", 2);
                ultrasound.Init();
                return;
            }
        }

        //超音波を出し切った場合
        if (ultrasound.IsAlive() == false)
        {
            ultrasound.Init();
            untilLaunch = 0;
        }
    }

    private void ActionSearch()
    {
        //カプセルコライダーの処理が停止している場合は開始する
        CapsuleCollider capsule = childGameObject.GetComponent<CapsuleCollider>();
        if (capsule.enabled == false) capsule.enabled = true;

        //指定のフレーム分数える
        frame--;
        if (frame > 0) return;

        BatCapsuleScript _batCapsule = childGameObject.GetComponent<BatCapsuleScript>();

        if (_batCapsule.colList.Count <= 0)
        {
            nowAction = e_Action.sticking;
            navmeshOnFlg = false;
            targetPos = SearchCeiling(transform.position);
        }
        else
        {
            //障害物を回避できる方向を割り出す
            Vector3 _targetVec = _batCapsule.MoveDirction();
            _targetVec.Normalize();
            _targetVec *= 2.0f;
            _targetVec += transform.position;

            //ナビメッシュないに補完する
            NavMeshHit hit;
            if (NavMesh.SamplePosition(_targetVec, out hit, 1.0f, NavMesh.AllAreas))
            {
                // 位置をNavMesh内に補正
                _targetVec = hit.position;
            }

            distance = Vector3.Distance(_targetVec, transform.position);
            defaltHight = myController.hight;
            targetPos = SearchCeiling(_targetVec);

            agent.destination = _targetVec;
            nowAction = e_Action.move;
        }
    }

    private void ActionMove()
    {
        BatCapsuleScript _batCapsule = childGameObject.GetComponent<BatCapsuleScript>();

        Vector3 _myPos = transform.position;
        Vector3 _targetPos = agent.destination;
        float _maxHight = Vector3.Distance(_targetPos, targetPos);

        _myPos.y = 0;
        _targetPos.y = 0;

        float _targetDistance = Vector3.Distance(_myPos, _targetPos);
        float _amountMove = Mathf.Abs(distance - _targetDistance);

        myController.hight = NeedHight(_amountMove, defaltHight, _maxHight, distance);

        if (_targetDistance <= 0.1f)
        {
            Animator animator = GetComponent<Animator>();

            //コウモリが180度回転していない場合
            if (myController.forwardAngle < 180.0f)
            {
                myController.forwardAngle += 1.0f;

                if (myController.forwardAngle >= 180.0f)
                {
                    myController.forwardAngle = 180.0f;
                }
            }

            //体を回転させる処理
            if (myController.forwardAngle >= 90)
            {
                transform.eulerAngles = new Vector3(180.0f - myController.forwardAngle, rotateY + 180.0f, 180.0f);
            }
            else
            {
                transform.eulerAngles = new Vector3(myController.forwardAngle, rotateY, 0);
            }

            if (nextAnime == false)
            {
                //羽を閉じるアニメーションに切り替える
                animator.SetInteger("trans", 1);
                nextAnime = true;
            }

            childGameObject.GetComponent<CapsuleCollider>().enabled = false;

            if (_targetDistance <= 0.001f)
            {
                navmeshOnFlg = false;
                transform.position = targetPos;
                nowAction = e_Action.none;
            }
        }
        else
        {
            rotateY = transform.eulerAngles.y;
            //体を前に傾ける
            Vector3 _localAngle = transform.localEulerAngles;
            _localAngle.x = myController.forwardAngle;
            transform.localEulerAngles = _localAngle;
        }
    }

    private void ActionLeave()
    {
        //翼を広げるアニメーションに変更
        Animator animator = GetComponent<Animator>();

        //コウモリ移動処理
        transform.position = Vector3.MoveTowards(transform.position, targetPos, amountChangeDis * (Time.deltaTime * 1.5f));

        //コウモリの回転処理
        if (myController.forwardAngle > 20.0f)
        {
            //変化する値
            float _changeAng = amountChangeAngX * (Time.deltaTime * 1.5f);
            myController.forwardAngle -= _changeAng;

            if (myController.forwardAngle <= 20.0f)
            {
                myController.forwardAngle = 20.0f;
            }
        }

        if (Vector3.Distance(targetPos, transform.position) <= 0.001f)
        {
            animator.SetInteger("trans", 0);

            GameObject.Find("CollisionDetector").GetComponent<BoxCollider>().enabled = true;

            //コウモリを追跡ステートに切り替える
            BatController batCon = gameObject.GetComponent<BatController>();
            batCon.ChangeState(GetComponent<batMove>());
            return;
        }
    }

    private Vector3 SearchCeiling(Vector3 _rayPos)
    {
        Vector3 _rayPosition = _rayPos;
        Vector3 _targetPos;

        Ray ray = new Ray(_rayPosition, Vector3.up);

        int layerMask = 1 << 9;

        //レイヤーマスクを"cave"（洞窟）にしてレイ判定を行う
        if (Physics.Raycast(ray, out hit, 1000.0f, layerMask))
        {
            //レイが天井に衝突している場合はターゲット座標に設定する。
            Vector3 _targetVec = Vector3.up * hit.distance;
            _targetPos = _rayPosition + _targetVec;
        }
        else
        {
            _targetPos = Vector3.zero;

            //洞窟外にいるためコウモリを消滅させる
            myController.Damage(1);
        }

        return _targetPos;
    }

    public void OnDetectorObject(Collider collider)
    {
        if (nowAction == e_Action.sticking)
        {
            GameObject.Find("CollisionDetector").GetComponent<BoxCollider>().enabled = false;
        }
    }

    public float NeedHight(float _amountMoved, float _defaltHight, float _maxHight, float _maxDistance)
    {
        float a, q, x, y;
        q = _defaltHight;
        y = _maxHight;
        x = _maxDistance;

        a = (-1.0f * (q - y)) / (x * x);

        float _hight = a * (_amountMoved * _amountMoved) + q;
        return _hight;
    }
}
