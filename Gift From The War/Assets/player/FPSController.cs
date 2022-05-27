using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g���Ƃ��Ă���
    [SerializeField] private CharacterController CC;
    [SerializeField] private GameObject ladderHund;
    [SerializeField] private playerHundLadder ladder;
    [SerializeField] private MoveWindGun moveWindGun;
    [SerializeField] private GameObject cam;
    [SerializeField] private playerDied died;

    //�J�[�\�����b�N
    private bool cursorLock = true;

    //�v���C���[��XZ�ړ�
    private Transform trans;
    [SerializeField] private float walkSpeed;       //���s���x
    [SerializeField] private float dashSpeedRaito;  //���鑬���̔{��
    private float nowMoveSpeed;                     //���̈ړ����x
    public bool moveFlg { get; set; }
    public bool dashFlg { get; set; }
    public bool groundFlg { get; set; }

    //�d��
    [SerializeField] private float gravity;
    private float nowGravity;
    [SerializeField] private float groundDis;   //�n�ʂƂ̋���

    //�v���C���[�ړ��S��
    private Vector3 moveVelocity; // �L�����̈ړ����x���
    private Vector3 moveVec; // �����p

    //�v���C���[�̉�]
    Quaternion cameraRot, characterRot;
    [SerializeField] private float Xsensityvity = 3f, Ysensityvity = 3f;    //�U��������x
    [SerializeField] private float turnSpeed; //�U���������
    private float turnRaito = 180; //�U������i�K
    [SerializeField] private float minX = -45f, maxX = 45f; //�p�x�̐����p

    void Start()
    {
        trans = transform;
        nowMoveSpeed = walkSpeed;
        moveFlg = false;
        dashFlg = false;
        groundFlg = false;
    }

    void Update()
    {
        UpdateCursorLock();

        if (ladder.touchLadderFlg || died.diedFlg)
        {
            moveFlg = false;
            return;
        }

        AssignTmpRot();

        MiddleClick();

        if (turnRaito >= 180) //180�^�[�����g���Ă��Ȃ�������
        {
            CameraMove();
        }

        cameraRot = ClampRotation(cameraRot);

        AssignFinalRot();

        MoveKey();

        DashJudge();

        Dash();

        Move();

        CC.Move(moveVec* Time.deltaTime);

    }

    //--------------------------------------------------------------------
    //����֐�
    //--------------------------------------------------------------------
    private void UpdateCursorLock()  //�J�[�\���\���؂�ւ�
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void AssignTmpRot()    //�v�Z���邽�߂ɉ�]�ʂ�ێ�����
    {
        cameraRot = cam.transform.localRotation;
        characterRot = trans.localRotation;
    }

    private void AssignFinalRot()   //�v�Z������]�ʂ���
    {
        cam.transform.localRotation = cameraRot;
        trans.localRotation = characterRot;
    }

    private void MiddleClick()
    {
        if (Input.GetMouseButtonDown(2))
        {
            turnRaito = 0;
        }

        if (turnRaito < 180)
        {
            //Y����]�����ɂ���
            cameraRot[0] = 0;
            cameraRot[2] = 0;

            turnRaito += turnSpeed * Time.deltaTime;    //�w�肵���p�x�ɂȂ�܂ŉ�]����

            characterRot *= Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0); //��]��ێ�
        }
    }

    private void MoveKey()
    {
        //�ړ��������Ƃ��Ă���
        moveVelocity.x = Input.GetAxis("Horizontal");
        moveVelocity.z = Input.GetAxis("Vertical");

        moveFlg = !(moveVelocity.x == 0 && moveVelocity.z == 0);
    }

    private void Move()
    {
        //�ړ��������v�Z
        moveVec = cam.transform.forward * moveVelocity.z + cam.transform.right * moveVelocity.x;
        moveVec.y = 0;

        //�ړ��ʂ��v�Z
        moveVec.Normalize();
        moveVec *= nowMoveSpeed;

        Gravity();
    }

    private void CameraMove()
    {
        //�}�E�X����p�x���v�Z
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

        //�J�����ƃL�����N�^�[�̉�]����
        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);
    }

    private void DashJudge()
    {
        dashFlg = Input.GetKey(KeyCode.LeftShift);
    }

    private void Dash()
    {
        nowMoveSpeed = dashFlg ? walkSpeed * dashSpeedRaito : walkSpeed;
    }
    
    private Quaternion ClampRotation(Quaternion q)  //�p�x����
    {
        //q = x,y,z,w (x,y,z�̓x�N�g���i�ʂƌ����j�Fw�̓X�J���[�i���W�Ƃ͖��֌W�̗ʁj)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }

    private void Gravity()
    {
        if (moveWindGun.upWindFlg)
        {
            nowGravity = gravity * Time.deltaTime;
            return;
        }

        Ray ray = new Ray(trans.position, -trans.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, groundDis))
        {
            nowGravity = gravity * Time.deltaTime;

            if (groundFlg) return;
            moveVec.y += 1.0f;  //���S�ɒn�ʂɂ��邽�߂̏���
            groundFlg = true;
            return;
        }
        else
        {
            groundFlg = false;
        }

        nowGravity += gravity * Time.deltaTime;
        moveVec.y += nowGravity;
    }

    public float GetGravity //�d�͂̃Q�b�^�[
    {
        get { return gravity; }
    }
}
