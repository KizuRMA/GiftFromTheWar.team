using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    private float moveSpeed;
    [SerializeField] private float normalSpeed = 3; // �ړ����x
    [SerializeField] private float dashSpeedRaito = 3;//���鑬���̔{��
    [SerializeField] private float jumpPower = 3; // �W�����v��
    [SerializeField] private float fallSpeed = 3; //�����X�s�[�h
    [SerializeField] private float jumpingMoveSpeedRaito = 3; //�W�����v���̈ړ����x�␳
    [SerializeField] private float landingSpeed = 3; //���n�X�s�[�h
    [SerializeField] private float airResistRaito = 3; //��C��R�̕ω���
    private float airResist = 0; //���̋�C��R
    [SerializeField] private float jumpingAirResistRaito = 3; //�W�����v���̋�C��R�̕ω���
    private float jumpingAirResist = 1; //�W�����v���̍��̋�C��R
    [SerializeField] private float turnSpeed = 20; //�U���������
    private float turnRaito = 180; //�U������i�K

    private CharacterController _characterController; // CharacterController�̃L���b�V��
    private Transform _transform; // Transform�̃L���b�V��
    private Vector3 _moveVelocity; // �L�����̈ړ����x���
    private Vector3 _moveVec; // �����p

    public GameObject cam;
    Quaternion cameraRot, characterRot;
    float Xsensityvity = 3f, Ysensityvity = 3f;

    bool cursorLock = true;

    //�ϐ��̐錾(�p�x�̐����p)
    float minX = -60f, maxX = 60f;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>(); // ���t���[���A�N�Z�X����̂ŁA���ׂ������邽�߂ɃL���b�V�����Ă���
        _transform = transform; // Transform���L���b�V������Ə����������ׂ�������
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;

        moveSpeed = normalSpeed;
        airResist = landingSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (turnRaito >= turnSpeed) //180�^�[�����g���Ă��Ȃ�������
        {
            float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
            float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

            cameraRot *= Quaternion.Euler(-yRot, 0, 0);
            characterRot *= Quaternion.Euler(0, xRot, 0);
        }

        //Update�̒��ō쐬�����֐����Ă�
        cameraRot = ClampRotation(cameraRot);

        MiddleClick();

        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        UpdateCursorLock();
    }

    private void FixedUpdate()
    {
        _moveVelocity.x = Input.GetAxis("Horizontal");
        _moveVelocity.z = Input.GetAxis("Vertical");

        IsGroundProcess();

        _moveVec = cam.transform.forward * _moveVelocity.z + cam.transform.right * _moveVelocity.x;
        _moveVec.y = 0;

        _moveVec.Normalize();
        _moveVec *= moveSpeed;
        _moveVec.y = _moveVelocity.y;

        _characterController.Move(_moveVec);

    }

    //--------------------------------------------------------------------
    //����֐�
    //--------------------------------------------------------------------
    public void UpdateCursorLock()
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

        if (turnRaito < turnSpeed)
        {
            cameraRot[0] = 0;
            cameraRot[2] = 0;
            turnRaito++;
            cameraRot *= Quaternion.Euler(0, 180 / turnSpeed, 0);
        }
    }

    public void IsGroundProcess()
    {
        if (_characterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // �W�����v����
                _moveVelocity.y = jumpPower; // �W�����v�̍ۂ͏�����Ɉړ�������

                jumpingAirResist = 1.0f;
            }

            if (airResist < 1)
            {
                airResist += airResistRaito;
                if (airResist > 1)
                {
                    airResist = 1.0f;
                }

                moveSpeed *= airResist;
                moveSpeed *= airResist;
            }
        }
        else
        {
            // �d�͂ɂ�����
            _moveVelocity.y += Physics.gravity.y * fallSpeed;

            moveSpeed *= jumpingMoveSpeedRaito * jumpingAirResist;
            moveSpeed *= jumpingMoveSpeedRaito * jumpingAirResist;

            jumpingAirResist -= jumpingAirResistRaito;

            airResist = landingSpeed;
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

    //--------------------------------------------------------------------
    //�Q�b�^�[�Z�b�^�[
    //--------------------------------------------------------------------
    public float GetSetMoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float GetSetNormalSpeed
    {
        get { return normalSpeed; }
        set { normalSpeed = value; }
    }

    public GameObject GetCamera
    {
        get { return cam; }
    }
}
