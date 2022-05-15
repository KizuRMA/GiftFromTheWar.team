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
    private Vector3 rayPosition;
    private Vector3 targetPos;
    private RaycastHit hit;
    private CharacterController playerCC;
    private NavMeshAgent agent;
    private UltraSound ultrasound;
    private GameObject childGameObject;
    private bool nextAnime;
    private bool decisionDir;
    private bool navmeshOnFlg;
    private float frame;
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
        decisionDir = false;
        navmeshOnFlg = true;
        nowAction = e_Action.move;
        untilLaunch = 0;
        frame = 10;

        myController = GetComponent<BatController>();
        agent = GetComponent<NavMeshAgent>();
        playerCC = GameObject.Find("player").GetComponent<CharacterController>();
        childGameObject = transform.Find("Capsule").gameObject;
        childGameObject.GetComponent<CapsuleCollider>().enabled = true;
        childGameObject.GetComponent<BatCapsuleScript>().Start();

        ultrasound = GetComponent<UltraSound>();
        ultrasound.Init();
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
            Ray _ray = new Ray(transform.position, Vector3.down);
            RaycastHit _raycastHit;
            bool _rayHit = Physics.Raycast(_ray, out _raycastHit);

            //Debug.Log(_raycastHit.collider.gameObject);

            //レイがオブジェクトに当たっている場合
            if (_rayHit == true)
            {
                //現在の高さを記録しておく
                myController.hight = _raycastHit.distance;
                // Debug.Log(myController.hight);
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
            untilLaunch = Time.time;
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
        //超音波のクールタイムが終了している場合
        if (untilLaunch + ultrasoundCoolTime <= Time.time)
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
            untilLaunch = Time.time;
        }
    }

    private void ActionSearch()
    {
        rayPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Ray ray = new Ray(rayPosition, Vector3.up);

        //Rayを上に飛ばす
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "cave")
            {
                //レイが天井に衝突している場合はターゲット座標に設定する。
                Vector3 targetVec = Vector3.up * hit.distance;
                targetPos = rayPosition + targetVec;
                nowAction = e_Action.sticking;
            }
            else
            {
                transform.position += transform.forward.normalized * 0.01f;
            }
        }
        else
        {
            BatController batCon = gameObject.GetComponent<BatController>();
            batCon.ChangeState(GetComponent<batMove>());
            return;
        }
    }

    private void ActionMove()
    {
        //カプセルコライダーの処理が停止している場合は開始する
        CapsuleCollider capsule = childGameObject.GetComponent<CapsuleCollider>();
        if (capsule.enabled == false) capsule.enabled = true;

        rotateY = transform.eulerAngles.y;

        //体を前に傾ける
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = myController.forwardAngle;
        transform.localEulerAngles = _localAngle;

        myController.AdjustHeight();

        frame--;
        if (frame > 0)return;

        BatCapsuleScript _batCapsule = childGameObject.GetComponent<BatCapsuleScript>();

        if (_batCapsule.colList.Count <= 0)
        {
            nowAction = e_Action.search;
            navmeshOnFlg = false;
        }
        else
        {
            //目的地が決まっていない場合
            if (decisionDir == false)
            {
                Vector3 _targetVec = _batCapsule.MoveDirction();
                _targetVec.Normalize();
                _targetVec *= 2.0f;
                _targetVec += transform.position;

                agent.destination = _targetVec;
                decisionDir = true;
            }

            Vector3 _myPos = transform.position;
            Vector3 _targetPos = agent.destination;
            _myPos.y = 0;
            _targetPos.y = 0;

            if (Vector3.Distance(_myPos, _targetPos) <= 0.01f)
            {
                nowAction = e_Action.search;
                childGameObject.GetComponent<CapsuleCollider>().enabled = false;

                //ナビメッシュによるオブジェクトの回転を更新しない
                navmeshOnFlg = false;
            }
        }
    }

    private void ActionLeave()
    {
        //翼を広げるアニメーションに変更
        Animator animator = GetComponent<Animator>();
        float _nowTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

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

    public void OnDetectorObject(Collider collider)
    {
        if (nowAction == e_Action.sticking)
        {
            GameObject.Find("CollisionDetector").GetComponent<BoxCollider>().enabled = false;
        }
    }
}
