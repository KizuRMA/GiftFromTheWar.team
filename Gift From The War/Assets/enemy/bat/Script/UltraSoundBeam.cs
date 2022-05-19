using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraSoundBeam : MonoBehaviour
{
    private CharacterController playerCC;
    private ParticleSystem particle;
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
        particle = transform.Find("ultrasoundBeem").gameObject.GetComponent<ParticleSystem>();
        particle.Stop();

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
        longestLength = defaultLongestLength;
        nowTime = 0;
        particle.Stop();
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
        if(length <= defaultLength)
        {
            particle.Play();
        }

        nowTime += Time.deltaTime;

        //�����g�r�[���𒷂�����
        length += velocity;
        length = Mathf.Min(length,longestLength);

        //�����蔻��
        Vector3 _firePos = transform.position + (transform.up * 0.3f);
        Vector3 _targetVec = playerCC.transform.position - _firePos;

        float dot = Vector3.Dot(transform.forward.normalized,_targetVec.normalized);

        //�f�o�b�O�p�̐���`��
        var lineRenderer = gameObject.GetComponent<LineRenderer>();

        var positions = new Vector3[]
        {
            _firePos,
            _firePos + (transform.forward * length),
        };

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.SetPositions(positions);

        if (nowTime - duration > 0)
        {
            aliveFlg = false;
            particle.Stop();
        }

        if (Mathf.Acos(dot) * Mathf.Rad2Deg <= 20.0f)
        {
            //�v���C���[�̕����ɑ΂��钴���g�̎˒��������o���B
            float withinRange = length / dot;

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
}
