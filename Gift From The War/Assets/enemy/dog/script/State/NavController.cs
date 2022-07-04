using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavController : MonoBehaviour
{
    [Header("�ړ�")]
    [TooltipAttribute("�������x"), SerializeField]
    public float walkSpeed = 2f;
    [TooltipAttribute("�ʏ�̐��񑬓x"), SerializeField]
    public float angularSpeed = 200f;
    [TooltipAttribute("�^�[�����鎞�̊p�x��"), SerializeField]
    public float turnAngle = 45f;
    [TooltipAttribute("�^�[�����̐��񑬓x"), SerializeField]
    public float turnAngularSpeed = 1000f;
    [TooltipAttribute("�X�s�[�h�𗎂Ƃ������B�ړI�n�����̋����ȓ��ɂȂ�����A����p�x�ɉ���������������"), SerializeField]
    public float speedDownDistance = 0.5f;
    [TooltipAttribute("��~�����B���̋����ȉ��͈ړ����Ȃ�"), SerializeField]
    public float stopDistance = 0.01f;

    [Header("�A�j���[�V����")]
    [TooltipAttribute("�ړ����x�ƃA�j���[�V�������x�̕ϊ���"), SerializeField]
    public float Speed2Anim = 1f;
    [TooltipAttribute("�A�j�����~�Ƃ݂Ȃ����x"), SerializeField]
    public float stopSpeed = 0.01f;
    [TooltipAttribute("�A�j���̕��ω��W��"), SerializeField]
    public float averageSpeed = 0.5f;

    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public CharacterController chrController;
    NavMeshPath path;

    public float DistanceXZ(Vector3 src, Vector3 dst)
    {
        src.y = dst.y;
        return Vector3.Distance(src, dst);
    }

    public Vector3 LastCorner
    {
        get
        {
            if (path.corners.Length == 0)
            {
                return agent.destination;
            }
            return path.corners[path.corners.Length - 1];
        }
    }

    public void Move(NavMeshPath _path)
    {
        path = _path;

        Vector3 _move = chrController.velocity;
        float spd = 0.0f;

        Vector3 _target = LastCorner;
        spd = walkSpeed * Time.deltaTime;
        for (int i = 0; i < path.corners.Length; i++)
        {
            _target = path.corners[i];
            if (DistanceXZ(_target, transform.position) >= spd)
            {
                break;
            }
        }

        // �ړ������Ƒ��x���Z�o
        _move = _target - transform.position;
        _move.y = 0f;
        float rot = angularSpeed * Time.deltaTime;

        float angle = Vector3.SignedAngle(transform.forward, _move, Vector3.up);

        Vector3 _localAngle;
        _localAngle = transform.localEulerAngles;
        _localAngle.y += angle;
        transform.localEulerAngles = _localAngle;
        //transform.Rotate(0f, rot * Mathf.Sign(angle), 0f);

        // �p�x��turnAngle���z���Ă����瑬�x0
        //if (Mathf.Abs(angle) > turnAngle)
        //{
        //    // �ō����x���z���Ă���̂Ń^�[���̂�
        //    float rotmax = turnAngularSpeed * Time.deltaTime;
        //    rot = Mathf.Min(Mathf.Abs(angle), rotmax);
        //    transform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
        //    _move = Vector3.zero;
        //    spd = 0f;
        //}
        //else
        //{
        //    // �^�[���͂��Ȃ�

        //    // �S�[���������X�s�[�h�_�E�����߂��ꍇ�A�p�x�̈Ⴂ�̕��A�O�i���x���ጸ������
        //    if (DistanceXZ(LastCorner, transform.position) < speedDownDistance)
        //    {
        //        spd *= (1f - (Mathf.Abs(angle) / turnAngle));
        //    }

        //    // 1�񕪂̈ړ����L�����Z������ꍇ�A��]���x�͐������Ȃ�
        //    if (_move.magnitude < spd)
        //    {
        //        spd = _move.magnitude;
        //        rot = angle;
        //        transform.Rotate(0f, angle, 0f);
        //    }
        //    else
        //    {
        //        // �ړ����Ȃ���^�[��
        //        rot = Mathf.Min(Mathf.Abs(angle), rot);
        //        transform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
        //    }

        //    // �L�����N�^�[�̑O���Ɉړ�
        //    _move = transform.forward * spd;
        //}

        _move = transform.forward * (Time.deltaTime * walkSpeed);
        chrController.Move(_move);

    }
}
