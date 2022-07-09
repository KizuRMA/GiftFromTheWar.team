using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAwake : MonoBehaviour
{
    [SerializeField] private GameObject eye;    //�ڂ̉摜
    [SerializeField] private GameObject eye2;   //�ڂ̉摜
    [SerializeField] private float moveEyeSpeed;//�ق̓�������
    private RectTransform eyeRec;               //�ڂ̉摜���
    private RectTransform eye2Rec;              //�ڂ̉摜���
    private bool awakeFinishFlg = false;        //�ڂ��o�܂��I�������

    private PlayerStartDown playerStartDown;

    void Start()
    {
        if (eye == null) return;
        eyeRec = eye.GetComponent<RectTransform>();
        eye2Rec = eye2.GetComponent<RectTransform>();
        eyeRec.localPosition = new Vector3(0, 0, 0);
        eye2Rec.localPosition = new Vector3(0, 0, 0);

        playerStartDown = transform.GetComponent<PlayerStartDown>();
    }

    void Update()
    {
        if (awakeFinishFlg || eyeRec == null) return;

        if (playerStartDown != null && playerStartDown.isAuto == true)
        {
            if (eyeRec.localPosition.y < 800 && eye2Rec.localPosition.y > -800)
            {
                //�ق𓮂���
                eyeRec.localPosition += new Vector3(0, moveEyeSpeed * Time.deltaTime, 0);
                eye2Rec.localPosition += new Vector3(0, -moveEyeSpeed * Time.deltaTime, 0);
            }
        }
        else
        {
            //�ق𓮂���
            eyeRec.localPosition += new Vector3(0, moveEyeSpeed * Time.deltaTime, 0);
            eye2Rec.localPosition += new Vector3(0, -moveEyeSpeed * Time.deltaTime, 0);

            //�ڂ��J������A�摜���\��
            if (eyeRec.localPosition.y > 900 && eye2Rec.localPosition.y < -900)
            {
                eye.SetActive(false);
                eye2.SetActive(false);

                awakeFinishFlg = true;
            }
        }
    }
}
