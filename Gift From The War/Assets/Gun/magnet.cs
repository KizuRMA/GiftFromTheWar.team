using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnet : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private Transform trans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private remainingAmount energyAmount;
    [SerializeField] private GameObject cameraObj;

    //�e�̔���
    [SerializeField] private float shotSpeed;   //���˃X�s�[�h
    [SerializeField] private float range;       //�e�̏�����܂ł̎���
    [SerializeField] private float useEnergy;   //����G�l���M�[
    private bool shotFlg;                       //���ˉ\
    private Vector3 shotPos;                    //���e�_

    //���΂̏���
    public GameObject metal { get; set; }           //������������
    private Vector3 firstPos;                       //�����̍ŏ���Z���W
    private float nowReturnSpeed;                   //���̖߂鑬��
    [SerializeField] private float returnSpeed;     //�߂鑬��
    [SerializeField] private float returnSpeedMin;  //�߂鑬���̍ŏ��l
    [SerializeField] private float returnSpeedMax;  //�߂鑬���̍ŏ��l
    [SerializeField] private float useEnergyMag;    //�������Ă���Ƃ��ɏ����G�l���M�[
    private bool magnetFlg = false;                 //���΂��G�ꂽ�u�Ԃ̃t���O
    private bool cameraOverFlg = false;             //�������J�����O�ɂł���
    [SerializeField] private float cameraOverMax;   //�J�����̊O�̏��
    public float sensityvity;                       //�J�����̊��x

    private void Start()
    {
        trans = transform;
        metal = null;
    }

    void Update()
    {
        //�G�l���M�[���K�v�ʂ����
        shotFlg = energyAmount.GetSetNowAmount > (1.0f - useEnergy);

        //�G�l���M�[���g�p���Ȃ��Ƃ���0�ɂ���
        if (!Input.GetMouseButtonDown(1) || energyAmount.GetSetNowAmount <= 0 || cameraOverFlg)
        {
            energyAmount.GetSetNowAmount = 0;
        }

        //���˃L�[����������
        if (Input.GetMouseButtonDown(1))
        {
            if(shotFlg && !magnetFlg)
            Shot();
        }

        //���˂����e�������ɓ������ĂȂ�������A�������Ȃ�
        if (metal == null) return;

        CatchMetal();
    }

    private void Shot() //�e��ł���
    {
        energyAmount.GetSetNowAmount = useEnergy;
        energyAmount.useDeltaTime = false;

        BulletVecter();

        CreateBullet();
    }

    private void BulletVecter() //�e�̌��������߂�
    {
        //�v���C���[�̑O�Ƀ��C������΂��A�I�u�W�F�N�g�Ƃ̋��������߂�B
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            shotPos = hit.point;
        }
    }

    private void CreateBullet() //�v���n�u����e�����
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, trans.position, Quaternion.identity);
        trans.LookAt(shotPos);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(trans.forward * shotSpeed * Time.deltaTime);

        //�ˌ�����Ă���w��b��ɏe�e�̃I�u�W�F�N�g��j�󂷂�
        Destroy(bullet, range);
    }

    private void CatchMetal()   //�����𑀂�
    {
        if (!magnetFlg) //�ŏ��ɋ����ɐG�ꂽ�Ƃ������s��
        {
            magnetFlg = true;
            cameraOverFlg = false;
            metal.transform.parent = cameraObj.gameObject.transform;
            firstPos = metal.transform.localPosition;
            metal.GetComponent<Rigidbody>().useGravity = false;
            metal.gameObject.AddComponent<metalHitJudge>();
        }

        ReturnMiddle();
        EraseInertia();

        //�G�l���M�[����
        energyAmount.GetSetNowAmount = useEnergyMag;
        energyAmount.useDeltaTime = true;

        CameraOver();

        Relieve();
    }

    private void EraseInertia() //����������
    {
        metal.GetComponent<Rigidbody>().velocity = Vector3.zero;
        metal.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void AddInertia() //����������
    {
        metal.GetComponent<Rigidbody>().useGravity = true;
    }

    private void ReturnMiddle() //�^�񒆂ɖ߂鏈��
    {
        Vector3 returnVec = metal.transform.localPosition - firstPos;   //�߂�������Z�o

        ReturnSpeed();

        bool returnVecLarge = returnVec.magnitude > nowReturnSpeed * Time.deltaTime;   //�߂�x�N�g�����傫�����Ȃ���
        if (returnVecLarge)
        {
            returnVec = returnVec.normalized * nowReturnSpeed * Time.deltaTime;
        }
        metal.transform.localPosition -= returnVec;
    }

    private void ReturnSpeed()  //�߂鑬���̎Z�o
    {
        //�I�u�W�F�N�g�ɓ����������ǂ����i�����������������ǂ����Ŕ��f����j
        bool inertiaFlg = metal.GetComponent<metalHitJudge>().hitJudge;
        if (inertiaFlg)
        {
            nowReturnSpeed = returnSpeedMin;
        }
        else
        {
            nowReturnSpeed += returnSpeed * Time.deltaTime;
        }

        //����␳
        if (nowReturnSpeed > returnSpeedMax)
        {
            nowReturnSpeed = returnSpeedMax;
        }
    }

    private void CameraOver()   //�J�����̊O�ɏo�鏈��
    {
        cameraOverFlg = (metal.transform.localPosition - firstPos).magnitude > cameraOverMax;
    }

    private void Relieve()   //��������
    {
        //�������鏈��
        if (Input.GetMouseButtonDown(1) || energyAmount.GetSetNowAmount <= 0 || cameraOverFlg)
        {
            magnetFlg = false;
            metal.transform.parent = null;
            AddInertia();
            metal = null;
        }
    }
}
