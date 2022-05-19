using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    private Transform trans;
    [SerializeField] private Vector3 firstPos;
    private float posY = 0;
    private int upDown = 1; //è„Ç™ÇÈÇ©â∫Ç™ÇÈÇ©ÇÃïÑçÜÇï\Ç∑
    [SerializeField] private float upDownSpeed;
    [SerializeField] private float maxPosY;
    [SerializeField] private float dashRaito;
    private FPSController fpsC;

    // Start is called before the first frame update
    void Start()
    {
        trans = transform;
        fpsC = GameObject.Find("player").GetComponent<FPSController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fpsC.GetMoveFlg())
        {
            if (fpsC.GetDashFlg())
            {
                posY += upDownSpeed * upDown * dashRaito * Time.deltaTime;
            }
            else
            {
                posY += upDownSpeed * upDown * Time.deltaTime;
            }
            
            if (Mathf.Abs(posY) > Mathf.Abs(maxPosY))
            {
                upDown *= -1;
            }
        }
        else
        {
            if (Mathf.Abs(posY) > upDownSpeed * Time.deltaTime)
            {
                if (posY > 0)
                {
                    posY += -upDownSpeed * Time.deltaTime;
                }
                else
                {
                    posY += upDownSpeed * Time.deltaTime;
                }
            }
            else
            {
                posY = 0;
                upDown = 1;
            }
        }

        trans.localPosition = firstPos + new Vector3(0.0f, posY, 0.0f);
    }
}
