using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeUltrasound : BaseUltrasound
{
    [SerializeField] private ParticleSystem particle;
    ParticleSystem nowParticleSystem;
    private float minimumRange;
    private float hitRange;

    [System.NonSerialized] public bool pos;

    private void Awake()
    {
        playerObject = GameObject.Find("player").gameObject;
        var main = particle.main;
        duration = main.duration;
        duration += 2.0f;
        velocity = main.startSize.constant / main.startLifetime.constant;
    }
    public override void Start()
    {
        coolDown = 0;
        time = 0;
        range = 0.0f;
        maxRange = 0.0f;
        minimumRange = 5.0f;
        hitRange = 0.5f;
        aliveFlg = true;
    }

    public override void Init()
    {
        coolDown = 10;
        time = 0;
        range = 0.0f;
        maxRange = 0.0f;
        minimumRange = 5.0f;
        hitRange = 0.5f;
        aliveFlg = true;
    }

    public override void Update()
    {
        if (range <= 0.0f)
        {
            //超音波の最大範囲を調べる
            SearchMaxRange();

            // パーティクルシステムのインスタンスを生成する。
            ParticleSystem newParticle = Instantiate(particle);

            newParticle.Stop();

            newParticle.transform.position = transform.position + (transform.up * 0.3f);

            var main = newParticle.main;

            main.startSize = maxRange * 2;
            main.startLifetime = maxRange / velocity;

            // パーティクルを発生させる。
            newParticle.Play();
            nowParticleSystem = newParticle;
        }

        range += velocity * Time.deltaTime;
        range = Mathf.Min(range, maxRange);

        //超音波の持続時間が終了した場合
        time += Time.deltaTime;
        if (time - duration < 0) return;
        aliveFlg = false;
        nowParticleSystem = null;
    }

    public override bool CheckHit()
    {
        if (nowParticleSystem == null) return false;

        //当たり判定
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerObject.transform.position - _firePos;

        //超音波の長さに調整する
        _targetVec = _targetVec.normalized * (range - 0.5f);

        //超音波本体の座標を算出
        Vector3 _pos = _firePos + _targetVec;

        //超音波本体とプレイヤーの距離を調べる
        _targetVec = playerObject.transform.position - _pos;

        float _distance = _targetVec.magnitude;

        if (_distance <= hitRange)
        {
            nowParticleSystem.Stop();
            nowParticleSystem = null;
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
}
