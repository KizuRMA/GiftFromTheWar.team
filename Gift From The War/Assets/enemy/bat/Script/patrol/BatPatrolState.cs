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
    public float height;
    public float hightRatio;

    [SerializeField] public float trackingSpeed;
    [SerializeField] public float moveWayPointSpeed;

    void Start()
    {
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
        //NavMeshAgent???e????Y?????n???????u?????????????????????????????A????????????????????????
        if (agent.updatePosition == true)
        {
            //?????O???X????
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
        //?V???????????????C???????s??
        Ray _ray = new Ray(transform.position, Vector3.up);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        //???????\??????(?V???????????? - ?R?E??????????)
        float _climbableheight = _raycastHit.distance - 1.5f;

        //?X?e?[?W?????????L?^
        float _maxHeight = _raycastHit.distance;
        float _minHeight = _maxHeight;

        //?X?e?[?W???c?????S???????u????????????????
        _minHeight *= hightRatio;

        //?R?E?????????s??????????????
        if (_minHeight > limitHight)
        {
            _minHeight = limitHight;
        }

        float _targetHeight = _maxHeight;

        //?v???C???[???n??????????????????
        _ray = new Ray(player.transform.position, Vector3.down);
        _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        if (_hit == true && IsPlayerDiscover == true)
        {
            _targetHeight = Mathf.Min(Mathf.Max(_raycastHit.distance, _minHeight), _maxHeight);
        }
        else
        {
            _targetHeight = _minHeight;
        }

        //??????????????????????
        if (height - _targetHeight <= 0)
        {
            //???????\????????????????
            if (_climbableheight <= 0)
            {
                return;
            }
        }

        //???????R?E?????????????????????W
        Vector3 nowPos = new Vector3(transform.position.x, height, transform.position.z);
        //?{???????????????W
        Vector3 nextPos = new Vector3(transform.position.x, _targetHeight, transform.position.z);

        //?i?r???b?V?????X?s?[?h???p?????R?E????????????????????
        nowPos = Vector3.MoveTowards(nowPos, nextPos, 0.8f * Time.deltaTime);

        //?????t???[????????????Y?????????????????????A?L?^?????????B
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

        // ?p?[?e?B?N???V?X?e?????C???X?^???X???????????B
        ParticleSystem newParticle = Instantiate(windBladeParticle);

        newParticle.transform.position = transform.position + (transform.up * 0.3f);
        newParticle.transform.rotation = transform.rotation;

        // ?p?[?e?B?N???????????????B
        newParticle.Play();


        // ?C???X?^???X???????p?[?e?B?N???V?X?e????GameObject???????????B(?C??)
        // ????????????newParticle?????????????R???|?[?l???g?????????????????B
        Destroy(newParticle.gameObject, 5.0f);
    }

    public void OnAttackFinished()
    {
        attackCollider.enabled = false;
    }

    public void SearchPlayerAction()
    {

    }

    public float CheckVectorDegAngCode(Vector3 _baseVec,Vector3 _targetVec,Vector2 _searchAxis)    //2?{???x?N?g???????p?x?????????????p?x????????
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
