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
    public float hight { get; set; }
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
        hight = defaltHight;
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

        //�X�e�[�W�̗������L�^
        float _maxHight = _raycastHit.distance;
        float _minHight = _maxHight;

        //�X�e�[�W�̏c���̂S���̈ʒu�ɂ���悤�ɂ���
        _minHight *= 0.4f;
        //�R�E�����̔�s�����ݒ肷��
        if (_minHight > 0.8f)
        {
            _minHight = 0.8f;
        }

        float _targetHight = _maxHight;

        //�v���C���[�̒n�ʂ܂ł̋������擾
        _ray = new Ray(playerCC.transform.position, Vector3.down);
        _hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, raycastLayerMask);

        if (_hit == true)
        {
            _targetHight = Mathf.Min(Mathf.Max(_raycastHit.distance,_minHight),_maxHight);
        }
        else
        {
            _targetHight = _minHight;
        }

        //���݂̃R�E�������������܂񂾍��W
        Vector3 nowPos = new Vector3(transform.position.x, hight, transform.position.z);
        //�{�����Ăق������W
        Vector3 nextPos = new Vector3(transform.position.x, _targetHight, transform.position.z);

        //�i�r���b�V���̃X�s�[�h��p���ăR�E�����̍����𒲐�����
        nowPos = Vector3.MoveTowards(nowPos, nextPos, 0.8f * Time.deltaTime);

        //���̃t���[���ł͌��݂�Y�����ۑ�����Ȃ����߁A�L�^���Ă����B
        hight = nowPos.y;

        transform.position = new Vector3(transform.position.x, transform.position.y + hight, transform.position.z);
    }

    public void SimpleAdjustHeight()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + hight, transform.position.z);
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
