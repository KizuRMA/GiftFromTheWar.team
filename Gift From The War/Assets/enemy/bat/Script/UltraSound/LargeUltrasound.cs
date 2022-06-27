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
            //�����g�̍ő�͈͂𒲂ׂ�
            SearchMaxRange();

            // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�����
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

            // �p�[�e�B�N���𔭐�������B
            nowParticleSystem.Play();
            StartCoroutine(DelayCoroutine());
            range += 0.001f;
        }

        //�x�����������ĂȂ��ꍇ
        if (delayEnd == false) return;

        range += velocity * Time.deltaTime;
        range = Mathf.Min(range, maxRange);

        //�����g�̎������Ԃ��I�������ꍇ
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

        //�����蔻��
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = (playerObject.transform.position) - _firePos;

        //���C����
        Ray _ray = new Ray(_firePos,_targetVec);
        RaycastHit _raycastHit;

        //�V��Ɍ������ă��C����
        bool hit = Physics.Raycast(_ray, out _raycastHit, range,layer);

        //���C����
        if (hit == false || _raycastHit.collider.tag != "Player") return false;

        //�����g�̒����ɒ�������
        _targetVec = _targetVec.normalized * (range - 0.7f);

        //�����g�{�̂̍��W���Z�o
        Vector3 _pos = _firePos + _targetVec;

        //�����g�{�̂ƃv���C���[�̋����𒲂ׂ�
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
