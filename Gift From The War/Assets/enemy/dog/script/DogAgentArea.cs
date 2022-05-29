using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAgentArea : MonoBehaviour
{
    [SerializeField] GameObject dog;
    [SerializeField] GameObject stage;
    [SerializeField] List<string> useAgentNames;

    NavMeshSurface[] navMesh;
    Dictionary<string,NavMeshSurface> navMeshes = new Dictionary<string,NavMeshSurface>();


    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�W�ɂ���S�Ă�NavMeshSurface�̏����擾����
        navMesh = stage.GetComponents<NavMeshSurface>();

        for (int i = 0; i < navMesh.Length; i++) 
        {
            //NavMeshSurface����Agent�̖��O���擾����
            string _name = NavMesh.GetSettingsNameFromID(navMesh[i].agentTypeID);

            //�g�p����Agent�̖��O��S�ĎQ�Ƃ��邽�߂̃��[�v
            foreach (string str in useAgentNames)
            {
                //���O����v���镨���������ꍇ��Dictionary�z��ɒǉ�����
                if (str == _name)
                {
                    navMeshes.Add(str, navMesh[i]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = dog.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //���Ɠ��������Ƃ��̂ݏ������s
        Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag != "Dog1") return;

        GameObject dogObject = other.gameObject;
        NavMeshAgent agent = dogObject.GetComponent<NavMeshAgent>();
        string _agentName = NavMesh.GetSettingsNameFromID(agent.agentTypeID);

        Debug.Log(11111111111111);

        foreach (string str in navMeshes.Keys)
        {
            if (_agentName != str)
            {
                navMeshes[str].BuildNavMesh();
            }
        }
    }
}
