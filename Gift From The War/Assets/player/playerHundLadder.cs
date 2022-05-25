using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHundLadder : MonoBehaviour
{
    //�Q�[���I�u�W�F�N�g��X�N���v�g
    [SerializeField] private CharacterController playerCC;
    [SerializeField] private GameObject playerCamera;
    private Transform playerTrans;
    private Transform cmaTrans;

    //�t���O�֌W
    public bool closeLadderFlg { get; set; }    //��q�̋߂��ɂ��邩�ǂ���
    public bool touchLadderFlg { get; set; }     //��q�ɐG��Ă��邩

    private bool moveBeforeFlg = false;     //��q�̑O�܂ł�������
    private bool rotXBeforeFlg = false;     //��q�̕��Ɍ����Ă��邩
    private bool rotYBeforeFlg = false;     //��q�̕��Ɍ����Ă��邩

    //��q�̏��
    private Vector3 ladderStartPos;     //��q�̎n�_�̍��W
    private Vector3 ladderStartRot;     //��q�̌���
    private Quaternion ladderStartQua;  //��q�̌���
    private Vector3 ladderEndPos;       //��q�̏I�_�̍��W

    //��q�܂ł̈ړ�
    [SerializeField] private float moveBeforeSpeed;   //��q�̑O�܂ōs������
    [SerializeField] private float rotXBeforeSpeed;   //X����]�̑���
    [SerializeField] private float rotYBeforeSpeed;   //Y����]�̑���

    [SerializeField] private float climbSpeed;      //��q��鑬��

    void Start()
    {
        playerTrans = playerCC.transform;
        cmaTrans = playerCamera.transform;

        touchLadderFlg = false;
        closeLadderFlg = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "ladder") return;
        closeLadderFlg = true;

        //��q�̏������
        ladderStartPos = other.gameObject.transform.GetChild(2).gameObject.transform.position;
        ladderStartRot = other.gameObject.transform.GetChild(2).gameObject.transform.eulerAngles;
        ladderStartQua = other.gameObject.transform.GetChild(2).gameObject.transform.rotation;
        ladderEndPos = other.gameObject.transform.GetChild(3).gameObject.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        closeLadderFlg = false;
    }

    void Update()
    {
        if (closeLadderFlg && Input.GetKeyDown(KeyCode.Space))
        {
            touchLadderFlg = true;
        }

        if (!touchLadderFlg) return;

        if (!MoveBeforeFinishFlg())
        {
            MoveLadderBefore();
        }

        if (MoveBeforeFinishFlg())
        {
            ClimbLadder();

            DescendLadder();

            GoUpLadder();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            FinishLadder();
        }
    }

    private void MoveLadderBefore() //��q�̑O�܂ŃZ�b�e�B���O
    {
        MoveLadderBeforePosition();

        MoveLadderBeforeRotation();
    }

    private void MoveLadderBeforePosition() //��q�̑O�܂ňړ�
    {
        //�ړ�����������v�Z
        Vector3 moveDir = ladderStartPos - playerTrans.position;
        moveDir.y = 0;

        bool moveSmallFlg = Mathf.Abs(moveDir.magnitude) <= moveBeforeSpeed * Time.deltaTime;   //�ړ��ʂ����������邩
        if (moveSmallFlg)
        {
            moveBeforeFlg = true;
        }
        else
        {
            //�ړ��ʂ����ɂ���
            moveDir.Normalize();
            moveDir = moveDir * moveBeforeSpeed * Time.deltaTime;
        }
        playerCC.Move(moveDir);
    }

    private void MoveLadderBeforeRotation() //��q�̕��Ɍ���
    {
        RotationX();

        RotationY();
    }

    private void RotationX()    //X����]
    {
        //�v���C���[�̌������Z�o
        float camLocalRotX = cmaTrans.rotation.eulerAngles.x;

        if (camLocalRotX >= 180.0f)
        {
            camLocalRotX -= 360.0f;
        }

        //�v���C���[�̒�q���猩���p�x���Z�o
        float playerRotX = camLocalRotX - ladderStartRot.x;

        bool rotLargeFlg = Mathf.Abs(playerRotX) > rotXBeforeSpeed * Time.deltaTime;    //�p�x���傫�����邩
        if (rotLargeFlg)
        {
            //���ʉ�]����
            if (camLocalRotX > ladderStartRot.x)
            {
                cmaTrans.localRotation *= Quaternion.Euler(new Vector3(-rotXBeforeSpeed * Time.deltaTime, 0f, 0f));
            }
            else
            {
                cmaTrans.localRotation *= Quaternion.Euler(new Vector3(rotXBeforeSpeed * Time.deltaTime, 0f, 0f));
            }
        }
        else
        {
            //�҂������]����
            rotXBeforeFlg = true;
            cmaTrans.localRotation = Quaternion.Euler(new Vector3(ladderStartRot.x, 0f, 0f));
        }
    }

    private void RotationY()    //Y����]
    {
        //�v���C���[�̌������Z�o
        float playerLocalRotY = playerTrans.rotation.eulerAngles.y;

        if (playerLocalRotY >= 180.0f)
        {
            playerLocalRotY -= 360.0f;
        }

        //�v���C���[�̒�q���猩���p�x���Z�o
        float ladderRotY = ladderStartRot.y - 180.0f;
        float playerRotY = playerLocalRotY - (ladderStartRot.y - 180.0f);

        bool rotLargeFlg = Mathf.Abs(playerRotY) > rotYBeforeSpeed * Time.deltaTime;    //�p�x���傫�����邩
        if (rotLargeFlg)
        {
            //���ʉ�]����
            float tmp = playerTrans.rotation.eulerAngles.y - ladderStartRot.y;
            if (tmp < 0)
            {
                tmp += 360.0f;
            }
            if (tmp > 180.0f)
            {
                playerTrans.localRotation *= Quaternion.Euler(new Vector3(0f, -rotYBeforeSpeed * Time.deltaTime, 0f));
            }
            else
            {
                playerTrans.localRotation *= Quaternion.Euler(new Vector3(0f, rotYBeforeSpeed * Time.deltaTime, 0f));
            }
        }
        else
        {
            //�҂������]����
            rotYBeforeFlg = true;
            playerTrans.localRotation = Quaternion.Euler(new Vector3(0f, ladderStartRot.y - 180.0f, 0f));
        }
    }

    private bool MoveBeforeFinishFlg()  //��q�̃Z�b�e�B���O�I��
    {
        if (!moveBeforeFlg) return false;
        if (!rotXBeforeFlg) return false;
        if (!rotYBeforeFlg) return false;
        return true;
    }

    private void FinishLadder() //��q���I����
    {
        closeLadderFlg = false;
        touchLadderFlg = false;
        moveBeforeFlg = false;
        rotXBeforeFlg = false;
        rotYBeforeFlg = false;
    }

    private void ClimbLadder()  //��q����艺�肷��
    {
        if (playerTrans.position.y > ladderEndPos.y) return;

        //��q�̌������Z�o
        Vector3 climbVec = Vector3.zero;
        Quaternion ladderQuaX = Quaternion.Euler(-ladderStartRot.x, ladderStartRot.y, 0);

        if (Input.GetKey(KeyCode.W))    //��鏈��
        {
            climbVec = ladderQuaX * new Vector3(0, climbSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.S))    //�����鏈��
        {
            climbVec = ladderQuaX * new Vector3(0, -climbSpeed, 0);
        }

        playerCC.Move(climbVec * Time.deltaTime);
    }

    private void DescendLadder()    //��q���~��鏈��
    {
        if (!playerCC.isGrounded) return;

        if (Input.GetKey(KeyCode.S))
        {
            FinishLadder();
        }
    }

    private void GoUpLadder()   //��q��o�肫�鏈��
    {
        //��ԏ�܂œo���ĂȂ�������A���������Ȃ�
        if (playerTrans.position.y < ladderEndPos.y) return;

        //��q�̌������Z�o
        Vector3 climbVec = Vector3.zero;
        Quaternion ladderQuaX = Quaternion.Euler(0, ladderStartRot.y, 0);

        //�o�肫�鏈��
        if (Input.GetKey(KeyCode.W))
        {
            climbVec = ladderQuaX * new Vector3(0, 0, climbSpeed);  //�܂������i��
        }
        playerCC.Move(-climbVec * Time.deltaTime);

        //�v���C���[�̉��Ƀ��C���΂��A�o��؂������画�肷��
        int layerMask = 1 << 13;
        Ray ray = new Ray(playerTrans.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (!(hit.collider.gameObject.tag == "wall")) return;
            FinishLadder();
        }
    }

    public bool ClimbLadderFlg()    //��q��o���Ă��邩
    {
        if (!closeLadderFlg) return false;
        if (!touchLadderFlg) return false;
        if (!MoveBeforeFinishFlg()) return false;
        return true;
    }
}