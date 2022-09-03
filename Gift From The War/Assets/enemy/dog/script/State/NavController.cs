using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavController : MonoBehaviour
{
    [Header("移動")]
    [TooltipAttribute("最大速度"), SerializeField]
    public float maxSpeed = 2f;
    [TooltipAttribute("加速度"), SerializeField]
    public float acceleration = 1;
    [TooltipAttribute("通常の旋回速度"), SerializeField]
    public float angularSpeed = 200f;
    [TooltipAttribute("ターンする時の角度差"), SerializeField]
    public float turnAngle = 45f;
    [TooltipAttribute("ターンを終了する時の角度差"), SerializeField]
    public float turnEndAngle = 45f;
    [TooltipAttribute("ターン時の旋回速度"), SerializeField]
    public float turnAngularSpeed = 1000f;
    [TooltipAttribute("スピードを落とす距離。目的地がこの距離以内になったら、旋回角度に応じた減速をする"), SerializeField]
    public float speedDownDistance = 0.5f;
    [TooltipAttribute("停止距離。この距離以下は移動しない"), SerializeField]
    public float stopDistance = 0.01f;

    [Header("アニメーション")]
    [TooltipAttribute("移動速度とアニメーション速度の変換率"), SerializeField]
    public float Speed2Anim = 1f;
    [TooltipAttribute("アニメを停止とみなす速度"), SerializeField]
    public float stopSpeed = 0.01f;
    [TooltipAttribute("アニメの平均化係数"), SerializeField]
    public float averageSpeed = 0.5f;

    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public CharacterController chrController;
    NavMeshPath path;

    float lastSpeed;
    float walkSpeed;

    bool turnFlg;

    private void Awake()
    {
        walkSpeed = 0f;
        turnFlg = false;
    }

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

    public bool IsReached
    {
        get
        {
            return Vector3.Distance(transform.position, LastCorner) <= stopDistance;
        }
    }

    public void Move(NavMeshPath _path,float _addMoveSpeed)
    {
        walkSpeed += (acceleration / 10) * Time.deltaTime;
        walkSpeed = Mathf.Min(maxSpeed + _addMoveSpeed, walkSpeed);

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

        // 移動方向と速度を算出
        _move = _target - transform.position;
        _move.y = 0f;
        float rot = angularSpeed * Time.deltaTime;

        if (!IsReached)
        {
            float angle = Vector3.SignedAngle(transform.forward, _move, Vector3.up);

            //角度がturnAngleを越えていたら速度0
            if (Mathf.Abs(angle) > turnAngle || turnFlg == true)
            {
                // 最高速度を越えているのでターンのみ
                float rotmax = turnAngularSpeed * Time.deltaTime;
                rot = Mathf.Min(Mathf.Abs(angle), rotmax);
                transform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
                _move = Vector3.zero;
                spd = 0f;
                turnFlg = true;
                if (angle <= turnEndAngle)
                {
                    turnFlg = false;
                }
            }
            else
            {
                // ターンはしない

                // ゴール距離がスピードダウンより近い場合、角度の違いの分、前進速度を比例減速する
                if (DistanceXZ(LastCorner, transform.position) < speedDownDistance)
                {
                    spd *= (1f - (Mathf.Abs(angle) / turnAngle));
                }

                // 1回分の移動をキャンセルする場合、回転速度は制限しない
                if (_move.magnitude < spd)
                {
                    spd = _move.magnitude;
                    rot = angle;
                    transform.Rotate(0f, angle, 0f);
                }
                else
                {
                    // 移動しながらターン
                    rot = Mathf.Min(Mathf.Abs(angle), rot);
                    transform.Rotate(0f, rot * Mathf.Sign(angle), 0f);
                }

                // キャラクターの前方に移動
                _move = transform.forward * spd;
            }
        }
        else
        {
            _move = LastCorner - transform.position;
            _move.y = 0f;
        }

        chrController.Move(_move);
    }

    public void Reset()
    {
        walkSpeed = 0f;
    }
}
