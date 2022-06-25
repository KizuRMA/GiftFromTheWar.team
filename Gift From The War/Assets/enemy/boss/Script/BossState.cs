using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum e_BossState
{
    Tracking,
    BomAttack,
}

public class BossState : StatefulObjectBase<BossState, e_BossState>
{
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject myGameObject;
    [SerializeField] public GameObject grenadePrefab;
    [SerializeField] public Transform createPosition;
    [SerializeField] public Transform CatchPosition;
    [SerializeField] public Animator animator;
    [SerializeField] public WayPoint wayPoint;
    [SerializeField] public float trackingSpeed = 1.0f;

    private int currentWaypointIndex;
    private GameObject generatedGrenade;

    void Start()
    {
        stateList.Add(new BossTrackingState(this));
        stateList.Add(new BossBomAttack(this));

        ChangeState(e_BossState.Tracking);

        stateMachine = new StateMachine<BossState>();

        //=============
        //変数の初期化
        //=============
        currentWaypointIndex = 0;
        agent.speed = trackingSpeed;

        //追いかけるターゲットを設定
        agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;
    }

    protected override void Update()
    {
        base.Update();

        //目的地の更新は常時処理する
        DestinationUpdate();

        //攻撃するか確認
        if (IsAttack() == true)
        {
            ChangeState(e_BossState.BomAttack);
        }
    }

    private void DestinationUpdate()    //目的値を更新する処理
    {
        Vector3 _nowPos = new Vector3(transform.position.x, agent.destination.y, transform.position.z);
        float targetDis = Vector3.Distance(_nowPos, agent.destination);

        // 目的地点までの距離(remainingDistance)が目的地の手前までの距離(stoppingDistance)以下になったら
        if (targetDis <= 0.1f)
        {
            // 目的地の番号を１更新（右辺を剰余演算子にすることで目的地をループさせれる）
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoint.wayPoints.Count;
            // 目的地を次の場所に設定
            agent.destination = wayPoint.wayPoints[currentWaypointIndex].position;
        }
    }

    public bool IsAttack()
    {
        if (IsCurrentState(e_BossState.BomAttack) == true) return false;

        CalcVelocityExample example = player.GetComponent<CalcVelocityExample>();
        float _speed = example.nowSpeed;

        if (_speed >= 3.0f)
        {
            return true;
        }
        return false;
    }

    public void Create()
    {
        if (grenadePrefab == null) return;

        generatedGrenade = Instantiate(grenadePrefab);
        generatedGrenade.transform.parent = transform;
        generatedGrenade.transform.position = createPosition.position;
    }

    public void Catch()
    {
        generatedGrenade.transform.parent = CatchPosition.transform.parent;
        Rigidbody rd = generatedGrenade.GetComponent<Rigidbody>();
        rd.useGravity = false;
        rd.isKinematic = true;
        generatedGrenade.transform.localPosition = CatchPosition.transform.localPosition;
    }

    public void Throw()
    {
        generatedGrenade.transform.parent = null;

        Rigidbody rd = generatedGrenade.GetComponent<Rigidbody>();
        rd.useGravity = true;
        rd.isKinematic = false;

        CapsuleCollider collider = generatedGrenade.GetComponent<CapsuleCollider>();
        collider.isTrigger = false;

        // 標的の座標
        Vector3 targetPosition = player.transform.position;

        // 射出角度
        float angle = 1.0f;

        // 射出速度を算出
        Vector3 velocity = CalculateVelocity(generatedGrenade.transform.position, targetPosition, angle);

        // 射出
        rd.AddForce(velocity * rd.mass, ForceMode.Impulse);

        var random = new System.Random();
        var min = -3;
        var max = 3;

        var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
        rd.AddTorque(vect, ForceMode.Impulse);
    }

    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}
