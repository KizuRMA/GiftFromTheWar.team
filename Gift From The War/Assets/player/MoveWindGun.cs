using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindGun : MonoBehaviour
{
    CharacterController CC;
    Transform trans;
    [SerializeField] GameObject cam;
    [SerializeField] playerHundLadder ladder;
    Quaternion viewpoint;

    [SerializeField] float movePower;
    [SerializeField] float movePowerMin;
    [SerializeField] float range;
    float disRaitoPower;

    bool groundFlg = false;
    float airResistance;
    [SerializeField] float airResistancePower;
    [SerializeField] float airResistanceMax;
    [SerializeField] float airResistanceMin;

    Vector3 power;
    bool upWindFlg = false;
    float gravity;

    // Start is called before the first frame update
    void Start()
    {
        CC = this.GetComponent<CharacterController>();
        trans = transform;
        gravity = this.GetComponent<FPSController>().GetGravity();
    }

    // Update is called once per frame
    void Update()
    {
        if (ladder.GetTouchLadderFlg()) return;

        KnowViewpoint();

        Move();
    }

    private void KnowViewpoint() //‚Ç‚±‚Þ‚¢‚Ä‚¢‚é‚©
    {
        if (!Input.GetMouseButton(0)) return;

        viewpoint = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x, trans.localRotation.eulerAngles.y, 0);
    }

    private void Move()
    {
        if (!Input.GetMouseButton(0))
        {
            upWindFlg = false;

            if (CC.isGrounded)
            {
                groundFlg = true;
            }

            if (groundFlg)
            {
                airResistance -= airResistancePower * Time.deltaTime;
            }

            if(airResistance < 0)
            {
                airResistance = 0;
                groundFlg = false;
            }

            power = viewpoint * new Vector3(0, 0, -movePower) * airResistance * Time.deltaTime;
            power.y = 0;
            CC.Move(power);
            return;
        }

        upWindFlg = true;
        CorrectionDis();

        power = viewpoint * new Vector3(0, 0, -movePower) * disRaitoPower * Time.deltaTime;

        if (cam.transform.localRotation.eulerAngles.x < 40)
        {
            upWindFlg = false;
        }
        CC.Move(power);
    }

    private void CorrectionDis()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        int layerMask = 1 << 9;
        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            disRaitoPower = 1.0f - hit.distance / range + movePowerMin;

            airResistance = disRaitoPower;
            if (airResistance < airResistanceMin)
            {
                airResistance = airResistanceMin;
            }

            if(airResistance > airResistanceMax)
            {
                airResistance = airResistanceMax;
            }
        }
        else
        {
            disRaitoPower = 0;
        }
    }

    public bool GetUpWindGunFlg()
    {
        return upWindFlg;
    }

    public Vector3 GetPower()
    {
        return power;
    }
}