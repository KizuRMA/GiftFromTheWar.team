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
    MagnetCatch,
}

public enum e_UltrasoundState
{
    Large,
    Small,
    Beam,
}

public class BatPatrolState : StatefulObjectBase<BatPatrolState, e_BatPatrolState>
{
    public enum e_CauseOfDead
    {
        None,
        Explosion,
        Wind,
    }

    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject bat;
    [SerializeField] public Animator animator;
    [SerializeField] public WayPoint wayPoint;
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] public GameObject prefab;
    [SerializeField] private Collider attackCollider;
    [SerializeField] private ParticleSystem windBladeParticle;
    [SerializeField] public float trackingSpeed;
    [SerializeField] public float moveWayPointSpeed;

    [System.NonSerialized] public float untilLaunch = 0;
    [System.NonSerialized] public e_CauseOfDead causeOfDead = e_CauseOfDead.None;
    [System.NonSerialized] public Vector3 hypocenter;
    [System.NonSerialized] public BaseUltrasound currentUltrasound;


    protected List<BaseUltrasound> ultrasoundsList = new List<BaseUltrasound>();
    public bool IsNavMeshON => agent.isStopped == false;
    public bool IsPlayerDiscover => IsCurrentState(e_BatPatrolState.Attack) == true || IsCurrentState(e_BatPatrolState.Tracking) == true;
    private float limitHight;
    public float forwardAngle = 20.0f;
    public float height = 0.8f;
    public float hightRatio = 0.4f;




    void Start()
    {
        forwardAngle = 20.0f;
        height = 0.8f;
        limitHight = 0.8f;
        hightRatio = 0.4f;
        stateMachine = new StateMachine<BatPatrolState>();

        ultrasoundsList.Add(GetComponent<LargeUltrasound>());
        ultrasoundsList.Add(GetComponent<SmallUltrasound>());
        ultrasoundsList.Add(GetComponent<UltraSoundBeam>());
        ChangeUltrasound(e_UltrasoundState.Small);

        stateList.Add(new BatMoveWayPointsState(this));
        stateList.Add(new BatDeadState(this));
        stateList.Add(new BatTrackingState(this));
        stateList.Add(new BatAttackState(this));
        stateList.Add(new BatShakeHeadState(this));
        stateList.Add(new PatrolBatMagnetCatchState(this));

        ChangeState(e_BatPatrolState.MoveWayPoints);
    }

    protected override void Update()
    {
        float coolDown = currentUltrasound.coolDown;
        if (currentUltrasound != null && untilLaunch - coolDown >= 0)
        {
            currentUltrasound.Update();
        }
        else
        {
            untilLaunch += Time.deltaTime;
        }

        if (agent.isOnOffMeshLink == false)
        {
            //NavMeshAgent�̉e����Y�����n�ʂ̈ʒu�܂ŉ������Ă��܂��Ă��邽�߁A�����͎����ŊǗ����Ă���
            if (agent.updatePosition == true)
            {
                //�̂�O�ɌX����
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
        //�V��Ɍ������ă��C������s��
        Ray _ray = new Ray(transform.position, Vector3.up);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        //�X�e�[�W�̗������L�^
        float _maxHeight = (_raycastHit.distance - 1.0f);
        float _minHeight = _raycastHit.distance;

        //�X�e�[�W�̏c���̂S���̈ʒu�ɂ���悤�ɂ���
        _minHeight *= hightRatio;

        //�R�E�����̔�s�����ݒ肷��
        if (_minHeight > limitHight)
        {
            _minHeight = limitHight;
        }

        float _targetHeight;

        //�v���C���[�̒n�ʂ܂ł̋������擾
        _ray = new Ray(player.transform.position, Vector3.down);
        _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        playerAbnormalcondition abnormalcondition = player.GetComponent<playerAbnormalcondition>();

        if (_hit == true && abnormalcondition.IsHowling() == true && IsPlayerDiscover == true)
        {
            float _playerHeight = _raycastHit.distance;

            //�R�E�������n�ʂ���ǂꂾ������Ă��邩���ׂ�
            _ray = new Ray(transform.position, Vector3.down);
            _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

            //�R�E�������n�ʂ��痣��Ă��镪�����v���C���[�̍�����Ⴍ����
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

        //���݂̃R�E�������������܂񂾍��W
        Vector3 nowPos = new Vector3(transform.position.x, height, transform.position.z);
        //�{�����Ăق������W
        Vector3 nextPos = new Vector3(transform.position.x, _targetHeight, transform.position.z);

        //�i�r���b�V���̃X�s�[�h��p���ăR�E�����̍����𒲐�����
        nowPos = Vector3.MoveTowards(nowPos, nextPos, 0.8f * Time.deltaTime);

        //���̃t���[���ł͌��݂�Y�����ۑ�����Ȃ����߁A�L�^���Ă����B
        height = nowPos.y;

        transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
    }

    public void Damage(int _damage)
    {
        if (agent.isOnOffMeshLink == true)
        {
            agent.CompleteOffMeshLink();
        }

        causeOfDead = e_CauseOfDead.Wind;
        ChangeState(e_BatPatrolState.Dead);
    }

    public void ExpDamage(int _damage, Vector3 _hypocenter)
    {

        if (agent.isOnOffMeshLink == true)
        {
            agent.CompleteOffMeshLink();
        }

        causeOfDead = e_CauseOfDead.Explosion;
        hypocenter = _hypocenter;
        ChangeState(e_BatPatrolState.Dead);
    }

    public void DestroyBat()
    {
        GameObject game = Instantiate(prefab, transform.position, transform.rotation);

        //�|���ꂽ�����������̎�
        if (causeOfDead == e_CauseOfDead.Explosion)
        {
            DestructionScript dead = game.GetComponent<DestructionScript>();
            dead.ExpBlownAway(hypocenter);
        }
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

    public void SearchPlayerAction() { }

    public void OnAttackStart()
    {
        attackCollider.enabled = true;


        Vector3 _fowardVec = transform.forward;

        // CheckVectorDegAngCode();

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����B
        ParticleSystem newParticle = Instantiate(windBladeParticle);

        newParticle.transform.position = transform.position + (transform.up * 0.3f);
        newParticle.transform.rotation = transform.rotation;

        // �p�[�e�B�N���𔭐�������B
        newParticle.Play();


        // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject���폜����B(�C��)
        // ����������newParticle�����ɂ���ƃR���|�[�l���g�����폜����Ȃ��B
        Destroy(newParticle.gameObject, 5.0f);
    }

    public void OnAttackFinished()
    {
        attackCollider.enabled = false;
    }

    public void MagnetCatch()
    {
        if (agent.isOnOffMeshLink == true)
        {
            agent.CompleteOffMeshLink();
        }

        ChangeState(e_BatPatrolState.MagnetCatch);
    }

    public float CheckVectorDegAngCode(Vector3 _baseVec, Vector3 _targetVec, Vector2 _searchAxis)    //2�{�̃x�N�g������p�x�𕄍����܂ފp�x�����߂�
    {
        _baseVec = new Vector3(_baseVec.x * _searchAxis.x, _baseVec.y * _searchAxis.y, _baseVec.z).normalized;
        _targetVec = new Vector3(_targetVec.x * _searchAxis.x, _targetVec.y * _searchAxis.y, _targetVec.z).normalized;

        float dot = Vector3.Dot(_baseVec, _targetVec);
        Vector3 cross = Vector3.Cross(_baseVec, _targetVec);

        if (cross.y < 0)
        {
            dot *= -1;
        }

        float _degAng = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return _degAng;
    }

    public void WarpPosition(Vector3 _pos)
    {
        agent.Warp(_pos);
        bat.transform.position = _pos;
    }
}
