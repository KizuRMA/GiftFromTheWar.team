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

            // 衝突位置を取得する
            Vector3 hitPos = collision.contacts[0].point;

            // 衝突位置から自機へ向かうベクトルを求める
            Vector3 boundVec = this.transform.position - hitPos;

            // 逆方向にはねる
            Vector3 forceDir = boundsPower * boundVec.normalized;
            this.GetComponent<Rigidbody>().AddForce(forceDir, ForceMode.Impulse);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //trans.localPosition = firstPos;
    }
}
