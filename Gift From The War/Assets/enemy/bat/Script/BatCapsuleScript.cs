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
        //�e�Q�[���I�u�W�F�N�g��Transform
        BatController batcon = gameObject.transform.parent.GetComponent<BatController>();

        //�̂�O�ɌX����
        Vector3 _localAngle = transform.localEulerAngles;
        _localAngle.x = -(batcon.forwardAngle);
        transform.localEulerAngles = _localAngle;
    }

    // Update is called once per frame
    public Vector3 MoveDirction()
    {

        AngleDir[] targetDir = new AngleDir[colList.Count];
        float[] angle = new float[colList.Count];

        //�e�Q�[���I�u�W�F�N�g��Transform
        Transform parentTrans = gameObject.transform.parent;

        //���X�g���̃Q�[���I�u�W�F�N�g�ɑ΂���������擾
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

            //��̃x�N�g������p�x���o��
        for (int i = 0; i < colList.Count; i++)
        {
            float dot = Vector3.Dot(targetDir[i].targetVec.normalized, targetDir[(i + 1) % colList.Count].targetVec.normalized);
            angle[i] = Mathf.Acos(dot) * Mathf.Rad2Deg;
        }

        //��ԑ傫���p�x���o��
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
        //�ߓ��΂������ꍇ
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
