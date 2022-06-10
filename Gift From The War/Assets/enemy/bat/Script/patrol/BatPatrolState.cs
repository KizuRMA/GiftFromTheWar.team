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
    public float forwardAngle;
    public float height;
    public float hightRatio;

    [SerializeField] public float trackingSpeed;
    [SerializeField] public float moveWayPointSpeed;

    void Start()
    {
        forwardAngle = 20.0f;
        height = 0.8f;
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
       

        if (currentUltrasound != null)
        {
            currentUltrasound.Update();
        }

        if (agent.isOnOffMeshLink == false)
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

            base.Update();
        }
       
    }

    public void AdjustHeight()
    {
        //天井に向かってレイ判定を行う
        Ray _ray = new Ray(transform.position, Vector3.up);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        //ステージの立幅を記録
        float _maxHeight = (_raycastHit.distance - 1.0f);
        float _minHeight = _raycastHit.distance;

        //ステージの縦幅の４割の位置にいるようにする
        _minHeight *= hightRatio;

        //コウモリの飛行上限を設定する
        if (_minHeight > limitHight)
        {
            _minHeight = limitHight;
        }

        float _targetHeight;

        //プレイヤーの地面までの距離を取得
        _ray = new Ray(player.transform.position, Vector3.down);
        _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        playerAbnormalcondition abnormalcondition = player.GetComponent<playerAbnormalcondition>();

        if (_hit == true && abnormalcondition.IsHowling() == true)
        {
            float _playerHeight = _raycastHit.distance;

            //コウモリが地面からどれだけ離れているか調べる
            _ray = new Ray(transform.position, Vector3.down);
            _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

            //コウモリが地面から離れている分だけプレイヤーの高さを低くする
            _playerHeight -= _raycastHit.distance;
            if (_playerHeight < 0)
            {
                _playerHeight = 0;
            }

            _targetHeight = Mathf.Min(Mathf.Max(_playerHeight, _minHeight), _maxHeight);
        }
        else
        {
            _targetHeight = _minHeight;
        }

        //現在のコウモリを高さを含んだ座標
        Vector3 nowPos = new Vector3(transform.position.x, height, transform.position.z);
        //本来いてほしい座標
        Vector3 nextPos = new Vector3(transform.position.x, _targetHeight, transform.position.z);

        //ナビメッシュのスピードを用いてコウモリの高さを調整する
        nowPos = Vector3.MoveTowards(nowPos, nextPos, 0.8f * Time.deltaTime);

        //次のフレームでは現在のY軸が保存されないため、記録しておく。
        height = nowPos.y;

        transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
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
        if (ultrasoundsList.Count <= 0) return;
        if ((int)state >= System.Enum.GetValues(typeof(e_UltrasoundState)).Length - 1) return;

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

        newParticle.transform.position = transform.position + (transform.up * 0.3f);
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
