using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatController : MonoBehaviour
{
    public enum e_State
    {
        move,
        wingFold,
        attack,
        magnetCatch,
        dead,
    }

    public enum e_CauseOfDead
    {
        None,
        Explosion,
        Wind,
    }

    [SerializeField] public float defaltHight;
    [SerializeField] public float defaltForwardAngle;
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public CharacterController playerCC;
    [SerializeField] public Animator animator;

    [System.NonSerialized] public e_CauseOfDead causeOfDead = e_CauseOfDead.None;
    [System.NonSerialized] public Vector3 hypocenter;
    [System.NonSerialized] public BaseState state;

    public float height { get; set; }
    public float forwardAngle { get; set; }
    private float life;
    public bool IsAttackable => (int)e_State.move == state.CurrentState && life >= 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerCC = GameObject.Find("player").GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        life = 1.0f;
        height = defaltHight;
        forwardAngle = defaltForwardAngle;
        //�X�e�[�g��؂�ւ���
        ChangeState(GetComponent<batMove>());
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isOnOffMeshLink == true)
        {
           
        }
        else
        {
            state.Update();
        }
    }

    public void ChangeState(BaseState _state)
    {
        //���̂��폜
        if (state != null)
        {
            state.Init();
            state.Exit();
        }
        state = null;
        //�V�������̂̃A�h���X������
        state = _state;
        state.Start();
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    public void Damage(int _damage)
    {
        if (agent.isOnOffMeshLink == true)
        {
            agent.CompleteOffMeshLink();
        }

        //�Q��ȏ�DeadState�ɂȂ�Ȃ��悤�ɂ���
        if (life < 0) return;

        life -= _damage;
        if (life > 0) return;

        causeOfDead = e_CauseOfDead.Wind;
        ChangeState(GetComponent<DeadState>());
    }

    public void ExpDamage(int _damage,Vector3 _hypocenter)
    {
        //�Q��ȏ�DeadState�ɂȂ�Ȃ��悤�ɂ���
        if (life < 0) return;

        life -= _damage;
        if (life > 0) return;

        hypocenter = _hypocenter;
        causeOfDead = e_CauseOfDead.Explosion;
        ChangeState(GetComponent<DeadState>());
    }

    //�����𓮓I�ɕς��鏈��(�i�r���b�V���������̏����������Ȃ�����)
    //��transform.position��hight��������Ă��Ȃ��ꍇ�Ɍ���
    public void AdjustHeight()
    {
        //�i�r���b�V���̉e����Y���̒l���n�ʂ̍��W�ɂȂ��Ă���
        Ray _ray = new Ray(transform.position, Vector3.up);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        //�X�e�[�W�̗������L�^
        float _maxHeight = (_raycastHit.distance - 1.0f);
        float _minHeight = _raycastHit.distance;

        //�X�e�[�W�̏c���̂S���̈ʒu�ɂ���悤�ɂ���
        _minHeight *= 0.4f;

        //�R�E�����̔�s�����ݒ肷��
        if (_minHeight > 0.8f)
        {
            _minHeight = 0.8f;
        }

        float _targetHeight;

        //�v���C���[�̒n�ʂ܂ł̋������擾
        _ray = new Ray(playerCC.transform.position, Vector3.down);
        _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        playerAbnormalcondition abnormalcondition = playerCC.GetComponent<playerAbnormalcondition>();

        if (_hit == true && abnormalcondition.IsHowling() == true)
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

    public void SimpleAdjustHeight()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
    }

    //�i�r���b�V���̏������ĊJ
    public void OffNavMesh()
    {
        agent.isStopped = true;
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.updatePosition = false;
    }

    //�i�r���b�V���̏������~
    public void OnNavMesh()
    {
        agent.isStopped = false;
        agent.updateUpAxis = true;
        agent.updateRotation = true;
        agent.updatePosition = true;
    }

    public void MagnetCatch()
    {
        if (agent.isOnOffMeshLink == true)
        {
            agent.CompleteOffMeshLink();
        }

        ChangeState(GetComponent<BatMagnetCatchState>());
    }

    public void WarpPosition(Vector3 _pos)
    {
        agent.Warp(_pos);
        transform.position = _pos;
    }
}
