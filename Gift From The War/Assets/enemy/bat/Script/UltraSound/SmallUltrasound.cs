using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallUltrasound : BaseUltrasound
{
    private void Awake()
    {
        playerObject = GameObject.Find("player").gameObject;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        velocity = 1;
        coolDown = 0;
        duration = 0;
        time = 0;
        range = 0.0f;
        maxRange = 1.0f;
    }

    // Update is called once per frame
    public override void Update()
    {
        range += velocity * Time.deltaTime;
        range = Mathf.Min(range, maxRange);
    }

    public override bool CheckHit()
    {
        Vector3 _targetPos = playerObject.transform.position;
        Vector3 _firePos = transform.position + (transform.up * 0.3f);

        float _distance = Vector3.Distance(_targetPos, _firePos);
        if (_distance <= range)
        {
            return true;
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
            _firePos + ((playerObject.transform.position - _firePos).normalized * range),
        };

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.SetPositions(positions);
    }
}
