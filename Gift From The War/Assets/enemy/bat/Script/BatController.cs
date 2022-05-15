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
        //ステートを切り替える
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
        //実体を削除
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
    public void AdjustHeight()
    {
        Ray _ray = new Ray(transform.position, Vector3.up);
        RaycastHit _raycastHit;
        bool _hit = Physics.Raycast(_ray, out _raycastHit);

        //ステージの立幅を記録
        float _hight = _raycastHit.distance;

        //ステージの縦幅の４割の位置にいるようにする
        _hight *= 0.4f;
        //コウモリの飛行上限を設定する
        if (_hight > 0.8f)
        {
            _hight = 0.8f;
        }

        //現在のコウモリを高さを含んだ座標
        Vector3 nowPos = new Vector3(transform.position.x, hight, transform.position.z);
        //本来いてほしい座標
        Vector3 nextPos = new Vector3(transform.position.x, _hight, transform.position.z);

        //ナビメッシュのスピードを用いてコウモリの高さを調整する
        nowPos = Vector3.MoveTowards(nowPos, nextPos, 0.001f);

        //次のフレームでは現在のY軸が保存されないため、記録しておく。
        hight = nowPos.y;

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
