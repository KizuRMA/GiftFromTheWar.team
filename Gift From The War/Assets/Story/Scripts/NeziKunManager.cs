using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NeziKunManager : MonoBehaviour
{
    // �e�I�u�W�F�N�g�̃��X�g
    [SerializeField] public List<Transform> parentList= new List<Transform>();

    //�X�|�[�����X�g
    [SerializeField] private List<NeziKunSpawnList> list = null;

    private int parentCount=0;
    private void Awake()
    {
        foreach (var neziKunList in list)
        {
            //�X�|�[���n�_
            NeziKunSpawn neziKuns = neziKunList.spawnLists[0];

            //�w�肵���ʒu�Ƀl�W�N����
            GameObject game = Instantiate(neziKuns.spawnType);
            game.transform.position = neziKuns.spawnPos.transform.position;

            //�e�q�֌W�ݒ�
            game.transform.parent = parentList[parentCount];
            game.name = "NeziKun" + (parentCount+1);

            if(parentCount<parentList.Count)
            {
                parentCount++;
            }
        }

        

    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
       switch(ScenarioManager.Instance.talkCount)
        {
            case 1:
                list[0].gameObject.SetActive(false);
                list[1].gameObject.SetActive(true);
                break;
            case 2:
                list[1].gameObject.SetActive(false);
                break;

        }
    }

}
