using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeUltrasound : BaseUltrasound
{
    private float minimumRange;
    private float hitRange;

    private void Awake()
    {
        playerObject = GameObject.Find("player").gameObject;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        velocity = 1;
        coolDown = 0;
        duration = 1.0f;
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
            //�����g�̍ő�͈͂𒲂ׂ�
            SearchMaxRange();
        }

        range += velocity * Time.deltaTime;
        range = Mathf.Min(range, maxRange);

        if (range >= maxRange)
        {
            //�����g�̎������Ԃ��I�������ꍇ
            time += Time.deltaTime;
            if (time - duration < 0) return;
            aliveFlg = false;
        }
    }

    public override bool CheckHit()
    {
        //�����蔻��
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerObject.transform.position - _firePos;

        //�����g�̒����ɒ�������
        _targetVec = _targetVec.normalized * range;

        //�����g�{�̂̍��W���Z�o
        Vector3 _pos = _firePos + _targetVec;

        //�����g�{�̂ƃv���C���[�̋����𒲂ׂ�
        _targetVec = playerObject.transform.position - _pos;

        float _distance = _targetVec.magnitude;

        if (_distance <= hitRange)
        {
            return true;
        }
        return false;
    }

    private void SearchMaxRange()
    {
        //���C���[�}�X�N��"cave"�i���A�j�ɂ��ă��C������s��
        int layerMask = 1 << 9;
        Ray _ray = new Ray(transform.position, transform.up);
        RaycastHit _raycastHit;

        //�V��Ɍ������ă��C����
        bool hit = Physics.Raycast(_ray, out _raycastHit, 1000.0f, layerMask);

        if (hit == true)
        {
            //�����g�͈̔͂��Œ�l�����������ꍇ
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
        //�����蔻��
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerObject.transform.position - _firePos;

        //�f�o�b�O�p�̐���`��
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
