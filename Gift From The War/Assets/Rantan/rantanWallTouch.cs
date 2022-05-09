using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rantanWallTouch : MonoBehaviour
{
    Transform trans;
    Vector3 force;
    Vector3 firstPos;
    Rigidbody rd;

    // Start is called before the first frame update
    void Start()
    {
        trans = transform;
        rd = this.GetComponent<Rigidbody>();
        firstPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(firstPos.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "noMove")
        {
            float boundsPower = 0.1f;

            // �Փˈʒu���擾����
            Vector3 hitPos = collision.contacts[0].point;

            // �Փˈʒu���玩�@�֌������x�N�g�������߂�
            Vector3 boundVec = this.transform.position - hitPos;

            // �t�����ɂ͂˂�
            Vector3 forceDir = boundsPower * boundVec.normalized;
            this.GetComponent<Rigidbody>().AddForce(forceDir, ForceMode.Impulse);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //trans.localPosition = firstPos;
    }
}
