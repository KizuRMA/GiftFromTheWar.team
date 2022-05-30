using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCapsuleScript : MonoBehaviour
{
    struct AngleDir
    {
        public Vector3 targetVec;
        public float degAngle;
    }

    public List<GameObject> colList = new List<GameObject>();


    private void Awake()
    {
    }

    // Start is called before the first frame update
    public void Start()
    {
        colList.Clear();
    }

    private void Update()
    {
        //親ゲームオブジェクトのTransform
        BatController batcon = gameObject.transform.parent.GetComponent<BatController>();

        //体を前に傾ける
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = -(batcon.forwardAngle);
        transform.localEulerAngles = _localAngle;
    }

    // Update is called once per frame
    public Vector3 MoveDirction()
    {

        AngleDir[] targetDir = new AngleDir[colList.Count];
        float[] angle = new float[colList.Count];

        //親ゲームオブジェクトのTransform
        Transform parentTrans = gameObject.transform.parent;

        //リスト内のゲームオブジェクトに対する方向を取得
        for (int i = 0; i < colList.Count; i++)
        {
            targetDir[i].targetVec = colList[i].transform.position - parentTrans.position;
            targetDir[i].targetVec.y = 0;

            float dot = Vector3.Dot(Vector3.forward, targetDir[i].targetVec.normalized);
            targetDir[i].degAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (targetDir[i].targetVec.x < 0)
            {
                targetDir[i].degAngle += 180.0f;
            }
        }

        if (colList.Count == 1) return -(targetDir[0].targetVec.normalized);

        for (int i = 0; i < (colList.Count - 1); i++)
        {
            for (int j = (colList.Count - 1); j > i; j--)
            {
                if (targetDir[j - 1].degAngle > targetDir[j].degAngle)
                {
                    AngleDir temp = targetDir[j - 1];
                    targetDir[j - 1] = targetDir[j];
                    targetDir[j] = temp;
                }
            }
        }

            //二つのベクトルから角度を出す
        for (int i = 0; i < colList.Count; i++)
        {
            float dot = Vector3.Dot(targetDir[i].targetVec.normalized, targetDir[(i + 1) % colList.Count].targetVec.normalized);
            angle[i] = Mathf.Acos(dot) * Mathf.Rad2Deg;
        }

        //一番大きい角度を出す
        float maxAngle = 0;
        int maxIndex = 0;

        for (int i = 0; i < colList.Count; i++)
        {
            if (angle[i] > maxAngle)
            {
                maxAngle = angle[i];
                maxIndex = i;
            }
        }

        Vector3 targetVec = targetDir[maxIndex].targetVec + targetDir[(maxIndex + 1) % colList.Count].targetVec;
        targetVec.Normalize();

        return targetVec;
    }

    private void OnTriggerEnter(Collider other)
    {
        //鍾乳石だった場合
        if (other.gameObject.tag == "stalactite")
        {
            if (colList.Count <= 0)
            {
                colList.Add(other.gameObject);
            }
            else
            {
                bool exist = false;
                for (int i = 0; i < colList.Count; i++)
                {
                    if (colList[i].gameObject.name == other.gameObject.name)
                    {
                        exist = true;
                    }
                }

                if (exist == false)
                {
                    colList.Add(other.gameObject);
                }
            }
        }
    }
}
