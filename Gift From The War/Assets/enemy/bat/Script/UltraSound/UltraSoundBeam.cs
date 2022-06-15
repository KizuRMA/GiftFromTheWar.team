using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraSoundBeam : BaseUltrasound
{
    [SerializeField] private ParticleSystem particle;
    float delay;
    bool delayEnd;

    private void Awake()
    {
        particle.Stop();
        playerObject = GameObject.Find("player").gameObject;
        var main = particle.main;
        duration = main.duration;
        velocity = 0.5f * 4;
        delay = 2.0f;
    }

    public override void Start()
    {
        particle.Stop();
        coolDown = 0.0f;
        time = 0;
        range = 0.0f;
        aliveFlg = true;
        delayEnd = false;
    }

    public override void Init()
    {
        particle.Stop();
        coolDown = 5.0f;
        time = 0;
        range = 0.0f;
        aliveFlg = true;
        delayEnd = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        //初めて更新関数が実行される時
        if (range <= 0.0f)
        {
            particle.Play();
            StartCoroutine(DelayCoroutine());
            range += 0.001f;
        }

        //遅延が完了してない場合
        if (delayEnd == false) return;

        //超音波の持続時間が終了した場合
        time += Time.deltaTime;

        //超音波ビームを長くする
        range += velocity * Time.deltaTime;
        //range = Mathf.Min(range, maxRange);

        if (time - duration < 0) return;
        aliveFlg = false;
        particle.Stop();
    }

    public override bool CheckHit()
    {
        //当たり判定
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerObject.transform.position - _firePos;

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

                return true;
            }
        }
        return false;
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
            _firePos + (transform.forward * range),
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
}
