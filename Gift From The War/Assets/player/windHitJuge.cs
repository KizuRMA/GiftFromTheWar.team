using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windHitJuge : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private Transform trans;
    private MeshCollider MC;
    [SerializeField] private MoveWindGun wind;

    //����������������
    [SerializeField] private float windPower;
    [SerializeField] private float windPowerMax;
    private float nowWindPower = 0;
    [SerializeField] private float windPower2;
    [SerializeField] private float windPowerMax2;
    private float nowWindPower2 = 0;
    private Vector3 firstScale;

    void Start()
    {
        GameObject _parent = transform.parent.gameObject;
        //�e�I�u�W�F�N�g������ꍇ
        if (_parent == true)
        {
            wind = _parent.GetComponent<WindGunEffectInfo>().gun;
        }

        trans = transform;
        firstScale = trans.localScale;
        MC = this.GetComponent<MeshCollider>();
    }

    void Update()
    {
        if (!wind.effectFlg)    //�����o���Ă��Ȃ������珈�����Ȃ�
        {
            if (!MC.enabled) return; //���łɓ����蔻��������Ă����珈�����Ȃ�
            nowWindPower = 0;
            nowWindPower2 = 0;
            MC.enabled = false;
            return;
        }

        if(!MC.enabled) MC.enabled = true;  //�����蔻����I���ɂ���

        //�����蔻��𒷂�����
        nowWindPower += windPower * Time.deltaTime;
        nowWindPower = nowWindPower > windPowerMax ? windPowerMax : nowWindPower;

        //�����蔻���傫������
        nowWindPower2 += windPower2 * Time.deltaTime;
        nowWindPower2 = nowWindPower2 > windPowerMax2 ? windPowerMax2 : nowWindPower2;

        trans.localScale = new Vector3(nowWindPower2, nowWindPower2, nowWindPower);

    }
}
