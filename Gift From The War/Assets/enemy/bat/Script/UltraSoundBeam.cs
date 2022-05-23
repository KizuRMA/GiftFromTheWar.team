using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraSoundBeam : BaseUltrasound
{
    [SerializeField] private ParticleSystem particle;

    private void Awake()
    {
        particle.Stop();
        playerObject = GameObject.Find("player").gameObject;
    }

    public override void Start()
    {
        particle.Stop();
        velocity = 1;
        coolDown = 0.0f;
        duration = 10.0f;
        time = 0;
        range = 0.0f;
        maxRange = 5.0f;
        aliveFlg = true;
    }

    public override void Init()
    {
        particle.Stop();
        coolDown = 5.0f;
        time = 0;
        range = 0.0f;
        aliveFlg = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        //���߂čX�V�֐������s����鎞
        if (range <= 0.0f)
        {
            particle.Play();
        }

        //�����g�r�[���𒷂�����
        range += velocity * Time.deltaTime;
        range = Mathf.Min(range, maxRange);

        if (range >= maxRange)
        {
            //�����g�̎������Ԃ��I�������ꍇ
            time += Time.deltaTime;
            if (time - duration < 0) return;
            aliveFlg = false;
            particle.Stop();
        }
    }

    public override bool CheckHit()
    {
        //�����蔻��
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerObject.transform.position - _firePos;

        float dot = Vector3.Dot(transform.forward.normalized, _targetVec.normalized);

        if (Mathf.Acos(dot) * Mathf.Rad2Deg <= 20.0f)
        {
            //�v���C���[�̕����ɑ΂��钴���g�̎˒��������o���B
            float withinRange = range / dot;

            //�����g�͈̔͂Ȃ��Ƀv���C���[�����邩�m�F����
            if (_targetVec.magnitude - withinRange < 0)
            {
                Ray _ray = new Ray(_firePos, _targetVec);
                RaycastHit _raycastHit;

                //�v���C���[�Ɍ������ă��C�𔭎�
                bool hit = Physics.Raycast(_ray, out _raycastHit, withinRange);

                Debug.Log("�����g�����������I�I");
                return true;
            }
        }
        return false;
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
            _firePos + (transform.forward * range),
        };

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.SetPositions(positions);
    }
}
