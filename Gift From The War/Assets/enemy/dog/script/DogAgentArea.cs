using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAgentArea : MonoBehaviour
{
    [SerializeField] GameObject stage;
    [SerializeField] List<string> useAgentNames;
    [SerializeField] NavMeshModifierVolume volume;
    GameObject baseDogObject = null;


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
        if (baseDogObject == null) return;

        if (this != baseDogObject.GetComponent<UsingDogArea>().area)
        {
            baseDogObject = null;
            volume.AllRemoveAffectsAgentType();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Dog1" || baseDogObject != null) return;

        baseDogObject = other.gameObject;
        baseDogObject.GetComponent<UsingDogArea>().area = this;
        NavMeshAgent agent = baseDogObject.GetComponent<NavMeshAgent>();
        string _agentName = NavMesh.GetSettingsNameFromID(agent.agentTypeID);

        foreach (string str in navMeshes.Keys)
        {
            if (_agentName != str)
            {
                volume.AddAffectsAgentType(navMeshes[str].agentTypeID);
                navMeshes[str].BuildNavMesh();
            }
        }
    }
}
