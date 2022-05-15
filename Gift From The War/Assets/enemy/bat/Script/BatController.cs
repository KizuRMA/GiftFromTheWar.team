using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatController : MonoBehaviour
{
    BaseState state;
    public float hight { get; set; }
    public float forwardAngle { get; set; }
    private NavMeshAgent agent;

    private float life;
    [SerializeField] public float defaltHight;
    [SerializeField] public float defaltForwardAngle;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        life = 1.0f;
        hight = defaltHight;
        forwardAngle = defaltForwardAngle;
        //�X�e�[�g��؂�ւ���
        ChangeState(GetComponent<batMove>());
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
    public void AdjustHeight()
    {
        Ray _ray = new Ray(transform.position, Vector3.up);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit);

        //�X�e�[�W�̗������L�^
        float _hight = _raycastHit.distance;

        //�X�e�[�W�̏c���̂S���̈ʒu�ɂ���悤�ɂ���
        _hight *= 0.4f;
        //�R�E�����̔�s�����ݒ肷��
        if (_hight > 0.8f)
        {
            _hight = 0.8f;
        }

        //���݂̃R�E�������������܂񂾍��W
        Vector3 nowPos = new Vector3(transform.position.x, hight, transform.position.z);
        //�{�����Ăق������W
        Vector3 nextPos = new Vector3(transform.position.x, _hight, transform.position.z);

        //�i�r���b�V���̃X�s�[�h��p���ăR�E�����̍����𒲐�����
        nowPos = Vector3.MoveTowards(nowPos, nextPos, 0.001f);

        //���̃t���[���ł͌��݂�Y�����ۑ�����Ȃ����߁A�L�^���Ă����B
        hight = nowPos.y;

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
