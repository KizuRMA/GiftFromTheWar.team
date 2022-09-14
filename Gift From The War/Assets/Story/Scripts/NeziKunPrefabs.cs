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
        neziKunIns = new GameObject[prefabNum];

        // �w�肳�ꂽ�ʒu�Ƀl�W�N��z�u
        for (int i = 0; i < (prefabNum); i++)
        {
            neziKunIns[i] = Instantiate(neziKun, neziKunGroup);
            neziKunIns[i].transform.position = spawnList[i].transform.position;
            neziKunIns[i].name = "neziKun" + i;
        }

        for (int i =1; i < prefabNum; i++)
        {
            neziKunIns[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (ScenarioManager.Instance.talkCount)
        {
            case 1:
                neziKunIns[0].SetActive(false);
                break;
        }
    }

}
