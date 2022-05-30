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
        //ステートを切り替える
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
        //実体を削除
        if (state != null)
        {
            state.Init();
        }
        state = null;
        //新しい実体のアドレスを入れる
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
        //２回以上DeadStateにならないようにする
        if (life < 0) return;

        life -= _damage;
        if (life > 0) return;

        ChangeState(GetComponent<DeadState>());
    }

    //高さを動的に変える処理(ナビメッシュが高さの情報を処理しないため)
    //※transform.positionにhightが足されていない場合に限る
    public void AdjustHeight()
    {
        //ナビメッシュの影響でY軸の値が地面の座標になっている
        Ray _ray = new Ray(transform.position, Vector3.up);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit,1000.0f, raycastLayerMask);

        //ステージの立幅を記録
        float _maxHight = _raycastHit.distance;
        float _minHight = _maxHight;

        //ステージの縦幅の４割の位置にいるようにする
        _minHight *= 0.4f;
        //コウモリの飛行上限を設定する
        if (_minHight > 0.8f)
        {
            _minHight = 0.8f;
        }

        float _targetHight = _maxHight;

        //プレイヤーの地面までの距離を取得
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

        //現在のコウモリを高さを含んだ座標
        Vector3 nowPos = new Vector3(transform.position.x, hight, transform.position.z);
        //本来いてほしい座標
        Vector3 nextPos = new Vector3(transform.position.x, _targetHight, transform.position.z);

        //ナビメッシュのスピードを用いてコウモリの高さを調整する
        nowPos = Vector3.MoveTowards(nowPos, nextPos, 0.8f * Time.deltaTime);

        //次のフレームでは現在のY軸が保存されないため、記録しておく。
        hight = nowPos.y;

        transform.position = new Vector3(transform.position.x, transform.position.y + hight, transform.position.z);
    }

    public void SimpleAdjustHeight()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + hight, transform.position.z);
    }

    //ナビメッシュの処理を再開
    public void OffNavMesh()
    {
        agent.isStopped = true;
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.updatePosition = false;
    }

    //ナビメッシュの処理を停止
    public void OnNavMesh()
    {
        agent.isStopped = false;
        agent.updateUpAxis = true;
        agent.updateRotation = true;
        agent.updatePosition = true;
    }
}
