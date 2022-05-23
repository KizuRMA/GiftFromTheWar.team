using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    private CharacterController CC;
    [SerializeField] private GameObject ladderHund;
    [SerializeField] private playerHundLadder ladder;
    private MoveWindGun moveWindGun;

    private float moveSpeed;
    [SerializeField] private float normalSpeed = 3; // �ړ����x
    [SerializeField] private float dashSpeedRaito = 3; //���鑬���̔{��
    [SerializeField] private float turnSpeed = 20; //�U���������
    private float turnRot = 90; //�U������Ă������
    private float turnRaito = 180; //�U������i�K
    private bool moveFlg = false;
    private bool dashFlg = false;

    private Vector3 moveVelocity; // �L�����̈ړ����x���
    private Vector3 moveVec; // �����p

    public GameObject cam;
    Quaternion cameraRot, characterRot;
    [SerializeField] private float Xsensityvity = 3f, Ysensityvity = 3f;

    bool cursorLock = true;

    //�p�x�̐����p
    [SerializeField] float minX = -45f, maxX = 45f;

    //�d��
    [SerializeField] float gravity;

    void Start()
    {
        CC = GetComponent<CharacterController>(); // ���t���[���A�N�Z�X����̂ŁA���ׂ������邽�߂ɃL���b�V�����Ă���
        moveWindGun = this.GetComponent<MoveWindGun>();
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;

        moveSpeed = normalSpeed;
    }

    void Update()
    {
        UpdateCursorLock();

        if (ladder.GetTouchLadderFlg())
        {
            moveFlg = false;
            return;
        }

        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;

        if (turnRaito >= 180) //180�^�[�����g���Ă��Ȃ�������
        {
            CameraMove();
        }

        cameraRot = ClampRotation(cameraRot);

        MiddleClick();

        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        MoveKey();

        DashJudge();

    }

    private void FixedUpdate()
    {
        if (ladder.GetTouchLadderFlg())
        {
            return;
        }

        Dash();

        Move();

        CC.Move(moveVec* Time.deltaTime);

    }

    //--------------------------------------------------------------------
    //����֐�
    //--------------------------------------------------------------------
    public void UpdateCursorLock()  //�J�[�\���\���؂�ւ�
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

    public void MiddleClick()
    {
        if (Input.GetMouseButtonDown(2))
        {
            turnRaito = 0;
        }

        if (turnRaito < 180)
        {
            cameraRot[0] = 0;
            cameraRot[2] = 0;
            turnRaito += turnSpeed * Time.deltaTime;
            characterRot *= Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0);
        }
    }

    public void MoveKey()
    {
        moveVelocity.x = Input.GetAxis("Horizontal");
        moveVelocity.z = Input.GetAxis("Vertical");

        if(moveVelocity.x == 0 && moveVelocity.z == 0)
        {
            moveFlg = false;
        }
        else
        {
            moveFlg = true;
        }
    }

    public void Move()
    {

        moveVec = cam.transform.forward * moveVelocity.z + cam.transform.right * moveVelocity.x;
        moveVec.y = 0;

        moveVec.Normalize();
        moveVec *= moveSpeed;

        Gravity();
    }

    public void CameraMove()
    {
        //�}�E�X����p�x���v�Z
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

        //�J�����ƃL�����N�^�[�̉�]����
        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);
    }

    public void DashJudge()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            dashFlg = true;
        }
        else
        {
            dashFlg = false;
        }
    }

    public void Dash()
    {
        if (dashFlg)
        {
            moveSpeed = normalSpeed * dashSpeedRaito;
        }
        else
        {
            moveSpeed = normalSpeed;
        }
    }

    //�p�x�����֐��̍쐬
    public Quaternion ClampRotation(Quaternion q)
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
        if (moveWindGun.GetUpWindGunFlg()) return;

        moveVec.y += gravity;
    }

    //--------------------------------------------------------------------
    //�Q�b�^�[�Z�b�^�[
    //--------------------------------------------------------------------
    public bool GetMoveFlg()
    {
        return moveFlg;
    }

    public bool GetDashFlg()
    {
        return dashFlg;
    }

    public float GetGravity()
    {
        return gravity;
    }
}
