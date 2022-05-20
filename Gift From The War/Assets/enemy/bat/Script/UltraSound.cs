using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraSound : MonoBehaviour
{
    private CharacterController playerCC;
    [SerializeField] private float longestLength;
    [SerializeField] private float duration;
    [SerializeField] private float length;
    [SerializeField] private float velocity;
    private float defaultVelocity;
    private float defaultDuration;
    private float defaultLength;
    private float defaultLongestLength;
    private float nowTime;
    private bool aliveFlg;

    private void Awake()
    {
        playerCC = GameObject.Find("player").GetComponent<CharacterController>();

        defaultVelocity = velocity;
        defaultLength = length;
        defaultDuration = duration;
        defaultLongestLength = longestLength;
        nowTime = 0;
        aliveFlg = true;
    }

    public void Init()
    {
        velocity = defaultVelocity;
        length = defaultLength;
        duration = defaultDuration;
        longestLength = 0;
        nowTime = 0;
        aliveFlg = true;
    }

    public bool IsAlive()
    {
        return aliveFlg;
    }

    // Update is called once per frame
    public bool Update()
    {
        if (longestLength <= 0)
        {
            SearchLongestLength();
        }
        nowTime += Time.deltaTime;

        //超音波を徐々に遠くに飛ばす
        length += velocity * Time.deltaTime;
        length = Mathf.Min(length, longestLength);

        //当たり判定
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerCC.transform.position - _firePos;

        //超音波の長さに調整する
        _targetVec = _targetVec.normalized * length;

        //超音波本体の座標を算出
        Vector3 _pos = _firePos + _targetVec;

        //超音波本体とプレイヤーの距離を調べる
        _targetVec = playerCC.transform.position - _pos;
        float _distance = _targetVec.magnitude;

        //デバッグ用の線を描画
        var lineRenderer = gameObject.GetComponent<LineRenderer>();

        var positions = new Vector3[]
        {
            _firePos,
            _firePos + ((playerCC.transform.position - _firePos).normalized * length),
        };

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.SetPositions(positions);

        //持続時間を計算
        if (nowTime - duration > 0)
        {
            aliveFlg = false;
        }

        //超音波との距離が近い場合
        if (_distance <= 0.4f)
        {
            //当たっている
            return true;
        }
        //当たっていない
        return false;
    }

    public void SearchLongestLength()
    {
        int layerMask = 1 << 9;
        Ray _ray = new Ray(transform.position, transform.up);
        RaycastHit _raycastHit;

        //プレイヤーに向かってレイを発射
        bool hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, layerMask);

        if (hit == true)
        {
            //レイが天井に衝突している場合はターゲット座標に設定する。
            longestLength = _raycastHit.distance;
            if (longestLength <= defaultLongestLength)
            {
                longestLength = defaultLongestLength;
            }
        }
        else
        {
            longestLength = defaultLongestLength;
        }
    }
}
