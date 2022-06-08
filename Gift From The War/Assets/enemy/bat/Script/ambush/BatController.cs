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
    }

    BaseState state;
    public float height { get; set; }
    public float forwardAngle { get; set; }
    private NavMeshAgent agent;

    private float life;
    [SerializeField] public float defaltHight;
    [SerializeField] public float defaltForwardAngle;
    [SerializeField] private LayerMask raycastLayerMask;

    private CharacterController playerCC;

    public bool IsAttackable => (int)e_State.move == state.CurrentState;

    // Start is called before the first frame update
    void Start()
    {
        playerCC = GameObject.Find("player").GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        life = 1.0f;
        height = defaltHight;
        forwardAngle = defaltForwardAngle;
        //�X�e�[�g��؂�ւ���
        ChangeState(GetComponent<WingFoldState>());
    }

    // Update is called once per frame
    void Update()
    {
        state.Update();
       

        if (Input.GetKey(KeyCode.T))
        {
            Damage(1);
        }
    }

    public void ChangeState(BaseState _state)
    {
        //���̂��폜
        if (state != null)
        {
            state.Init();
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
        //�Q��ȏ�DeadState�ɂȂ�Ȃ��悤�ɂ���
        if (life < 0) return;

        life -= _damage;
        if (life > 0) return;

        ChangeState(GetComponent<DeadState>());
    }

    //�����𓮓I�ɕς��鏈��(�i�r���b�V���������̏����������Ȃ�����)
    //��transform.position��hight��������Ă��Ȃ��ꍇ�Ɍ���
    public void AdjustHeight()
    {
        //�i�r���b�V���̉e����Y���̒l���n�ʂ̍��W�ɂȂ��Ă���
        Ray _ray = new Ray(transform.position, Vector3.up);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit,1000.0f, raycastLayerMask);

        float nowBatHeight = transform.position.y;
        
        //�X�e�[�W�̗������L�^
        float _maxHeight = (_raycastHit.distance - 1.0f) + transform.position.y;
        float _minHeight = _raycastHit.distance;

        //�X�e�[�W�̏c���̂S���̈ʒu�ɂ���悤�ɂ���
        _minHeight *= 0.4f;

        //�R�E�����̔�s�����ݒ肷��
        if (_minHeight > 0.8f)
        {
            _minHeight = 0.8f;
        }

        _minHeight += transform.position.y;

        float _targetHeight = playerCC.transform.position.y;

        playerAbnormalcondition abnormalcondition = playerCC.GetComponent<playerAbnormalcondition>();

        if (_hit == true && abnormalcondition.IsHowling() == true)
        {
            _targetHeight = Mathf.Min(Mathf.Max(_raycastHit.distance,_minHeight),_maxHeight);
        }
        else
        {
            _targetHeight = _minHeight;
        }

        //���݂̃R�E�������������܂񂾍��W
        Vector3 nowPos = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        //�{�����Ăق������W
        Vector3 nextPos = new Vector3(transform.position.x, _targetHeight, transform.position.z);

        //�i�r���b�V���̃X�s�[�h��p���ăR�E�����̍����𒲐�����
        nowPos = Vector3.MoveTowards(nowPos, nextPos, 0.8f * Time.deltaTime);

        //���̃t���[���ł͌��݂�Y�����ۑ�����Ȃ����߁A�L�^���Ă����B
        height = nowPos.y - nowBatHeight;

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
}
