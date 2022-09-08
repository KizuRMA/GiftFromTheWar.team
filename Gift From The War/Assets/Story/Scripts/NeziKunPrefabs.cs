using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NeziKunPrefabs : MonoBehaviour
{
    [SerializeField]
    private GameObject neziKun = null;

    // �e�I�u�W�F�N�g
    public Transform neziKunGroup;

    // �X�|�[���n�_
    public List<GameObject> spawnList = new List<GameObject>();

    // ��������v���n�u�̐�
    public int prefabNum;

    //  �l�W�N�̃C���X�^���X����
    private GameObject[] neziKunIns;

    //�@��b�\�ȑ���
    private GameObject conversationPartner;

    // Start is called before the first frame update
    void Start()
    {
        neziKunIns = new GameObject[ScenarioManager.Instance.talkCount+prefabNum];

        // �w�肳�ꂽ�ʒu�Ƀl�W�N��z�u
        for (int i = ScenarioManager.Instance.talkCount; i < (ScenarioManager.Instance.talkCount+prefabNum); i++)
        {
            neziKunIns[i] = Instantiate(neziKun, neziKunGroup);
            neziKunIns[i].transform.position = spawnList[i].transform.position;
            neziKunIns[i].name = "neziKun" + i;
        }

        for (int i = ScenarioManager.Instance.talkCount + 1; i < ScenarioManager.Instance.talkCount + prefabNum; i++)
        {
            neziKunIns[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �b���������񐔂ŕ\������l�W�N��ύX����
        switch (ScenarioManager.Instance.talkCount)
        {
            case 1:
                neziKunIns[0].SetActive(false);
                neziKunIns[1].SetActive(true);
                break;
            case 2:
                if (prefabNum < 3) return;
                neziKunIns[1].SetActive(false);
                neziKunIns[2].SetActive(true);
                break;
            case 3:

                break;
            case 4:
                neziKunIns[3].SetActive(false);
                neziKunIns[4].SetActive(true);
                break;
            case 5:
                neziKunIns[4].SetActive(false);
                break;
        }

        if (ScenarioManager.Instance.talkCount == 3 && prefabNum == 3)
        {
            neziKunIns[2].SetActive(false);
        }
    }

}
