using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraSoundBeam : BaseUltrasound
{
    [SerializeField] private ParticleSystem particle;
    private float duration;

    private void Awake()
    {
        particle.Stop();
        velocity = 1;
        coolDown = 10.0f;
        time = 0;
        range = 0.0f;
        maxRange = 5.0f;
        duration = 10.0f;
        aliveFlg = true;
    }

    public override void Init()
    {
        particle.Stop();
        time = 0;
        range = 0.0f;
        aliveFlg = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        //初めて更新関数が実行される時
        if (range <= 0.0f)
        {
            particle.Play();
        }

        time += Time.deltaTime;

        //超音波ビームを長くする
        range += velocity * Time.deltaTime;
        range = Mathf.Min(range, maxRange);

        if (time - duration > 0)
        {
            aliveFlg = false;
            particle.Stop();
        }
    }

    public override bool CheckHit()
    {
        //当たり判定
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerCC.transform.position - _firePos;

        float dot = Vector3.Dot(transform.forward.normalized, _targetVec.normalized);

        if (Mathf.Acos(dot) * Mathf.Rad2Deg <= 20.0f)
        {
            //プレイヤーの方向に対する超音波の射程距離を出す。
            float withinRange = range / dot;

            //超音波の範囲ないにプレイヤーがいるか確認する
            if (_targetVec.magnitude - withinRange < 0)
            {
                Ray _ray = new Ray(_firePos, _targetVec);
                RaycastHit _raycastHit;

                //プレイヤーに向かってレイを発射
                bool hit = Physics.Raycast(_ray, out _raycastHit, withinRange);

                Debug.Log("超音波が当たった！！");
                return true;
            }
        }
        return false;
    }

    public override void DrawLine()
    {
        //当たり判定
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerCC.transform.position - _firePos;

        //デバッグ用の線を描画
        var lineRenderer = gameObject.GetComponent<LineRenderer>();

        var positions = new Vector3[]
        {
            _firePos,
            _firePos + (transform.forward * range),
        };

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.SetPositions(positions);
    }
}
