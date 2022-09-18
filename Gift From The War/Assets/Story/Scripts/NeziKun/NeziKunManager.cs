using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class NeziKunManager : MonoBehaviour
{
    // �e�I�u�W�F�N�g�̃��X�g
    [SerializeField] public List<Transform> parentList= new List<Transform>();

    //�X�|�[�����X�g
    [SerializeField] private List<NeziKunSpawn> list = null;
 
    // Start is called before the first frame update
    void Start()
    {
        int index = ScenarioData.Instance.saveData.neziKunCount;

        for (int i = index; i < list.Count; i++)
        {
            //�w�肵���ʒu�Ƀl�W�N����
            GameObject game = Instantiate(list[i].neziKun);
            game.transform.position = list[i].spawn.position;

            //�e�q�֌W�ݒ�
            game.transform.parent = parentList[i];
            game.name = "NeziKun" + i;
        }


        for (int i = index + 1; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Spawn()
    {
        int index = ScenarioManager.Instance.neziKunCount;

        if (index < list.Count)
        {
            list[index].gameObject.SetActive(true);
        }
    }
}