using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerDied : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    private CharacterController CC;
    private Transform trans;
    [SerializeField] private Gravity gravity;
    [SerializeField] private GameObject rantan;
    private GameObject gun;
    private Rigidbody rantanRD;
    private Rigidbody gunRD;

    public bool diedFlg { get; set; }

    //�ړ�
    [SerializeField] private float height;      //����
    [SerializeField] private float downSpeed;   //������X�s�[�h
    [SerializeField] private float downMax;     //������ő�l
    private float nowGravity;                   //���̏d�͉����x
    private bool groundFlg = false;             //��x�ł��n�ʂɂ������ǂ���

    //��]
    [SerializeField] private float rotSpeed;    //��]�X�s�[�h
    [SerializeField] private float rotMax;      //��]�̍ő�l
    private float rotSum = 0;                   //��]�̍��v�l
    [SerializeField] private float gunRotSpeed; //�e�̉�]�X�s�[�h

    //�ڂ����
    [SerializeField] private GameObject eye;    //�ڂ̉摜
    [SerializeField] private GameObject eye2;   //�ڂ̉摜
    [SerializeField] private float eyeCoolTime; //�ڂ����܂ł́A�N�[���^�C��
    [SerializeField] private float sceneCoolTime; //�ڂ����܂ł́A�N�[���^�C��
    [SerializeField] private float moveEyeSpeed;//�ق̓�������
    private RectTransform eyeRec;               //�ڂ̉摜���
    private RectTransform eye2Rec;              //�ڂ̉摜���
    private bool eyeCloseFlg = false;           //�ڂ����t���O
    private float eyeTime = 0;  //�|��Đ��b��ڂ���鏈���Ɏg�p����

    void Start()
    {
        //�ϐ���������
        GunUseInfo _info = transform.GetComponent<GunUseInfo>();
        gun = _info.gunModel;

        CC = this.GetComponent<CharacterController>();
        trans = transform;
        rantanRD = rantan.GetComponent<Rigidbody>();
        gunRD = gun.GetComponent<Rigidbody>();
        diedFlg = false;
        nowGravity = gravity.GetGravity * Time.deltaTime;
        eyeRec = eye.GetComponent<RectTransform>();
        eye2Rec = eye2.GetComponent<RectTransform>();
        //eyeRec.localPosition = new Vector3(0 ,900, 0);
        //eye2Rec.localPosition = new Vector3(0, -900, 0);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    CC.GetComponent<playerAbnormalcondition>().life = 0;
        //}

        if (CC.GetComponent<playerAbnormalcondition>().life <= 0)   //HP���O�ɂȂ��Ă�����
        {
            diedFlg = true;
            playerHundLadder _ladder = transform.GetComponent<playerHundLadder>();
            _ladder.FinishLadder();

            CC.height = height;

            //�e�q�֌W�폜
            rantan.transform.parent = null;
            gun.transform.parent = null;

            rantanRD.useGravity = true;
            gunRD.useGravity = true;

            //�ړ��p�x�����폜
            rantanRD.constraints = RigidbodyConstraints.None;
            gunRD.constraints = RigidbodyConstraints.None;

            //�^�C���A�^�b�N�֌W
            if (TimeAttackManager.Instance.timeAttackFlg)
            {
                TimeAttackManager.Instance.timerStopFlg = true;
                TimeAttackManager.Instance.timerStartFlg = false;
                TimeAttackManager.Instance.playerDiedFlg = true;
                TimeAttackManager.Instance.TimerHide();
            }
        }

        if (!diedFlg) return;

        DownKnees();

        Fall();

        if (!eyeCloseFlg) return;

        MoveEye();
    }

    private void DownKnees()    //�G����
    {
        if (groundFlg) return;  //�n�ʂɂ��Ă�����ʂ�Ȃ�

        eyeTime += Time.deltaTime;
        //���C����Œn�ʂɒ��������m�F����
        Ray ray = new Ray(trans.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, downMax) || eyeTime >= 2.0f)
        {
            groundFlg = true;
            StartCoroutine(EyeCoolTime());
            StartCoroutine(SceneCoolTime());
        }

        nowGravity += gravity.GetGravity * Time.deltaTime;
        CC.Move(new Vector3(0, nowGravity, 0) * Time.deltaTime);   //�v���C���[���ړ�
    }

    private void Fall() //�|���Ƃ��̉�]
    {
        if (!groundFlg) return;
        if (rotSum > rotMax) return;

        rotSum += rotSpeed * Time.deltaTime;    //��]�̍��v��ۑ�
        trans.rotation *= Quaternion.Euler(rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime);    //�v���C���[����]
        gun.transform.rotation *= Quaternion.Euler(0, 0, gunRotSpeed * Time.deltaTime); //�e����]
    }

    private IEnumerator EyeCoolTime()  //�񕜂܂ł̃N�[���^�C��
    {
        yield return new WaitForSeconds(eyeCoolTime);  //�N�[���^�C�����҂�

        eyeCloseFlg = true;
        eye.SetActive(true);
        eye2.SetActive(true);
    }

    private void MoveEye()  //�ڂ𓮂���
    {
        if (eyeRec.localPosition.y > 0)
        {
            eyeRec.localPosition += new Vector3(0, -moveEyeSpeed * Time.deltaTime, 0);
        }

        if (eye2Rec.localPosition.y < 0)
        {
            eye2Rec.localPosition += new Vector3(0, moveEyeSpeed * Time.deltaTime, 0);
        }
    }

    private IEnumerator SceneCoolTime()  //�񕜂܂ł̃N�[���^�C��
    {
        yield return new WaitForSeconds(sceneCoolTime);  //�N�[���^�C�����҂�

        CursorManager.Instance.SetCursorLock(false);

        SceneManager.LoadScene("GameOverScene");
    }
}
