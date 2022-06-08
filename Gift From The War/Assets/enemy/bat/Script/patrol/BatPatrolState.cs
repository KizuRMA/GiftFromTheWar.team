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
    ShakeHead,
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
    [SerializeField] private Collider attackCollider;
    [SerializeField] private ParticleSystem windBladeParticle;

    public BaseUltrasound currentUltrasound;
    protected List<BaseUltrasound> ultrasoundsList = new List<BaseUltrasound>();

    public bool IsNavMeshON => agent.isStopped == false;
    public bool IsPlayerDiscover => IsCurrentState(e_BatPatrolState.Attack) == true || IsCurrentState(e_BatPatrolState.Tracking) == true;
    private float limitHight;
    public float hight;
    public float hightRatio;

    [SerializeField] public float trackingSpeed;
    [SerializeField] public float moveWayPointSpeed;

    void Start()
    {
        hight = 0.8f;
        limitHight = 0.8f;
        hightRatio = 0.4f;
        stateMachine = new StateMachine<BatPatrolState>();


        stateList.Add(new BatMoveWayPointsState(this));
        stateList.Add(new BatDeadState(this));
        stateList.Add(new BatTrackingState(this));
        stateList.Add(new BatAttackState(this));
        stateList.Add(new BatShakeHeadState(this));

        ChangeState(e_BatPatrolState.MoveWayPoints);

        ultrasoundsList.Add(GetComponent<LargeUltrasound>());
        ultrasoundsList.Add(GetComponent<SmallUltrasound>());
        ultrasoundsList.Add(GetComponent<UltraSoundBeam>());

        ChangeUltrasound(e_UltrasoundState.Small);
    }

    protected override void Update()
    {
        //NavMeshAgentの影響でY軸が地面の位置まで下がってしまっているため、高さは自分で管理している
        if (agent.updatePosition == true)
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

        if (_hit == true && IsPlayerDiscover == true)
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

    public void Damage(int damage)
    {
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

    public void OnHitAttack(Collider _collider)
    {
        var target = _collider.GetComponent<playerAbnormalcondition>();
        if (null == target) return;

        target.Damage(1.0f);
    }

    public void OnAttackStart()
    {
        attackCollider.enabled = true;


        Vector3 _fowardVec = transform.forward;

       // CheckVectorDegAngCode();

        // パーティクルシステムのインスタンスを生成する。
        ParticleSystem newParticle = Instantiate(windBladeParticle);

        newParticle.transform.position = transform.position;
        newParticle.transform.rotation = transform.rotation;

        // パーティクルを発生させる。
        newParticle.Play();


        // インスタンス化したパーティクルシステムのGameObjectを削除する。(任意)
        // ※第一引数をnewParticleだけにするとコンポーネントしか削除されない。
        Destroy(newParticle.gameObject, 5.0f);
    }

    public void OnAttackFinished()
    {
        attackCollider.enabled = false;
    }

    public void SearchPlayerAction()
    {

    }

    public float CheckVectorDegAngCode(Vector3 _baseVec,Vector3 _targetVec,Vector2 _searchAxis)    //2本のベクトルから角度を符号を含む角度を求める
    {
        _baseVec = new Vector3(_baseVec.x * _searchAxis.x, _baseVec.y * _searchAxis.y, _baseVec.z).normalized;
        _targetVec = new Vector3(_targetVec.x * _searchAxis.x, _targetVec.y * _searchAxis.y, _targetVec.z).normalized;

        float dot = Vector3.Dot(_baseVec,_targetVec);
        Vector3 cross = Vector3.Cross(_baseVec,_targetVec);

        if (cross.y < 0)
        {
            dot *= -1;
        }

        float _degAng = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return _degAng;
    }
}
