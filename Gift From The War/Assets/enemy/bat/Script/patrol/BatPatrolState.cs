using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum e_BatPatrolState
{
    MoveWayPoints,
    Dead,
    Tracking,
    Attack,
}

public enum e_UltrasoundState
{
    Large,
    Small,
    Beam,
}


public class BatPatrolState : StatefulObjectBase<BatPatrolState, e_BatPatrolState>
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject bat;
    [SerializeField] public Animator animator;
    [SerializeField] public WayPoint wayPoint;
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] public GameObject prefab;

    public BaseUltrasound currentUltrasound;
    protected List<BaseUltrasound> ultrasoundsList = new List<BaseUltrasound>();

    public bool IsNavMeshON => agent.isStopped == false;

    private float limitHight;
    public float hight;
    public float hightRatio;

    void Start()
    {
        hight = 0.8f;
        limitHight = 0.8f;
        hightRatio = 0.4f;
        stateMachine = new StateMachine<BatPatrolState>();


        stateList.Add(new BatMoveWayPointsState(this));
        stateList.Add(new BatDeadState(this));
        stateList.Add(new BatTrackingState(this));

        ChangeState(e_BatPatrolState.MoveWayPoints);

        ultrasoundsList.Add(GetComponent<LargeUltrasound>());

        ChangeUltrasound(e_UltrasoundState.Large);
    }

    protected override void Update()
    {
        //NavMeshAgentの影響でY軸が地面の位置まで下がってしまっているため、高さは自分で管理している
        if (IsNavMeshON == true)
        {
            //体を前に傾ける
            Vector3 _localAngle = transform.localEulerAngles;
            _localAngle.x = 20.0f;
            transform.localEulerAngles = _localAngle;

            AdjustHeight();
        }

        if (currentUltrasound != null)
        {
            currentUltrasound.Update();
        }

        base.Update();
    }

    public void AdjustHeight()
    {
        Ray _ray = new Ray(transform.position, Vector3.up);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        //ステージの立幅を記録
        float _maxHight = _raycastHit.distance;
        float _minHight = _maxHight;

        //ステージの縦幅の４割の位置にいるようにする
        _minHight *= hightRatio;

        //コウモリの飛行上限を設定する
        if (_minHight > limitHight)
        {
            _minHight = limitHight;
        }

        float _targetHight = _maxHight;

        //プレイヤーの地面までの距離を取得
        _ray = new Ray(player.transform.position, Vector3.down);
        _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        if (_hit == true)
        {
            _targetHight = Mathf.Min(Mathf.Max(_raycastHit.distance, _minHight), _maxHight);
        }
        else
        {
            _targetHight = _minHight;
        }

        //現在のコウモリを高さを含んだ座標
        Vector3 nowPos = new Vector3(transform.position.x, hight, transform.position.z);
        //本来いてほしい座標
        Vector3 nextPos = new Vector3(transform.position.x, _targetHight, transform.position.z);

        //ナビメッシュのスピードを用いてコウモリの高さを調整する
        nowPos = Vector3.MoveTowards(nowPos, nextPos, 0.8f * Time.deltaTime);

        //次のフレームでは現在のY軸が保存されないため、記録しておく。
        hight = nowPos.y;

        transform.position = new Vector3(transform.position.x, transform.position.y + hight, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "windBullet") return;

        ChangeState(e_BatPatrolState.Dead);
    }

    public void DestroyBat()
    {
        Instantiate(prefab, transform.position, transform.rotation);
        Destroy(bat);
    }

    public void ChangeUltrasound(e_UltrasoundState state)
    {
        currentUltrasound = ultrasoundsList[(int)state];
        currentUltrasound.Start();
    }
}
