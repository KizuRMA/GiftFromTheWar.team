using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanMove : MonoBehaviour
{
    private Transform trans;
    private FPSController fpsC;
    private Transform camTrans;
    private Quaternion firstQua;
    private int upDown = -1;
    [SerializeField] private Vector3 firstPos;
    private float posY = 0;
    [SerializeField] private float upDownSpeed;
    [SerializeField] private float maxPosY;
    [SerializeField] private float dashRaito;
    [SerializeField] private float movePower = 0.0001f;
    [SerializeField] private float upRaito = 0; //è„ÇÃåXÇ´ÇÃï‚ê≥î{ó¶
    [SerializeField] private float downRaito = 0; //è„ÇÃåXÇ´ÇÃï‚ê≥î{ó¶

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        camTrans = GameObject.Find("Main Camera").GetComponent<Transform>();
        fpsC = GameObject.Find("player").GetComponent<FPSController>();
        firstQua = trans.localRotation;
        firstPos = trans.localPosition;
        firstPos.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rotation();
        tremor();
    }

    void rotation()
    {
        Quaternion _ranQua = Quaternion.identity;

        //ÉJÉÅÉâÇÃÉNÉIÅ[É^ÉjÉIÉìílÇéÊìæ
        Quaternion _camQua = camTrans.rotation;

        _ranQua = trans.rotation;

        float _camEulerAngleX = _camQua.eulerAngles.x;

        //äpìxÇí≤êÆÇ∑ÇÈ
        if (_camEulerAngleX >= 300.0f)
        {
            _camEulerAngleX -= 360.0f;
            _camEulerAngleX *= upRaito;
        }
        else
        {
            _camEulerAngleX *= downRaito;
        }

        _camEulerAngleX *= -1;

        _ranQua = Quaternion.AngleAxis(_camEulerAngleX, Vector3.right);
        trans.localRotation = _ranQua * firstQua;
    }

    void tremor()
    {
        if (fpsC.GetMoveFlg())
        {
            if (fpsC.GetDashFlg())
            {
                posY += upDownSpeed * upDown * dashRaito;
            }
            else
            {
                posY += upDownSpeed * upDown;
            }

            if (Mathf.Abs(posY) > Mathf.Abs(maxPosY))
            {
                upDown *= -1;
            }
        }
        else
        {
            if (Mathf.Abs(posY) > upDownSpeed * 1.1f)
            {
                if (posY > 0)
                {
                    posY += -upDownSpeed;
                }
                else
                {
                    posY += upDownSpeed;
                }
            }
            else
            {
                posY = 0;
                upDown = -1;
            }
        }

        //trans.localPosition = firstPos + new Vector3(0.0f, posY, 0.0f);
    }
}
