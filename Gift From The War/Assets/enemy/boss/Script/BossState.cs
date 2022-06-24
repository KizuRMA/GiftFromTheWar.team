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
    [SerializeField] public Animator animator;
    [SerializeField] public WayPoint wayPoint;
    [SerializeField] public float trackingSpeed = 1.0f;

    private int currentWaypointIndex;

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
        CalcVelocityExample example = player.GetComponent<CalcVelocityExample>();
        float _speed = example.nowSpeed;

        if (_speed >= 100.0f)
        {
            return true;
        }
        return false;
    }
}
