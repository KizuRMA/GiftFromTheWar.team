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
        aliveFlg = true;
    }

    public void Init()
    {
        velocity = defaultVelocity;
        length = defaultLength;
        duration = defaultDuration;
        longestLength = defaultLongestLength;
        aliveFlg = true;
    }

    public bool IsAlive()
    {
        return aliveFlg;
    }

    // Update is called once per frame
    public bool Update()
    {
        //���߂čX�V�֐������s����鎞
        if (length <= defaultLength)
        {
            nowTime = Time.time;
        }

        //�����g�����X�ɉ����ɔ�΂�
        length += velocity;
        length = Mathf.Min(length, longestLength);

        //�����蔻��
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerCC.transform.position - _firePos;

        //�����g�̒����ɒ�������
        _targetVec = _targetVec.normalized * length;

        //�����g�{�̂̍��W���Z�o
        Vector3 _pos = _firePos + _targetVec;

        //�����g�{�̂ƃv���C���[�̋����𒲂ׂ�
        _targetVec = playerCC.transform.position - _pos;
        float _distance = _targetVec.magnitude;

        //�f�o�b�O�p�̐���`��
        var lineRenderer = gameObject.GetComponent<LineRenderer>();

        var positions = new Vector3[]
        {
            _firePos,
            _firePos + ((playerCC.transform.position - _firePos).normalized * length),
        };

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.SetPositions(positions);

        //�������Ԃ��v�Z
        if (nowTime + duration <= Time.time)
        {
            aliveFlg = false;
            lineRenderer.hideFlags = HideFlags.HideInHierarchy;
        }

        //Debug.Log(_distance);

        //�����g�Ƃ̋������߂��ꍇ
        if (_distance <= 0.4f)
        {
            //�������Ă���
            return true;
        }
        //�������Ă��Ȃ�
        return false;
    }
}
