using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeUltrasound : BaseUltrasound
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] public float startCoolTime = 2;
    [SerializeField] public float CoolTime = 10;
    [SerializeField] public LayerMask layer;

    ParticleSystem nowParticleSystem;
    private float minimumRange;
    private float hitRange;
    float delay;
    bool delayEnd;

    [System.NonSerialized] public bool movePos;

    private void Awake()
    {
        playerObject = GameObject.Find("player").gameObject;
        var main = particle.main;
        duration = main.duration;
        duration += 2.0f;
        velocity = main.startSize.constant / main.startLifetime.constant;

        delay = 3.0f;
        delayEnd = false;

        if (transform.GetComponent<BatController>() == null)
        {
            movePos = true;
        }
        else
        {
            movePos = false;
        }
    }
    public override void Start()
    {
        coolDown = startCoolTime;
        time = 0;
        range = 0.0f;
        maxRange = 0.0f;
        minimumRange = 5.0f;
        hitRange = 0.5f;
        aliveFlg = true;
        delayEnd = false;
    }

    public override void Init()
    {
        coolDown = CoolTime;
        time = 0;
        range = 0.0f;
        maxRange = 0.0f;
        minimumRange = 5.0f;
        hitRange = 0.5f;
        aliveFlg = true;
        delayEnd = false;
    }

    public override void Update()
    {
        if (aliveFlg == false) return;

        if (transform == null)
        {
            StopParticle();
        }

        if (range <= 0.0f)
        {
            //超音波の最大範囲を調べる
            SearchMaxRange();

            // パーティクルシステムのインスタンスを生成する
            if (movePos == true)
            {
                nowParticleSystem = particle;
            }
            else
            {
                nowParticleSystem = Instantiate(particle);

                nowParticleSystem.Stop();

                nowParticleSystem.transform.position = transform.position + (transform.up * 0.3f);

                var main = nowParticleSystem.main;

                main.startSize = maxRange * 2;
                main.startLifetime = maxRange / velocity;
            }

            // パーティクルを発生させる。
            nowParticleSystem.Play();
            StartCoroutine(DelayCoroutine());
            range += 0.001f;
        }

        //遅延が完了してない場合
        if (delayEnd == false) return;

        range += velocity * Time.deltaTime;
        range = Mathf.Min(range, maxRange);

        //超音波の持続時間が終了した場合
        time += Time.deltaTime;
        if (time - duration < 0) return;
        aliveFlg = false;

        if (nowParticleSystem != null)
        {
            StopParticle();
        }
    }

    public override bool CheckHit()
    {
        if (nowParticleSystem == null) return false;

        //当たり判定
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = (playerObject.transform.position) - _firePos;

        //レイ判定
        Ray _ray = new Ray(_firePos,_targetVec);
        RaycastHit _raycastHit;

        //天井に向かってレイ判定
        bool hit = Physics.Raycast(_ray, out _raycastHit, range,layer);

        //レイ判定
        if (hit == false || _raycastHit.collider.tag != "Player") return false;

        //超音波の長さに調整する
        _targetVec = _targetVec.normalized * (range - 0.7f);

        //超音波本体の座標を算出
        Vector3 _pos = _firePos + _targetVec;

        //超音波本体とプレイヤーの距離を調べる
        _targetVec = playerObject.transform.position - _pos;

        float _distance = _targetVec.magnitude;

        if (_distance <= hitRange)
        {
            StopParticle();
            aliveFlg = false;
            return true;
        }
        return false;
    }

    private void SearchMaxRange()
    {
        //レイヤーマスクを"cave"（洞窟）にしてレイ判定を行う
        int layerMask = 1 << 9;
        Ray _ray = new Ray(transform.position, transform.up);
        RaycastHit _raycastHit;

        //天井に向かってレイ判定
        bool hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, layerMask);

        if (hit == true)
        {
            //超音波の範囲が最低値よりも小さい場合
            maxRange = _raycastHit.distance;
            if (maxRange <= minimumRange)
            {
                maxRange = minimumRange;
            }
        }
        else
        {
            maxRange = minimumRange;
        }
    }

    public override void DrawLine()
    {
        //当たり判定
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerObject.transform.position - _firePos;

        //デバッグ用の線を描画
        var lineRenderer = gameObject.GetComponent<LineRenderer>();

        var positions = new Vector3[]
        {
            _firePos,
            _firePos + ((playerObject.transform.position - _firePos).normalized * range),
        };

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.SetPositions(positions);
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(delay);
        delayEnd = true;
    }

    public void StopParticle()
    {
        if (nowParticleSystem == null) return;

        nowParticleSystem.Stop();

        ParticleSystem particle = nowParticleSystem.transform.GetChild(1).GetComponent<ParticleSystem>();
        particle.Clear();

        nowParticleSystem = null;
    }

    public override void Exit()
    {
        StopParticle();
    }

}
