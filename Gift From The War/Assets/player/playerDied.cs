using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDied : MonoBehaviour
{
    [SerializeField] CharacterController CC;
    private Transform trans;
    [SerializeField] GameObject rantan;
    [SerializeField] GameObject gun;
    Rigidbody rantanRD;
    Rigidbody gunRD;

    bool diedFlg = false;

    [SerializeField] float downSpeed;
    [SerializeField] float downMax;
    float downSum = 0;

    [SerializeField] float rotSpeed;
    [SerializeField] float rotMax;
    float rotSum = 0;
    [SerializeField] float gunRotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        trans = transform;
        rantanRD = rantan.GetComponent<Rigidbody>();
        gunRD = gun.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            diedFlg = true;
            CC.enabled = false;
            rantan.transform.parent = null;
            gun.transform.parent = null;
            rantan.GetComponent<Rigidbody>().useGravity = true;
            gun.GetComponent<Rigidbody>().useGravity = true;
            rantan.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gun.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        if (!diedFlg) return;

        RayJudge();

        EraseInertia();

        DownKnees();

        Fall();
    }

    private void RayJudge()
    {

    }

    private void EraseInertia()
    {
        rantanRD.velocity = Vector3.zero;
        rantanRD.angularVelocity = Vector3.zero;
        gunRD.velocity = Vector3.zero;
        gunRD.angularVelocity = Vector3.zero;
    }

    private void DownKnees()
    {
        if (downSum < downMax)
        {
            downSum += downSpeed * Time.deltaTime;
            trans.position += new Vector3(0, -downSpeed, 0) * Time.deltaTime;
            rantan.transform.position += new Vector3(0, -downSpeed, 0) * Time.deltaTime;
            gun.transform.position += new Vector3(0, -downSpeed, 0) * Time.deltaTime;
        }
    }

    private void Fall()
    {
        if (downSum < downMax) return;

        if (rotSum < rotMax)
        {
            rotSum += rotSpeed * Time.deltaTime;
            trans.rotation *= Quaternion.Euler(rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime);
            gun.transform.rotation *= Quaternion.Euler(0, 0, gunRotSpeed * Time.deltaTime);
        }
    }

    public bool GetDiedFlg()
    {
        return diedFlg;
    }
}
