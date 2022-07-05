using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnet : ShootParent
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private GameObject bulletLineEffect;
    private GameObject bulletLinePos;
    private bulletChange bulletChange;
    [SerializeField] private GetItem getItem;
    private magnetChain magnetChain;

    //�e�̔���
    private bool shotFlg;                       //���ˉ\

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

    //�ǂ��蔲���h�~
    private List<GameObject> colliders = new List<GameObject>();    //�����蔻��̃Q�[���I�u�W�F�N�g�i�[
    private List<Vector3> pastPosList = new List<Vector3>();        //�����蔻��̍��W��ۑ�
    private Vector3 pastPos;                                        //�I�u�W�F�N�g�̍��W��ۑ�
    private Quaternion pastQua;                                     //�I�u�W�F�N�g�̊p�x��ۑ�

    private void Start()
    {
        //�ϐ�������
        bulletLinePos = transform.Find("muzzlePosLine").gameObject;

        if (transform.parent != null)
        {
            bulletChange = transform.parent.GetComponent<bulletChange>();
        }

        magnetChain = transform.GetComponent<magnetChain>();

        trans = transform;
        metal = null;
        bulletLineEffect.SetActive(false);
    }

    public void Finish()    //�����؂�ւ������̏I������
    {
        if (metal != null)
        {
            metal.transform.parent = null;
            AddInertia();
        }
        metal = null;
        bulletLineEffect.SetActive(false);
        nowReturnSpeed = 0;
        shotFlg = false;
        magnetFlg = false;
        cameraOverFlg = false;
    }

    void Update()
    {
        if (!getItem.magnetAmmunitionFlg) return;   //�e���E���ĂȂ������珈�����Ȃ�

        if (magnetChain.metalFlg) return;   //���łɕʂ̎��΂�ł��Ă����珈�����Ȃ�

        MoveBullet();

        if (bulletChange.nowBulletType != bulletChange.bulletType.e_magnet) return; //���̒e�̎�ނ��Ή����ĂȂ�������

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
            if (shotFlg && !magnetFlg)
                Shot();
        }

        //���˂����e�������ɓ������ĂȂ�������A�������Ȃ�
        if (metal == null)
        {
            bulletLineEffect.SetActive(false);
            return;
        }

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

    private void CatchMetal()   //�����𑀂�
    {
        if (!magnetFlg) //�ŏ��ɋ����ɐG�ꂽ�Ƃ������s��
        {
            magnetFlg = true;
            cameraOverFlg = false;
            metal.transform.parent = cameraObj.gameObject.transform;
            firstPos = metal.transform.localPosition;
            pastPos = metal.transform.position;
            pastQua = metal.transform.rotation;
            metal.GetComponent<Rigidbody>().useGravity = false;
            metal.gameObject.AddComponent<metalHitJudge>();

            if (metal.GetComponent<Rigidbody>().isKinematic)
            {
                metal.GetComponent<Rigidbody>().isKinematic = false;
            }

            ColliderInit();
        }

        ReturnMiddle();
        EraseInertia();

        //���C���G�t�F�N�g
        bulletLineEffect.SetActive(true);
        bulletLineEffect.transform.position = bulletLinePos.transform.position;
        bulletLineEffect.transform.LookAt(metal.transform.position);

        //�G�l���M�[����
        energyAmount.GetSetNowAmount = useEnergyMag;
        energyAmount.useDeltaTime = true;

        CameraOver();

        ThroughWall();

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

    private void ColliderInit() //�����蔻��p�̃Q�[���I�u�W�F�N�g�̏�����
    {
        GameObject colliderChild = metal.transform.Find("collider").gameObject;
        for (int i = 0; i < colliderChild.transform.childCount; i++)
        {
            colliders.Add(colliderChild.transform.GetChild(i).gameObject);
            pastPosList.Add(colliders[i].transform.position);
        }
    }

    private void ThroughWall()  //�ǂ�ʂ蔲����
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            Vector3 vec = colliders[i].transform.position - pastPosList[i];

            Ray ray = new Ray(pastPosList[i], vec);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, vec.magnitude))
            {
                if (hit.collider.gameObject.tag == "cave")
                {
                    metal.transform.position = pastPos;
                    metal.transform.rotation = pastQua;
                }
            }

            pastPosList[i] = colliders[i].transform.position;
        }

        pastPos = metal.transform.position;
        pastQua = metal.transform.rotation;
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
            colliders.Clear();
            pastPosList.Clear();
        }
    }
}
